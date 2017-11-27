using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;




namespace L2RBot
{
    class Screen
    {
        //Adjust for different android emulators//
        public static int topBoarder = 30;      //
        public static int bottomBorder = 2;     //
        public static int leftBorder = 2;       //
        public static int rightBorder = 2;      //
        //////////////////////////////////////////

        public static int borderWidth = leftBorder + rightBorder;
        public static int borderHeight = topBoarder + bottomBorder;
        /// <summary>
        /// takes a process object and returns its the game screens x, y, width and height
        /// </summary>
        /// <param name="proc"></param>
        /// <returns></returns>
        public static Rectangle GetRect(Process proc)
        {
            var rect = new User32.Rect();//Rekt type object for parameter of WINAPI
            User32.GetWindowRect(proc.MainWindowHandle, ref rect);

            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            Rectangle rectangle = new Rectangle(rect.Left, rect.Top, width, height);
            if (rectangle.Width != (1280 + rightBorder + leftBorder) | rectangle.Height != (720 + topBoarder + bottomBorder))
            {
                MainWindow.main.UpdateLog = proc.MainWindowTitle + " needs reset to 1280x720 or Nox player left and top borders have changed. " +
                    "Window should be detected as " + (1280 + borderWidth) + "x" + (720 + borderHeight) +
                    "your screen is " + rectangle.Width + "x" + rectangle.Height;
            }
            return rectangle;
        }

        /// <summary>
        /// converts game x, y points to windows x, y points for clicking purposes.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static Point PointToScreenPoint(Rectangle rect, int X, int Y)
        {
            int x;
            int y;
            x = rect.X + X + leftBorder;
            y = rect.Y + Y + topBoarder;
            //Debug.WriteLine(X + "," + Y + "|"+ x + "," + y);
            return new Point(x, y);

        }

        /// <summary>
        /// returns a color at a games screen x and y location
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static Color GetColor(Rectangle rect, int X, int Y)
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
        /// This method takes the percentage x and y position values that are created from a Nox Player screenshot
        /// It receives the windows position and size data in the form of a rectangle
        /// Tt will subtract the pixel boarders from the x and y values before calculating pixel location(since they are being determined with a screenshot that has no boarders).
        /// It converts the Perctage values to represent the actual screen locations of the pixels to account for Nox Player screen resizing or movement of the Nox Player Window
        /// </summary>
        /// <param name = "rect" > Windows postistion and dementions represented in a Rectangle</param>
        /// <param name = "X" > Percentage of the screen's x value devised from a nox screenshot(takes into account the 2 boarders and maths them away)</param>
        /// <param name = "Y" > Percentage of the screen's y value devised from a nox screenshot(takes into account the 2 boarders and maths them away)</param>
        /// <returns></returns>
        public static Color GetColor(Rectangle rect, double X, double Y)
        {
            Point point = PercentToPoint(rect, X, Y);
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
                    //MainWindow.main.UpdateLog = p.ToString();
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
                    //MainWindow.main.UpdateLog = p.ToString();
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
                    //MainWindow.main.UpdateLog = p.ToString();
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
                    //MainWindow.main.UpdateLog = p.ToString();
                    IsFound = true;
                    return p;
                }
            }
            IsFound = false;
            return new Pixel();
        }

        //Legacy

        /// <summary>
        /// Takes a games % x and y values and returns the windows screen x and y values.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static Point PercentToPoint(Rectangle rect, double X, double Y)
            {
                int mWidth = rect.Width - borderWidth;
                double mWidthD = (double) mWidth / 100;
                double dWidth = (double) mWidthD * X;

                int mHeight = rect.Height - borderHeight;
                double mHeightD = (double) mHeight / 100;
                double dHeight = (double) mHeightD * Y;

                int x = (int) Math.Round(dWidth);
                int y = (int) Math.Round(dHeight);

                x += rect.X + leftBorder;
                y += rect.Y + topBoarder;

                return new Point(x, y);

            }
            //public static Rectangle PercentToRect(Rectangle rect, double X, double Y, double Width, double Height)
            //{

            //    Point point = PercentToPoint(rect, X, Y);
            //    Point dem = PercentToPoint(rect, Width, Height);
            //    Rectangle inner = new Rectangle(point.X, point.Y, dem.X, dem.Y);

            //    return inner;
            //}

            //public Bitmap CaptureApplication(Process proc)
            //{

            //    var rect = new User32.Rect();
            //    User32.GetWindowRect(proc.MainWindowHandle, ref rect);

            //    int width = rect.right - rect.left;
            //    int height = rect.bottom - rect.top;
            //    var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            //    Graphics graphics = Graphics.FromImage(bmp);
            //    graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
            //    bmp.Save(@"test.PNG", ImageFormat.Png);
            //    return bmp;
            //}

            //public Bitmap CaptureApplicationScreen(Process proc, out Rectangle rectangle)
            //{

            //    var rect = new User32.Rect();
            //    //Console.WriteLine(proc.MainWindowTitle + proc.MainWindowHandle.ToString());
            //    User32.GetWindowRect(proc.MainWindowHandle, ref rect);

            //    int width = rect.right - rect.left;
            //    int height = rect.bottom - rect.top;
            //    //Console.WriteLine(rect.right.ToString() + rect.left.ToString() + rect.bottom.ToString() + rect.top.ToString());
            //    var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            //    Graphics graphics = Graphics.FromImage(bmp);
            //    graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

            //    //point = new Point(rect.left, rect.top);
            //    rectangle = new Rectangle(rect.left, rect.top, width, height);

            //    return bmp;
            //}

            //public static Color GetColor(Rectangle rect, double X, double Y, out Point click_location)
            //{
            //    Point point = PercentToPoint(rect, X, Y);
            //    int x = point.X;
            //    int y = point.Y;

            //    click_location = new Point(x, y);

            //    IntPtr desk = User32.GetDesktopWindow();
            //    IntPtr dc = User32.GetWindowDC(desk);
            //    int a = (int) User32.GetPixel(dc, x, y);
            //    User32.ReleaseDC(desk, dc);
            //    return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
            //}

        }
    }
