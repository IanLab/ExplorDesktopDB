using Microsoft.EntityFrameworkCore;
using System;

namespace DBCommon
{
    public static class AddDataRowsHelper
    {

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
            var dataRow = new T
            {
                Id = $"{batchId} {r}",
                RowNo = r,
                P3 = $"{r}",
                P4 = r,
                P5 = r,
                P6 = r,
                BatchId = batchId,
                P7 = DateTime.Now,
                P8 = DateTime.Now,
                P9 = DateTime.Now, 
                UpdatedDateTime = DateTime.Now, 
                UpdatedUserName = Environment.UserName
            };

            return dataRow;
        }
    }
}
