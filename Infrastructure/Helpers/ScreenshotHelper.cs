using System.Diagnostics;
using System.Runtime.InteropServices;

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
        var bitmap = new Bitmap(width, height);

        using var graphics = Graphics.FromImage(bitmap);

        graphics.CopyFromScreen(left, top, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);

        return bitmap;
    }

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);

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