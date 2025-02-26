using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulett
{

    public class RouletteWheel
    {
        public List<RouletteNumber> Numbers { get; set; }

        public RouletteWheel()
        {
            Numbers = new List<RouletteNumber>();

            // Rulett számok és azokhoz tartozó színek és típusok
            int[] redNumbers = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
            int[] blackNumbers = { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 };
            int[] evenNumbers = { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36 };
            int[] oddNumbers = { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35 };

            // Hozzáadjuk az összes számot és azokhoz a színt, paritást, és szöget
            for (int i = 0; i < 37; i++)
            {
                RouletteNumber rouletteNumber = new RouletteNumber
                {
                    Number = i,
                    Angle = i * (360 / 37),  // Szög elosztása
                };

                // Szín
                if (i == 0)
                {
                    rouletteNumber.Color = "Green";
                }
                else if (Array.Exists(redNumbers, element => element == i))
                {
                    rouletteNumber.Color = "Red";
                }
                else if (Array.Exists(blackNumbers, element => element == i))
                {
                    rouletteNumber.Color = "Black";
                }

                // Paritás
                if (Array.Exists(evenNumbers, element => element == i))
                {
                    rouletteNumber.Parity = "Even";
                }
                else if (Array.Exists(oddNumbers, element => element == i))
                {
                    rouletteNumber.Parity = "Odd";
                }

                Numbers.Add(rouletteNumber);
            }
        }
    }

}
