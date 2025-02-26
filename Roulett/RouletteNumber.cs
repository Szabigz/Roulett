using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulett
{
    public class RouletteNumber
    {
        public int Number { get; set; }
        public string Color { get; set; }  // "Red", "Black", "Green"
        public string Parity { get; set; } // "Even" or "Odd"
        public double Angle { get; set; }  // Angle on the wheel
    }
}
