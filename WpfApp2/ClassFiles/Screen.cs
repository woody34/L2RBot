using L2RBot.Common.Enum;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;




namespace L2RBot
{
    public class Screen
    {
        //Fields
        private Emulator _emu;

        private int _topBorder;

        private int _bottomBorder;

        private int _leftBorder;

        private int _rightBorder;

        private int _borderWidth;

        private int _borderHeight;

        //Properties
        public Emulator Emu
        {
            get
            {
                if(MainWindow.main != null)
                {
                    if (_emu == Emulator.None)
                    {
                        bool BS = false;

                        bool Nox = false;

                        bool MEmu = false;

                        MainWindow.main.Dispatcher.Invoke(new Action(() => BS = MainWindow.main.CbItemBS.IsSelected));

                        MainWindow.main.Dispatcher.Invoke(new Action(() => Nox = MainWindow.main.CbItemNox.IsSelected));

                        MainWindow.main.Dispatcher.Invoke(new Action(() => MEmu = MainWindow.main.CbItemMEmu.IsSelected));

                        if (BS)
                        {
                            _emu = Emulator.BlueStacks;
                        }

                        if (Nox)
                        {
                            _emu = Emulator.NoxPlayer;
                        }

                        if (MEmu)
                        {
                            _emu = Emulator.MEmu;
                        }
                    }
                }
                
                return _emu;
            }
            set
            {
                _emu = value;
            }
        }

        public int TopBorder
        {
            get
            {
                return _topBorder;
            }
            set
            {
                _topBorder = value;
            }
        }

        public int BottomBorder
        {
            get
            {
                return _bottomBorder;
            }
            set
            {
                _bottomBorder = value;
            }
        }

        public int LeftBorder
        {
            get
            {
                return _leftBorder;
            }
            set
            {
                _leftBorder = value;
            }
        }

        public int RightBorder
        {
            get
            {
                return _rightBorder;
            }
            set
            {
                _rightBorder = value;
            }
        }

        public int BorderWidth
        {
            get
            {
                return _borderWidth;
            }
            set
            {
                _borderWidth = value;
            }
        }

        public int BorderHeight
        {
            get
            {
                return _borderHeight;
            }
            set
            {
                _borderHeight = value;
            }
        }

        //Constructor
        public Screen()
        {
            BuildBorders();
        }

        public void BuildBorders()
        {
            if (Emu == Emulator.NoxPlayer)
            {
                TopBorder = 30;

                BottomBorder = 2;

                LeftBorder = 2;

                RightBorder = 2;
            }

            if (Emu == Emulator.BlueStacks)
            {
                TopBorder = 47;

                BottomBorder = 47;

                LeftBorder = 7;

                RightBorder = 7;
            }

            if (Emu == Emulator.MEmu)
            {
                TopBorder = 31;

                BottomBorder = 1;

                LeftBorder = 1;

                RightBorder = 37;
            }

            BorderWidth = LeftBorder + RightBorder;

            BorderHeight = TopBorder + BottomBorder;
        }

        //Logic
        /// <summary>
        /// takes a process object and returns its the game screens x, y, width and height
        /// </summary>
        /// <param name="App"></param>
        /// <returns></returns>
        public Rectangle GetRect(Process App)
        {
            if(Emu == Emulator.None)
            {
                BuildBorders();
            }

            var RECT = new User32.Rect();

            User32.GetWindowRect(App.MainWindowHandle, ref RECT);

            int width = RECT.Right - RECT.Left;

            int height = RECT.Bottom - RECT.Top;

            Rectangle Screen = new Rectangle(RECT.Left, RECT.Top, width, height);

            CheckScreenSize(App);

            return Screen;
        }

        private void CheckScreenSize(Process App)
        {
            var RECT = new User32.Rect();

            User32.GetWindowRect(App.MainWindowHandle, ref RECT);

            int width = RECT.Right - RECT.Left;

            int height = RECT.Bottom - RECT.Top;

            Rectangle Screen = new Rectangle(RECT.Left, RECT.Top, width, height);

            if (TopBorder + BottomBorder + LeftBorder + RightBorder != 0)//checks to see if boarders have been assigned values before spamming error.
            {
                if (Screen.Width != (1280 + RightBorder + LeftBorder) | Screen.Height != (720 + TopBorder + BottomBorder))
                {
                    MainWindow.main.UpdateLog = App.MainWindowTitle + " Incorrect game screen size detected. The visible game screen needs to be 1280x720, excluding any borders. Window detected at:" +
                        width + "x" + height + ". It needs to be " + (1280 + RightBorder + LeftBorder) + "x" + (720 + TopBorder + BottomBorder) +
                        ".";
                }
            }
        }

        /// <summary>
        /// converts game x, y points to windows x, y points for clicking purposes.
        /// </summary>
        /// <param name="Rect"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public Point PointToScreenPoint(Rectangle Rect, int X, int Y)
        {
            int x;

            int y;

            x = Rect.X + X + LeftBorder;

            y = Rect.Y + Y + TopBorder;

            return new Point(x, y);
        }

