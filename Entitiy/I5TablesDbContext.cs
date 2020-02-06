using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DBCommon
{
    public interface I5TablesDbContext
    {
        IEnumerable<Entity1> Table1 { get; set; }
        IEnumerable<Entity2> Table2 { get; set; }
        IEnumerable<Entity3> Table3 { get; set; }
        IEnumerable<Entity4> Table4 { get; set; }
        IEnumerable<Entity5> Table5 { get; set; }
    }
}
