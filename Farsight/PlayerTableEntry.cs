using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farsight
{
    public class PlayerTableEntry : IPlayerTableEntry
    {
        public bool? Active { get; set; }
        public string? Name { get; set; }
        public string? World { get; set; }

        public string? Colour { get; set; }
    }
}
