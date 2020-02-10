using System.Collections.Generic;

namespace DBCommon
{
    public interface IRepository
    {
        void Save(ICommandAble entity);
    }
}
