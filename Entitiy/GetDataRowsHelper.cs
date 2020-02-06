using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace DBCommon
{
    public static class GetDataRowsHelper
    {
        public static IEnumerable<EntityBase> GetDataRows<T>(DbSet<T> table, int p1, int p2)
            where T : EntityBase, new()
        {
            var p2Str = $"{p2} ";
            return (from e in table where e.BatchId == p1 && e.RowNo.StartsWith(p2Str) select e);
        }
    }
}
