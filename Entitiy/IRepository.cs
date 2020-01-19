using System.Collections.Generic;

namespace DBCommon
{
    public interface IRepository
    {
        IEnumerable<EntityBase> GetEntities(int batchId, int aNamePart1);
    }
}
