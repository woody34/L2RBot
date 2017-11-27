using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2RBot.Common.Enum
{
    class Input
    {
        public enum MouseEventFlags : uint
        {
            LEFTDOWN = 0x00000002,

            LEFTUP = 0x00000004,

            MIDDLEDOWN = 0x00000020,

            MIDDLEUP = 0x00000040,

            MOVE = 0x00000001,

            ABSOLUTE = 0x00008000,

            RIGHTDOWN = 0x00000008,

            RIGHTUP = 0x00000010,

            WHEEL = 0x00000800,

            XDOWN = 0x00000080,

            XUP = 0x00000100

        }

        public enum WindowMessages : uint
        {

            WM_MOUSEACTIVATE = 0x0021,

            WM_SETCURSOR = 0x0020,

            WM_KEYDOWN = 0x0100,

            WM_KEYUP = 0x0101,

            WM_MOUSEMOVE = 0x0200,

            WM_LBUTTONDOWN = 0x0201,

            WM_LBUTTONUP = 0x0202,

            WM_RBUTTONDOWN = 0x0204,

            WM_RBUTTONUP = 0x0205

        }

        public enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        public enum MouseKeyboard : uint
        {
            MK_LBUTTON = 0x0001,
            MK_RBUTTON = 0x0002,
            MK_CONTROL = 0x0008
        }

        public enum HitTestCodes : uint
        {
            HTCLIENT = 1,
        }

        //public enum MouseFlags : uint
        //{
        //    LeftDown = 0x00000002,
        //    LeftUp = 0x00000004,
        //    MiddleDown = 0x00000020,
        //    MiddleUp = 0x00000040,
        //    Move = 0x00000001,
        //    Absolute = 0x00008000,
        //    RightDown = 0x00000008,
        //    RightUp = 0x00000010,
        //    Wheel = 0x00000800,
        //    XDown = 0x00000080,
        //    XUp = 0x00000100
        //}

        public enum ShowWindowEnum
        {
            Hide = 0,
            ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
            Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
            Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
            Restore = 9, ShowDefault = 10, ForceMinimized = 11
        };
    }
}
