using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulett
{
    public class RouletteNumber
    {
        public int Number { get; set; }      // A rulett szám
        public double AngleStart { get; set; }  // A szög kezdete
        public double AngleEnd { get; set; }    // A szög vége
        public string Color { get; set; }    // Szín (Red, Black, Green)
        public string Parity { get; set; }   // Paritás (Even, Odd)
        public double Angle { get; set; }  // Angle on the wheel

    }

}
