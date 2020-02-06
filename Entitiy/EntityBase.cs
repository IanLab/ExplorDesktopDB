using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBCommon
{
    public abstract class EntityBase 
    {
        [Key]
        public string Id { get; set; }
        public int BatchId { get; set; }
        public string RowNo { get; set; }
        public string P3 { get; set; }
        public double P4 { get; set; }
        public double P5 { get; set; }
        public double P6 { get; set; }
        public DateTime P7 { get; set; }
        public DateTime P8 { get; set; }
        public DateTime P9 { get; set; }
    }
}
