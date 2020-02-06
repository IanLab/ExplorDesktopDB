using Microsoft.EntityFrameworkCore;
using System;

namespace DBCommon
{
    public static class AddDataRowsHelper
    {
        public static void Add1DataRow<T>(DbSet<T> table)
            where T : EntityBase, new()
        {
            table.Add(new T
            {
                RowNo = "AName",
                BatchId = 1,
                P3 = "p3",
                P4 = 4,
                P5 = 5,
                P6 = 6,
                P7 = DateTime.Now,
                P8 = DateTime.Now,
                P9 = DateTime.Now
            });
        }

        public static void Add1BatchDataRows<T>(DbSet<T> dbSet, 
            int batchId, 
            int rowCount1Batch = 5000)
            where T : EntityBase, new()
        {
            if (dbSet is null)
            {
                throw new ArgumentNullException(nameof(dbSet));
            }

            var rows = new T[rowCount1Batch];
            for (int r = 0; r < rowCount1Batch; r++)
            {
                rows[r] = CreateDataRow<T>(batchId, r);
            }
            dbSet.AddRange(rows);
        }

        public static T CreateDataRow<T>(int batchId, int r) where T : EntityBase, new()
        {
            var rowNo = $"{r} RowNo.";
            var dataRow = new T
            {
                Id = $"{batchId} {rowNo}",
                RowNo = rowNo,
                P3 = rowNo,
                P4 = r,
                P5 = r,
                P6 = r,
                BatchId = batchId,
                P7 = DateTime.Now,
                P8 = DateTime.Now,
                P9 = DateTime.Now
            };

            return dataRow;
        }
    }
}
