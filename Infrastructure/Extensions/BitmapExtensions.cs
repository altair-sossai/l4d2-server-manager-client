using System.Drawing.Imaging;

namespace L4D2AntiCheat.Infrastructure.Extensions;

public static class BitmapExtensions
{
    private static readonly ImageCodecInfo ImageCodecInfo = ImageCodecInfo.GetImageEncoders().First(o => o.FormatID == ImageFormat.Jpeg.Guid);

    public static MemoryStream Compress(this Bitmap bitmap)
    {
        var memoryStream = new MemoryStream();
        using var encoderParameter = new EncoderParameter(Encoder.Quality, 60L);
        using var encoderParameters = new EncoderParameters(1);
        encoderParameters.Param[0] = encoderParameter;

        bitmap.Save(memoryStream, ImageCodecInfo, encoderParameters);

        return memoryStream;
    }

    public static Bitmap Resize(this Bitmap bitmap)
    {
        return new Bitmap(bitmap, Math.Min(bitmap.Width, 1280), Math.Min(bitmap.Height, 720));
    }
}