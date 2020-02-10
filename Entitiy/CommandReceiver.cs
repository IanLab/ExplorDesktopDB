using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace DBCommon
{
    public sealed class CommandReceiver : IDisposable
    {
        private ISerializer _ser;
        private FileSystemWatcher _fileSystemWatcher;
        private readonly BlockingCollection<ICommandAble> _queue;
        private readonly Task _executeCmmdTask;
        private readonly IRepository _repository;        

        public CommandReceiver(ISerializer ser, IRepository rep)
        {
            _repository = rep;
            _ser = ser ?? throw new ArgumentNullException(nameof(ser));
            _queue = new BlockingCollection<ICommandAble>();
            _fileSystemWatcher = new FileSystemWatcher();
            _fileSystemWatcher.Path = CommandFile.SharedFolder;
            _fileSystemWatcher.Filter = $"*{CommandFile.FileExtension}";
            _fileSystemWatcher.Created += _fileSystemWatcher_Created;
            _executeCmmdTask = new Task(ExecuteCmmd);
        }

        public bool IsRuning { get { return _executeCmmdTask.Status == TaskStatus.Running; } }

        public void Start()
        {
            _executeCmmdTask.Start();
            _fileSystemWatcher.EnableRaisingEvents = true;
            
        }

        public void Stop()
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
            if (!_queue.IsCompleted)
            {
                _queue.CompleteAdding();
            }
        }

        private void ExecuteCmmd()
        {
            ICommandAble entity;
            while(_queue.TryTake(out entity)||_queue.IsCompleted == false)
            {
                if (entity != null)
                {
                    _repository.Save(entity);
                }
            }
        }

        private void _fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            int retryCount = 0;
            for (retryCount = 0; retryCount < 5; retryCount++)
            {
                try
                {
                    var cmmdStr = File.ReadAllText(e.FullPath);
                    var cmmd = _ser.DesSer(cmmdStr);
                    if (!(cmmd is CompleteEntity))
                    {
                        _queue.Add(cmmd);
                    }
                    else
                    {
                        _queue.CompleteAdding();
                        _fileSystemWatcher.EnableRaisingEvents = false;
                    }
                    break;
                }
                catch (System.IO.IOException exp)
                {
                    System.Diagnostics.Debug.WriteLine(exp);
                }
            }
            if(retryCount == 5)
            {
                throw new Exception();
            }
            System.Diagnostics.Debug.WriteLine($"Retry count {retryCount}");
        }

        private bool _disposed = false;
        private void Dispose(bool disposing)
        {
            if(_disposed == false)
            {
                if(disposing)
                {
                    _fileSystemWatcher.Dispose();
                    _executeCmmdTask.Dispose();
                    _queue.Dispose();
                    _disposed = true;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        ~CommandReceiver()
        {
            Dispose(false);
        }
    }
}
