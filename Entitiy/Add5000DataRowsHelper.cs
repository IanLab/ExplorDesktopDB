using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;

namespace RepositoryCommon
{
    

    public static class GetDataRowsHelper
    {
        public static IEnumerable<EntityBase> GetDataRows<T>(DbSet<T> table, int p1, int p2)
            where T : EntityBase, new()
        {
            var p2Str = $"{p2} ";
            return (from e in table where e.BatchId == p1 && e.AName.StartsWith(p2Str) select e);
        }
    }

    public static class AddDataRowsHelper
    {
        public static void Add1DataRow<T>(DbSet<T> table)
            where T : EntityBase, new()
        {
            table.Add(new T { 
                Id = Guid.NewGuid(), 
                AName = "AName",
                BatchId = 1, 
                P3 = "p3", 
                P4 = 4, 
                P5 = 5, 
                P6 = 6, 
                P7 = DateTime.Now, 
                P8 = DateTime.Now, 
                P9 = DateTime.Now });
        }

        public static void Add5000DataRows<T>(DbSet<T> dbSet, int batchId)
            where T: EntityBase, new()
        {
            if (dbSet is null)
            {
                throw new ArgumentNullException(nameof(dbSet));
            }

            const int aNamePart2Max = 5000;
            const int aNamePart1Max = 10;
            var ebss = new T[aNamePart2Max][];
            for (int aNamePart2 = 0; aNamePart2 < aNamePart2Max; aNamePart2++)
            {
                ebss[aNamePart2] = new T[aNamePart1Max];
                for (int aNamePart1 = 0; aNamePart1 < aNamePart1Max; aNamePart1++)
                {
                    var aName = $"{aNamePart1} {aNamePart2}";
                    ebss[aNamePart2][aNamePart1] = new T
                    {
                        Id = Guid.NewGuid(),
                        AName = aName,
                        P3 = aName,
                        P4 = aNamePart1,
                        P5 = aNamePart1,
                        P6 = aNamePart2,
                        BatchId = batchId,
                        P7 = DateTime.Now,
                        P8 = DateTime.Now,
                        P9 = DateTime.Now
                    };
                }
            }
            dbSet.AddRange((from ebs in ebss from eb in ebs select eb));
        }
    }
}
