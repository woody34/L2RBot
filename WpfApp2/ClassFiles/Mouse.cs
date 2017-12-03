using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using L2RBot.Common.Enum;
using static L2RBot.User32;
using static L2RBot.Common.Enum.Input;



namespace L2RBot
{
    class Mouse
    {
        //[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        //public static extern IntPtr SetFocus(IntPtr hWnd);

        /// <summary>
        /// Simulates left mouse click
        /// </summary>
        /// <param name="xpos"></param>
        /// <param name="ypos"></param>
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event((int) MouseEventFlags.LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event((int) MouseEventFlags.LEFTUP, xpos, ypos, 0, 0);

        }
        /// <summary>
        /// Clicks at point Down and drags to Point Up
        /// </summary>
        /// <param name="Down">x,y locations of left mouse press</param>
        /// <param name="Up">x,y locations of left mouse release</param>
        public static void LeftMouseClickDrag(System.Drawing.Point Down, System.Drawing.Point Up)
        {
            SetCursorPos(Down.X, Down.Y);
            mouse_event((int) MouseEventFlags.LEFTDOWN, Down.X, Down.X, 0, 0);
            SetCursorPos(Up.X, Up.Y);
            mouse_event((int) MouseEventFlags.LEFTDOWN, Up.X, Up.Y, 0, 0);

        }
    }
}
