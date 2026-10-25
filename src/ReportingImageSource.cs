using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace MATTAR.Reporting;

// PdfSharpCore's default adapter uses ImageSharp loading and encoder APIs removed after version 1.
internal sealed class ReportingImageSource(string path) : ImageSource.IImageSource, IDisposable
{
    private readonly Image<Rgba32> _image = Image.Load<Rgba32>(path);

    public int Width => _image.Width;
    public int Height => _image.Height;
    public string Name { get; } = path;
    public bool Transparent => _image.Metadata.DecodedImageFormat is PngFormat;

    public void SaveAsJpeg(MemoryStream stream)
        => _image.SaveAsJpeg(stream, new JpegEncoder { Quality = 75 });

    public void SaveAsPdfBitmap(MemoryStream stream)
        => _image.Save(stream, new BmpEncoder { BitsPerPixel = BmpBitsPerPixel.Bit32, SkipMetadata = true });

    public void Dispose() => _image.Dispose();
}
