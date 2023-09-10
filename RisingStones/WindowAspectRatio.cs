using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;

namespace RisingStones
{
    internal class WindowAspectRatio
    {
        public double Ratio { get; set; }

        public WindowAspectRatio(Window window)
        {
            Ratio = window.Width / window.Height;
            ((HwndSource)PresentationSource.FromVisual(window)).AddHook(DragHook);
        }

        internal enum WM
        {
            WINDOWPOSCHANGING = 0x0046,
        }

        [Flags()]
        public enum SWP
        {
            NoMove = 0x2,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;
        }

        private IntPtr DragHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handeled)
        {
            if ((WM)msg == WM.WINDOWPOSCHANGING)
            {
                WINDOWPOS? _position = (WINDOWPOS?)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));

                if (!_position.HasValue)
                {
                    return IntPtr.Zero;
                }

                WINDOWPOS position = _position.Value;

                if ((position.flags & (int)SWP.NoMove) != 0 ||
                    HwndSource.FromHwnd(hwnd).RootVisual == null) return IntPtr.Zero;

                position.cy = (int)(position.cx / Ratio);

                Marshal.StructureToPtr(position, lParam, true);
                handeled = true;
            }

            return IntPtr.Zero;
        }
    }
}
