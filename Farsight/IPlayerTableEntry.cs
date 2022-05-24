using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farsight
{
    public interface IPlayerTableEntry
    {
        bool? Active { get; set; }
        string? Name { get; set; }
        string? World { get; set; }

        string? Colour { get; set; }
    }
}
