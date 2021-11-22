using System;
using System.Collections.Generic;
using System.Text;

namespace GraphTheory
{
    public class Edge
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return string.Format("X: {0} Y: {1} SúlY: {2}", X, Y, Value);
        }
        public bool XEqualsY()
        {
            if (X == Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsNewCoordADuplicate(List<Edge> list)
        {
            Edge reversedCoord = new Edge
            {
                X = Y,
                Y = X
            };

            foreach (Edge i in list)
            {
                if (reversedCoord.ToString() == i.ToString())
                {
                    return true;
                }
                else if (i.X == X && i.Y == Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
