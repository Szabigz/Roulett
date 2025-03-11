using System;
using System.Collections.Generic;
using System.Linq;

namespace Roulett
{

    public class RouletteWheel
    {
        public List<RouletteNumber> Numbers { get; private set; }

        public RouletteWheel()
        {
            Numbers = new List<RouletteNumber>();

            // Az európai rulett számok elrendezése az óramutató járásával megegyező irányban
            int[] order = { 0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18, 29, 7, 28, 12, 35, 3, 26 };

            double angleStep = 360.0 / order.Length; // Egy számhoz tartozó szögtartomány

            for (int i = 0; i < order.Length; i++)
            {
                double startAngle = i * angleStep;
                double endAngle = startAngle + angleStep;

                string color = (order[i] == 0) ? "Green" :
                               new[] { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 }.Contains(order[i]) ? "Red" : "Black";

                string parity = (order[i] == 0) ? "None" :
                                (order[i] % 2 == 0) ? "Even" : "Odd";

                Numbers.Add(new RouletteNumber
                {
                    Number = order[i],
                    Color = color,
                    Parity = parity,
                    AngleStart = startAngle,
                    AngleEnd = endAngle
                });
            }
        }

        public int GetNumberByAngle(double angle)
        {
            foreach (var number in Numbers)
            {
                if (angle >= number.AngleStart && angle < number.AngleEnd)
                {
                    return number.Number;
                }
            }
            return -1; // Hiba esetén
        }
    }

}