        /// <summary>
        /// returns a color at a games screen x and y location
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public Color GetColor(Rectangle rect, int X, int Y)
        {

            Point point = PointToScreenPoint(rect, X, Y);

            int x = point.X;

            int y = point.Y;

            IntPtr desk = User32.GetDesktopWindow();

            IntPtr dc = User32.GetWindowDC(desk);

            int a = (int) User32.GetPixel(dc, x, y);

            User32.ReleaseDC(desk, dc);

            return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
        }

        /// <summary>
        /// Compares two colors with a tolerance values
        /// </summary>
        /// <param name="Pixel"></param>
        /// <param name="Find"></param>
        /// <param name="Tolerance"></param>
        /// <returns>returns true if color values align and false if they do not</returns>
        public static Boolean CompareColor(Color Pixel, Color Find, int Tolerance)
        {
            int r = Pixel.R - Find.R;

            r = Math.Abs(r);

            int g = Pixel.G - Find.G;

            g = Math.Abs(g);

            int b = Pixel.B - Find.B;

            b = Math.Abs(b);

            int rgb = r + g + b;

            int tol = Tolerance * 3;

            return (rgb <= tol) ? true : false;
        }

        /// <summary>
        /// looks through a single column of pixels for a specific pixel.
        /// </summary>
        /// <param name="Screen">The game screen location.</param>
        /// <param name="Start">The point at which we begin searching.</param>
        /// <param name="StrideLength">A possitive integer value the represents the distance of pixels to be searched through in the stride.</param>
        /// <param name="Color">The color we are searching for.</param>
        /// <param name="Tolerance">A threashold for pixel variation.</param>
        /// <returns></returns>
        public static Pixel SearchPixelVerticalStride(Rectangle Screen, Point Start, uint StrideLength, Color Color, int Tolerance = 0)
        {
            for (int i = 0; i <= StrideLength; i++)
            {
                Pixel p = new Pixel
                {
                    Color = Color,

                    Point = new Point(Start.X, Start.Y + i)
                };

                if (p.IsPresent(Screen, Tolerance))
                {
                    return p;
                }
            }

            return new Pixel();
        }

        /// <summary>
        /// looks through a single column of pixels for a specific pixel.
        /// </summary>
        /// <param name="Screen">The game screen location.</param>
        /// <param name="Start">The point at which we begin searching.</param>
        /// <param name="StrideLength">A possitive integer value the represents the distance of pixels to be searched through in the stride.</param>
        /// <param name="Color">The color we are searching for.</param>
        /// <param name="IsFound">Returns true if the pixel is found in the stride.</param>
        /// <param name="Tolerance">A threashold for pixel variation.</param>
        /// <returns></returns>
        public static Pixel SearchPixelVerticalStride(Rectangle Screen, Point Start, uint StrideLength, Color Color, out bool IsFound, int Tolerance = 0)
        {
            for (int i = 0; i <= StrideLength; i++)
            {
                Pixel p = new Pixel
                {
                    Color = Color,

                    Point = new Point(Start.X, Start.Y + i)
                };

                if (p.IsPresent(Screen, Tolerance))
                {
                    IsFound = true;

                    return p;
                }
            }

            IsFound = false;

            return new Pixel();
        }

        /// <summary>
        /// looks through a single column of pixels for a specific pixel.
        /// </summary>
        /// <param name="Screen">The game screen location.</param>
        /// <param name="Start">The point at which we begin searching.</param>
        /// <param name="StrideLength">A possitive integer value the represents the distance of pixels to be searched through in the stride.</param>
        /// <param name="Color">The color we are searching for.</param>
        /// <param name="Tolerance">A threashold for pixel variation.</param>
        /// <returns></returns>
        public static Pixel SearchPixelHorizontalStride(Rectangle Screen, Point Start, uint StrideLength, Color Color, int Tolerance = 0)
        {
            for (int i = 0; i <= StrideLength; i++)
            {
                Pixel p = new Pixel
                {
                    Color = Color,

                    Point = new Point(Start.X + i, Start.Y)
                };

                if (p.IsPresent(Screen, Tolerance))
                {
                    return p;
                }
            }
            return new Pixel();
        }

        /// <summary>
        /// looks through a single row of pixels for a specific pixel.
        /// </summary>
        /// <param name="Screen">The game screen location.</param>
        /// <param name="Start">The point at which we begin searching.</param>
        /// <param name="StrideLength">A possitive integer value the represents the distance of pixels to be searched through in the stride.</param>
        /// <param name="Color">The color we are searching for.</param>
        /// <param name="IsFound">Returns true if the pixel is found in the stride.</param>
        /// <param name="Tolerance">A threashold for pixel variation.</param>
        /// <returns></returns>
        public static Pixel SearchPixelHorizontalStride(Rectangle Screen, Point Start, uint StrideLength, Color Color, out bool IsFound, int Tolerance = 0)
        {
            for (int i = 0; i <= StrideLength; i++)
            {
                Pixel p = new Pixel
                {
                    Color = Color,

                    Point = new Point(Start.X + i, Start.Y)
                };

                if (p.IsPresent(Screen, Tolerance))
                {
                    IsFound = true;

                    return p;
                }
            }

            IsFound = false;

            return new Pixel();
        }
    }
}
