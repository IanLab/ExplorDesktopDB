using RepositoryCommon;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryNetCore
{
    //public class MultiTableNetCoreRepository : IRepository
    //{
    //    private readonly bool _isDBFileInLocalFolder;

    //    public MultiTableNetCoreRepository(bool isDBFileInLocalFolder)
    //    {
    //        _isDBFileInLocalFolder = isDBFileInLocalFolder;
    //    }

    //    public IEnumerable<EntityBase> GetEntities(int p1, int p2)
    //    {
    //        var p2Str = $"{p2} ";
    //        using (var dbc = new MultiTableNetCoreDBContext(_isDBFileInLocalFolder))
    //        {
    //            return (from e in dbc.Table1 where e.BatchId == p1 && e.AName.StartsWith(p2Str) select e);
    //        }
    //    }
    //}
}
