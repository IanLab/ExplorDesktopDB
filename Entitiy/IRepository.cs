using System.Collections.Generic;

namespace RepositoryCommon
{
    public interface IRepository
    {
        IEnumerable<EntityBase> GetEntities(int batchId, int aNamePart1);
    }
}
