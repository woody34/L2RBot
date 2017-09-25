using System.Drawing;

namespace L2RBot
{
    class Pixel
    {
        public int X;
        public int Y;
        public int A;
        public int R;
        public int G;
        public int B;
        public Color Color;
        public Point Point;
        public Pixel(Color Col, Point Loc)
        {
            X = Loc.X;
            Y = Loc.Y;
            A = Col.A;
            R = Col.R;
            G = Col.G;
            B = Col.B;
            Point = Loc;
            Color = Col;
        }
        public bool IsPresent(Rectangle _Screen, int Tolerance)
        {
            return L2RBot.Screen.CompareColor(Color, L2RBot.Screen.GetColor(_Screen, X, Y), Tolerance);
        }
        public Color CurrentValue(Rectangle _Screen)
        {
            return Screen.GetColor(_Screen, Point.X, Point.Y);
        }
        public void UpdateColor(Rectangle _Screen)
        {
            Color = CurrentValue(_Screen);
            A = Color.A;
            R = Color.R;
            G = Color.G;
            B = Color.B;
        }
        override public string ToString()
        {
            return "(" + X + ", " + Y + ")TargetL[" + A + "," + R + "," + G + "," + B + "]";
        }
    }
}
