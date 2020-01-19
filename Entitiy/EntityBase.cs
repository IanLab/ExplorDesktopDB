using System;

namespace DBCommon
{
    public abstract class EntityBase 
    {
        public Guid Id { get; set; }
        public int BatchId { get; set; }
        public string AName { get; set; }
        public string P3 { get; set; }
        public double P4 { get; set; }
        public double P5 { get; set; }
        public double P6 { get; set; }
        public DateTime P7 { get; set; }
        public DateTime P8 { get; set; }
        public DateTime P9 { get; set; }
    }
}
