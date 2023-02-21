using System.Diagnostics;
using System.Runtime.InteropServices;
using L4D2AntiCheat.Infrastructure.Extensions;

namespace L4D2AntiCheat.Infrastructure.Helpers;

public static class ScreenshotHelper
{
    public static Bitmap TakeScreenshot(Process process)
    {
        var rectangle = new Rectangle();

        GetWindowRect(process.MainWindowHandle, ref rectangle);

        var left = rectangle.Left;
        var top = rectangle.Top;
        var width = rectangle.Right - left + 1;
        var height = rectangle.Bottom - top + 1;

        var desktopWindow = GetDesktopWindow();
        var windowDc = GetWindowDC(desktopWindow);
        var compatibleDc = CreateCompatibleDC(windowDc);
        var compatibleBitmap = CreateCompatibleBitmap(windowDc, width, height);

        _ = SelectObject(compatibleDc, compatibleBitmap);
        BitBlt(compatibleDc, 0, 0, width, height, windowDc, left, top, 0x00CC0020);

        var intPtr = new IntPtr(compatibleBitmap);
        var hbitmap = Image.FromHbitmap(intPtr);
        using var bitmap = new Bitmap(hbitmap, hbitmap.Width, hbitmap.Height);

        _ = ReleaseDC(desktopWindow, windowDc);
        DeleteDC(compatibleDc);
        DeleteObject(compatibleBitmap);

        return bitmap.Resize();
    }

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);

    [DllImport("User32.dll")]
    private static extern int GetDesktopWindow();

    [DllImport("User32.dll")]
    private static extern int GetWindowDC(int hWnd);

    [DllImport("GDI32.dll")]
    private static extern int SelectObject(int hdc, int hgdiobj);

    [DllImport("GDI32.dll")]
    private static extern bool BitBlt(int hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, int hdcSrc, int nXSrc, int nYSrc, int dwRop);

    [DllImport("GDI32.dll")]
    private static extern int CreateCompatibleBitmap(int hdc, int nWidth, int nHeight);

    [DllImport("GDI32.dll")]
    private static extern int CreateCompatibleDC(int hdc);

    [DllImport("User32.dll")]
    private static extern int ReleaseDC(int hWnd, int hdc);

    [DllImport("GDI32.dll")]
    private static extern bool DeleteDC(int hdc);

    [DllImport("GDI32.dll")]
    private static extern bool DeleteObject(int hObject);

    private struct Rectangle
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public int Left { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public int Top { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public int Right { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public int Bottom { get; set; }
    }
}