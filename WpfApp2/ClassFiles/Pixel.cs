using System.Drawing;

namespace L2RBot
{
    class Pixel
    {
        public Color Color { get; set; }

        public Point Point { get; set; }

        ////public Pixel(Color Col, Point Loc)
        ////{
        ////    Point = Loc;
        ////    Color = Col;
        ////}
        public bool IsPresent(Rectangle _Screen, int Tolerance)
        {
            return L2RBot.Screen.CompareColor(Color, L2RBot.Screen.GetColor(_Screen, Point.X, Point.Y), Tolerance);
        }
        public Color CurrentValue(Rectangle _Screen)
        {
            return Screen.GetColor(_Screen, Point.X, Point.Y);
        }
        public void UpdateColor(Rectangle _Screen)
        {
            Color = CurrentValue(_Screen);
        }
        override public string ToString()
        {
            return "(" + Point.X + ", " + Point.Y + ")TargetL[" + Color.A + "," + Color.R + "," + Color.G + "," + Color.B + "]";
        }
    }
}
