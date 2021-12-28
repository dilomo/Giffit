//    < Gifit - a animated gif creation tool >
//    Copyright (C) 2021, Anton D. Kerezov, All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BumpKit
{
    /// <summary>
    /// BumpKit Encodes multiple images as an animated gif to a stream. <br />
    /// ALWAYS ALWAYS ALWAYS wire this up   in a using block <br />
    /// Disposing the encoder will complete the file. <br />
    /// Uses default .net GIF encoding and adds animation headers.
    /// https://github.com/DataDink/Bumpkit/tree/master/BumpKit/BumpKit
    /// https://psycodedeveloper.wordpress.com/2013/04/26/saving-an-animated-gif-image-with-c-in-windows-forms/
    /// </summary>
    public class GifEncoder : IDisposable
    {
        #region Header Constants
        private const string FileType = "GIF";
        private const string FileVersion = "89a";
        private const byte FileTrailer = 0x3b;

        private const int ApplicationExtensionBlockIdentifier = 0xff21;
        private const byte ApplicationBlockSize = 0x0b;
        private const string ApplicationIdentification = "NETSCAPE2.0";

        private const int GraphicControlExtensionBlockIdentifier = 0xf921;
        private const byte GraphicControlExtensionBlockSize = 0x04;

        private const long SourceGlobalColorInfoPosition = 10;
        private const long SourceGraphicControlExtensionPosition = 781;
        private const long SourceGraphicControlExtensionLength = 8;
        private const long SourceImageBlockPosition = 789;
        private const long SourceImageBlockHeaderLength = 11;
        private const long SourceColorBlockPosition = 13;
        private const long SourceColorBlockLength = 768;
        #endregion

        private bool _isFirstImage = true;
        private int? _width;
        private int? _height;
        private int? _repeatCount;
        private readonly Stream _stream;

        // Public Accessors
        public TimeSpan FrameDelay { get; set; }

        /// <summary>
        /// Encodes multiple images as an animated gif to a stream. <br />
        /// ALWAYS ALWAYS ALWAYS wire this in a using block <br />
        /// Disposing the encoder will complete the file. <br />
        /// Uses default .net GIF encoding and adds animation headers.
        /// </summary>
        /// <param name="stream">The stream that will be written to.</param>
        /// <param name="width">Sets the width for this gif or null to use the first frame's width.</param>
        /// <param name="height">Sets the height for this gif or null to use the first frame's height.</param>
        public GifEncoder(Stream stream, int? width = null, int? height = null, int? repeatCount = null)
        {
            _stream = stream;
            _width = width;
            _height = height;
            _repeatCount = repeatCount;
        }

        /// <summary>
        /// Adds a frame to this animation.
        /// </summary>
        /// <param name="img">The image to add</param>
        /// <param name="x">The positioning x offset this image should be displayed at.</param>
        /// <param name="y">The positioning y offset this image should be displayed at.</param>
        public void AddFrame(Image img, int x = 0, int y = 0, TimeSpan? frameDelay = null)
        {
            using (var gifStream = new MemoryStream())
            {
                img.Save(gifStream, ImageFormat.Gif);
                if (_isFirstImage) // Steal the global color table info
                {
                    InitHeader(gifStream, img.Width, img.Height);
                }
                WriteGraphicControlBlock(gifStream, frameDelay.GetValueOrDefault(FrameDelay));
                WriteImageBlock(gifStream, !_isFirstImage, x, y, img.Width, img.Height);
            }
            _isFirstImage = false;
        }

        private void InitHeader(Stream sourceGif, int w, int h)
        {
            // File Header
            WriteString(FileType);
            WriteString(FileVersion);
            WriteShort(_width.GetValueOrDefault(w)); // Initial Logical Width
            WriteShort(_height.GetValueOrDefault(h)); // Initial Logical Height
            sourceGif.Position = SourceGlobalColorInfoPosition;
            WriteByte(sourceGif.ReadByte()); // Global Color Table Info
            WriteByte(0); // Background Color Index
            WriteByte(0); // Pixel aspect ratio
            WriteColorTable(sourceGif);

            // App Extension Header
            WriteShort(ApplicationExtensionBlockIdentifier);
            WriteByte(ApplicationBlockSize);
            WriteString(ApplicationIdentification);
            WriteByte(3); // Application block length
            WriteByte(1);
            WriteShort(_repeatCount.GetValueOrDefault(0)); // Repeat count for images.
            WriteByte(0); // terminator
        }

        private void WriteColorTable(Stream sourceGif)
        {
            sourceGif.Position = SourceColorBlockPosition; // Locating the image color table
            var colorTable = new byte[SourceColorBlockLength];
            sourceGif.Read(colorTable, 0, colorTable.Length);
            _stream.Write(colorTable, 0, colorTable.Length);
        }

        private void WriteGraphicControlBlock(Stream sourceGif, TimeSpan frameDelay)
        {
            sourceGif.Position = SourceGraphicControlExtensionPosition; // Locating the source GCE
            var blockhead = new byte[SourceGraphicControlExtensionLength];
            sourceGif.Read(blockhead, 0, blockhead.Length); // Reading source GCE

            WriteShort(GraphicControlExtensionBlockIdentifier); // Identifier
            WriteByte(GraphicControlExtensionBlockSize); // Block Size
            WriteByte(blockhead[3] & 0xf7 | 0x08); // Setting disposal flag
            WriteShort(Convert.ToInt32(frameDelay.TotalMilliseconds / 10)); // Setting frame delay
            WriteByte(blockhead[6]); // Transparent color index
            WriteByte(0); // Terminator
        }

        private void WriteImageBlock(Stream sourceGif, bool includeColorTable, int x, int y, int h, int w)
        {
            sourceGif.Position = SourceImageBlockPosition; // Locating the image block
            var header = new byte[SourceImageBlockHeaderLength];
            sourceGif.Read(header, 0, header.Length);
            WriteByte(header[0]); // Separator
            WriteShort(x); // Position X
            WriteShort(y); // Position Y
            WriteShort(h); // Height
            WriteShort(w); // Width

            if (includeColorTable) // If first frame, use global color table - else use local
            {
                sourceGif.Position = SourceGlobalColorInfoPosition;
                WriteByte(sourceGif.ReadByte() & 0x3f | 0x80); // Enabling local color table
                WriteColorTable(sourceGif);
            }
            else
            {
                WriteByte(header[9] & 0x07 | 0x07); // Disabling local color table
            }

            WriteByte(header[10]); // LZW Min Code Size

            // Read/Write image data
            sourceGif.Position = SourceImageBlockPosition + SourceImageBlockHeaderLength;

            var dataLength = sourceGif.ReadByte();
            while (dataLength > 0)
            {
                var imgData = new byte[dataLength];
                sourceGif.Read(imgData, 0, dataLength);

                _stream.WriteByte(Convert.ToByte(dataLength));
                _stream.Write(imgData, 0, dataLength);
                dataLength = sourceGif.ReadByte();
            }

            _stream.WriteByte(0); // Terminator
        }

        private void WriteByte(int value)
        {
            _stream.WriteByte(Convert.ToByte(value));
        }

        private void WriteShort(int value)
        {
            _stream.WriteByte(Convert.ToByte(value & 0xff));
            _stream.WriteByte(Convert.ToByte((value >> 8) & 0xff));
        }

        private void WriteString(string value)
        {
            _stream.Write(value.ToArray().Select(c => (byte)c).ToArray(), 0, value.Length);
        }

        public void Dispose()
        {
            // Complete File
            WriteByte(FileTrailer);

            // Pushing data
            _stream.Flush();
        }
    }


    public static class ImageExtensions
    {
        /// <summary>
        /// Provides quick but unsafe access to this image's pixels. <br />
        /// ALWAYS ALWAYS ALWAYS wire this in a using block. <br />
        /// DO NOT EVER access the bitmap directly while in this unsafe context. <br />
        /// </summary>
        public static UnsafeBitmapContext CreateUnsafeContext(this Image image)
        {
            return new UnsafeBitmapContext(image);
        }

        /// <summary>
        /// Detects the widest and tallest pixels to determine the image padding based on the optional background color.
        /// </summary>
        /// <param name="image">The original image</param>
        /// <param name="backgroundColor">Specifies a background color for this image or transparent by default.</param>
        /// <returns>A rectangle containing the borders of this image's content.</returns>
        public static Rectangle DetectPadding(this Image image, Color backgroundColor = default(Color))
        {
            if (backgroundColor.IsEmpty)
                backgroundColor = Color.Transparent;

            var top = image.Height;
            var bottom = 0;
            var left = image.Width;
            var right = 0;
            var width = image.Width - 1;
            var height = image.Height - 1;
            using (var context = new UnsafeBitmapContext(image))
            {
                for (int y = 0; y < context.Height; y++)
                    for (int x = 0; x < context.Width; x++)
                    {
                        if (width - right <= x && left <= x && height - bottom <= y)
                            break;
                        if (x < left && !context.GetRawPixel(x, y).EqualsColor(backgroundColor))
                        {
                            if (y < top) { top = y; }
                            left = x;
                        }
                        if (x < width - right && !context.GetRawPixel(width - x, y).EqualsColor(backgroundColor))
                        {
                            if (y < top) { top = y; }
                            right = width - x;
                        }
                        if (y < height - bottom && !context.GetRawPixel(x, height - y).EqualsColor(backgroundColor))
                        {
                            bottom = height - y;
                        }
                    }
            }
            width = Math.Max(0, right - left);
            height = Math.Max(0, bottom - top);
            return new Rectangle(left, top, width, height);
        }

        /// <summary>
        /// Clones this image scaled to fit the specified size.
        /// </summary>
        /// <param name="image">The original image.</param>
        /// <param name="size">The size to scale the clone to.</param>
        /// <param name="mode">FitToContent will scale the content to be completely contained in the specified size. <br />
        /// Overflow will maximize the image to completely fill the specified size, clipping excess content.</param>
        /// <returns>A clone of this image scaled to the specified size.</returns>
        public static Image ScaleToFit(this Image image, Size size, ScalingMode mode = ScalingMode.FitContent)
        {
            return image.ScaleToFit(size, default(Color), true, mode);
        }

        /// <summary>
        /// Clones this image scaled to fit the specified size.
        /// </summary>
        /// <param name="image">The original image.</param>
        /// <param name="size">The size to scale the clone to.</param>
        /// <param name="backgroundColor">The color to fill unused image space with.</param>
        /// <param name="mode">FitToContent will scale the content to be completely contained in the specified size. <br />
        /// Overflow will maximize the image to completely fill the specified size, clipping excess content.</param>
        /// <returns>A clone of this image scaled to the specified size.</returns>
        public static Image ScaleToFit(this Image image, Size size, Color backgroundColor, ScalingMode mode = ScalingMode.FitContent)
        {
            return image.ScaleToFit(size, backgroundColor, true, mode);
        }

        /// <summary>
        /// Clones this image scaled to fit the specified size.
        /// </summary>
        /// <param name="image">The original image.</param>
        /// <param name="size">The size to scale the clone to.</param>
        /// <param name="dispose">If true, disposes this image upon cloning.</param>
        /// <param name="mode">FitToContent will scale the content to be completely contained in the specified size. <br />
        /// Overflow will maximize the image to completely fill the specified size, clipping excess content.</param>
        /// <returns>A clone of this image scaled to the specified size.</returns>
        public static Image ScaleToFit(this Image image, Size size, bool dispose, ScalingMode mode = ScalingMode.FitContent)
        {
            return image.ScaleToFit(size, default(Color), dispose, mode);
        }

        /// <summary>
        /// Clones this image scaled to fit the specified size.
        /// </summary>
        /// <param name="image">The original image.</param>
        /// <param name="size">The size to scale the clone to.</param>
        /// <param name="backgroundColor">The color to fill unused image space with.</param>
        /// <param name="dispose">If true, disposes this image upon cloning.</param>
        /// <param name="mode">FitToContent will scale the content to be completely contained in the specified size. <br />
        /// Overflow will maximize the image to completely fill the specified size, clipping excess content.</param>
        /// <returns>A clone of this image scaled to the specified size.</returns>
        public static Image ScaleToFit(this Image image, Size size, Color backgroundColor, bool dispose = true, ScalingMode mode = ScalingMode.FitContent)
        {
            var widthRatio = (double)size.Width / image.Width;
            var heightRatio = (double)size.Height / image.Height;
            var scaleRatio = mode == ScalingMode.Overflow
                                 ? Math.Max(widthRatio, heightRatio)
                                 : Math.Min(widthRatio, heightRatio);
            var width = image.Width * scaleRatio;
            var height = image.Height * scaleRatio;

            var newImage = new Bitmap(size.Width, size.Height);
            using (var gfx = Graphics.FromImage(newImage))
            {
                if (!backgroundColor.IsEmpty)
                    gfx.Clear(backgroundColor);
                gfx.DrawImage(image,
                    (float)((newImage.Width - width) / 2), (float)((newImage.Height - height) / 2),
                    (float)width, (float)height);
            }
            if (dispose) image.Dispose();
            return newImage;
        }

        /// <summary>
        /// Clones this image stretched to the specified size.
        /// </summary>
        /// <param name="image">The original image.</param>
        /// <param name="size">The size to stretch the clone to.</param>
        /// <param name="dispose">If true, disposes this image upon cloning.</param>
        /// <returns>A clone of this image streched to the specified size.</returns>
        public static Image Stretch(this Image image, Size size, bool dispose = true)
        {
            var newImage = new Bitmap(size.Width, size.Height);
            using (var gfx = Graphics.FromImage(newImage))
            {
                gfx.DrawImage(image, 0, 0, size.Width, size.Height);
            }
            if (dispose) image.Dispose();
            return newImage;
        }

        /// <summary>
        /// Clones this image with the specified rotation.
        /// </summary>
        /// <param name="image">The original image</param>
        /// <param name="angle">The angle to rotate the image</param>
        /// <param name="mode">FitToContent will resize the clone accordingly to fit the rotated content. <br />
        /// Overflow will maintain the original size and clip content.</param>
        /// <returns>A clone of this image with the specified rotation.</returns>
        public static Image Rotate(this Image image, double angle, ScalingMode mode = ScalingMode.Overflow)
        {
            return image.Rotate(angle, true, mode);
        }

        /// <summary>
        /// Clones this image with the specified rotation.
        /// </summary>
        /// <param name="image">The original image</param>
        /// <param name="angle">The angle to rotate the image</param>
        /// <param name="dispose">If true, disposes this image upon cloning.</param>
        /// <param name="mode">FitToContent will resize the clone accordingly to fit the rotated content. <br />
        /// Overflow will maintain the original size and clip content.</param>
        /// <returns>A clone of this image with the specified rotation.</returns>
        public static Image Rotate(this Image image, double angle, bool dispose, ScalingMode mode = ScalingMode.Overflow)
        {
            var width = image.Width;
            var height = image.Height;
            if (mode == ScalingMode.FitContent)
            {
                var o = angle % 180;
                var d = Math.Sqrt(Math.Pow(image.Width, 2) + Math.Pow(image.Height, 2));
                var a = (Math.Atan((double)image.Height / image.Width) * 180 / Math.PI) + (o > 90 ? 180 - o : o);
                height = (int)(Math.Sin(a * Math.PI / 180) * d);
                a = (Math.Atan((double)-image.Height / image.Width) * 180 / Math.PI) + (o > 90 ? 180 - o : o);
                width = (int)(Math.Cos(a * Math.PI / 180) * d);
            }
            var newImage = new Bitmap(width, height);
            using (var gfx = Graphics.FromImage(newImage))
            {
                gfx.TranslateTransform(-image.Width / (float)2, -image.Height / (float)2, MatrixOrder.Prepend);
                gfx.RotateTransform((float)angle, MatrixOrder.Append);
                gfx.TranslateTransform(newImage.Width / (float)2, newImage.Height / (float)2, MatrixOrder.Append);
                gfx.DrawImage(image, 0, 0);
            }
            if (dispose) image.Dispose();
            return newImage;
        }

        /// <summary>Creates a 24 bit-per-pixel copy of the source image.</summary>
        public static Image CopyImage(this Image image) => CopyImage(image, PixelFormat.Format24bppRgb);

        /// <summary>Creates a copy of the source image with the specified pixel format.</summary><remarks>
        /// This can also be achieved with the <see cref="Bitmap.Clone(int, int, PixelFormat)"/>
        /// overload, but I have had issues with that method.</remarks>
        public static Image CopyImage(this Image image, PixelFormat format)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            // Don't try to draw a new Bitmap with an indexed pixel format.
            if (format == PixelFormat.Format1bppIndexed || format == PixelFormat.Format4bppIndexed || format == PixelFormat.Format8bppIndexed || format == PixelFormat.Indexed)
                return (image as Bitmap).Clone(new Rectangle(0, 0, image.Width, image.Height), format);

            Image result = null;
            try
            {
                result = new Bitmap(image.Width, image.Height, format);

                using (Graphics graphics = Graphics.FromImage(result))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;

                    graphics.DrawImage(image, 0, 0, result.Width, result.Height);
                }
            }
            catch
            {
                if (result != null)
                    result.Dispose();

                throw;
            }
            return result;
        }
    }

    public enum ScalingMode
    {
        FitContent,
        Overflow
    }

    /// <summary>
    /// Provides quick but unsafe access to a bitmap's bits. <br />
    /// ALWAYS ALWAYS ALWAYS wire this in a using block. <br />
    /// DO NOT EVER access the bitmap directly while in this unsafe context. <br />
    /// </summary>
    public sealed unsafe class UnsafeBitmapContext : IDisposable
    {
        private Stream _originalStream;
        private Bitmap _bitmap;
        private BitmapData _lockData;
        private Byte* _ptrBase;
        private int _pixelWidth;

        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// Provides quick but unsafe access to a bitmap's bits. <br />
        /// ALWAYS ALWAYS ALWAYS wire this in a using block. <br />
        /// DO NOT EVER access the bitmap directly while in this unsafe context. <br />
        /// </summary>
        /// <param name="bitmap">The bitmap to access</param>
        public UnsafeBitmapContext(Bitmap bitmap)
        {
            _bitmap = bitmap;
            LockBits();
        }

        /// <summary>
        /// Provides quick but unsafe access to a bitmap's bits. <br />
        /// ALWAYS ALWAYS ALWAYS wire this in a using block. <br />
        /// DO NOT EVER access the bitmap directly while in this unsafe context. <br />
        /// </summary>
        /// <param name="image">The bitmap to access</param>
        public UnsafeBitmapContext(Image image)
        {
            if (!(image is Bitmap))
            {
                throw new ArgumentException("Image must be convertable to a bitmap.");
            }
            _bitmap = (Bitmap)image;
            LockBits();
        }

        /// <summary>
        /// Provides quick but unsafe access to a bitmap's bits. <br />
        /// The stream will be updated upon disposal. <br />
        /// ALWAYS ALWAYS ALWAYS wire this in a using block. <br />
        /// DO NOT EVER access the bitmap directly while in this unsafe context. <br />
        /// </summary>
        /// <param name="stream">The stream to read and write to.</param>
        public UnsafeBitmapContext(Stream stream)
        {
            try
            {
                _originalStream = stream;
                stream.Position = 0;
                _bitmap = (Bitmap)Image.FromStream(stream);
            }
            catch { throw new ArgumentException("Stream did not contain a valid image format."); }
            LockBits();
        }

        private void LockBits()
        {
            Width = _bitmap.Width;
            Height = _bitmap.Height;
            _pixelWidth = sizeof(Pixel);
            _lockData = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            _ptrBase = (Byte*)_lockData.Scan0.ToPointer();
        }

        public void Dispose()
        {
            _bitmap.UnlockBits(_lockData);
            _lockData = null;
            if (_originalStream != null)
            {
                _originalStream.SetLength(0);
                _originalStream.Position = 0;
                _bitmap.Save(_originalStream, _bitmap.RawFormat);
                _bitmap.Dispose();
                _originalStream.Position = 0;
            }
            _originalStream = null;
            _bitmap = null;
        }

        /// <summary>
        /// Access a pixel from the bitmap's memory (slower than GetRawPixel)
        /// </summary>
        public Color GetPixel(int x, int y)
        {
            var pixel = GetRawPixel(x, y);
            return Color.FromArgb(pixel.Alpha, pixel.Red, pixel.Green, pixel.Blue);
        }

        /// <summary>
        /// Access a pixel from the bitmap's memory (faster than GetPixel) <br />
        /// </summary>
        public Pixel GetRawPixel(int x, int y)
        {
            return *Pointer(x, y);
        }

        /// <summary>
        /// Replace a pixel in the bitmap's memory (slower than by bytes)
        /// </summary>
        public void SetPixel(int x, int y, Color color)
        {
            SetPixel(x, y, color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Replace a pixel in the bitmap's memory (faster than by Color)
        /// </summary>
        public void SetPixel(int x, int y, byte alpha, byte red, byte green, byte blue)
        {
            var pixel = Pointer(x, y);
            (*pixel).Alpha = alpha;
            (*pixel).Red = red;
            (*pixel).Green = green;
            (*pixel).Blue = blue;
        }

        private Pixel* Pointer(int x, int y)
        {
            if (x >= Width || x < 0 || y >= Height || y < 0)
            {
                Dispose();
                throw new ArgumentException("The X and Y parameters must be within the scope of the image pixel ranges.");
            }
            return (Pixel*)(_ptrBase + y * _lockData.Stride + x * _pixelWidth);
        }

        /// <summary>
        /// Represents raw pixel data.
        /// </summary>
        public struct Pixel
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }
    }

    public static class ColorExtensions
    {
        /// <summary>
        /// Compares a pixel and color and determines equality.
        /// </summary>
        public static bool EqualsColor(this UnsafeBitmapContext.Pixel pixel, Color color)
        {
            return (color.A == 0 && pixel.Alpha == 0)
                || (color.A == pixel.Alpha && color.R == pixel.Red && color.G == pixel.Green && color.B == pixel.Blue);
        }

        /// <summary>
        /// Compares a pixel and color and determines equality.
        /// </summary>
        public static bool EqualsPixel(this Color color, UnsafeBitmapContext.Pixel pixel)
        {
            return pixel.EqualsColor(color);
        }

        /// <summary>
        /// Creates a fade to a target color by percentage
        /// </summary>
        /// <param name="from">The source color</param>
        /// <param name="to">The target color</param>
        /// <param name="fade">The amount of the fade from 0 to 1</param>
        /// <returns></returns>
        public static Color FadeTo(this Color from, Color to, float fade)
        {
            return Color.FromArgb((int)Math.Min(255, Math.Max(0, from.A + (to.A - from.A) * fade)),
                                  (int)Math.Min(255, Math.Max(0, from.R + (to.R - from.R) * fade)),
                                  (int)Math.Min(255, Math.Max(0, from.G + (to.G - from.G) * fade)),
                                  (int)Math.Min(255, Math.Max(0, from.B + (to.B - from.B) * fade)));
        }
    }

    public static class FontEffects
    {
        /// <summary>
        /// Measures a string's pixel boundaries (without the extra padding MeasureString gives). <br />
        /// NOTE: This bench tests just under 50% slower than MeasureString.
        /// </summary>
        /// <param name="gfx">The source graphics object</param>
        /// <param name="text">The text to measure</param>
        /// <param name="font">The font to measure</param>
        /// <returns></returns>
        public static RectangleF MeasureStringBoundaries(this Graphics gfx, string text, Font font)
        {
            var rect = new RectangleF(0, 0, int.MaxValue, int.MaxValue);
            var format = new StringFormat();
            format.SetMeasurableCharacterRanges(new[] { new CharacterRange(0, text.Length) });
            var regions = gfx.MeasureCharacterRanges(text, font, rect, format);
            return regions[0].GetBounds(gfx);
        }

        /// <summary>
        /// Measures a string's pixel boundaries including border width (without the extra padding MeasureString gives). <br />
        /// NOTE: This bench tests just under 50% slower than MeasureString.
        /// </summary>
        /// <param name="gfx">The source graphics object</param>
        /// <param name="text">The text to measure</param>
        /// <param name="font">The font to measure</param>
        /// <param name="border">The intended border width</param>
        /// <returns></returns>
        public static RectangleF MeasureStringBoundaries(this Graphics gfx, string text, Font font, int border)
        {
            const float pathOffset = (float)0.97389271333939476;
            var measure = MeasureStringBoundaries(gfx, text, font);
            return new RectangleF(0, 0, (measure.Width / pathOffset + border * 2) / pathOffset, measure.Height + border * 2);
        }

        /// <summary>
        /// Draws text with a border.
        /// </summary>
        /// <param name="gfx">The source graphics object</param>
        /// <param name="text">The text to render</param>
        /// <param name="font">The font to render</param>
        /// <param name="brush">The brush to use for the rendered text</param>
        /// <param name="x">The x location to render the text at</param>
        /// <param name="y">The y location to render the text at</param>
        /// <param name="border">The width of the border to render in pixels</param>
        /// <param name="borderColors">A collection of colors to border should cycle through</param>
        /// <param name="colorOffsets">An index-matching collection of offsets to render the border colors at</param>
        public static void DrawString(this Graphics gfx, string text, Font font, Brush brush, int x, int y, int border, Color[] borderColors, float[] colorOffsets)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;
            if (gfx == null)
                throw new ArgumentNullException("gfx");
            if (font == null)
                throw new ArgumentNullException("font");
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (border <= 0)
                throw new ArgumentException("Border must be greater than 0", "border");
            if (borderColors.Length == 0)
                throw new ArgumentException("Border requires at least 1 color", "borderColors");
            if (borderColors.Length > 1 && borderColors.Length != colorOffsets.Length)
                throw new ArgumentException("A border with more than 1 color requires a matching number of offsets", "colorOffsets");
            if (colorOffsets == null || colorOffsets.Length == 0)
                colorOffsets = new[] { (float)0 };

            // Organize color fades from inner to outer
            var colors = borderColors
                .Select((c, i) => new KeyValuePair<float, Color>(colorOffsets[i], c))
                .OrderBy(c => c.Key)
                .ToArray();
            // Get bordered boundaries
            var offset = gfx.MeasureStringBoundaries(text, font).Location;
            var measure = gfx.MeasureStringBoundaries(text, font, border);

            using (var workImage = new Bitmap((int)measure.Width, (int)measure.Height))
            using (var gfxWork = Graphics.FromImage(workImage))
            {
                gfxWork.PageUnit = GraphicsUnit.Point;
                gfxWork.SmoothingMode = gfx.SmoothingMode;
                var path = new GraphicsPath();
                path.AddString(text, font.FontFamily, (int)font.Style, font.Size, new PointF((border - offset.X) * (float).75, (border - offset.Y) * (float).75), StringFormat.GenericDefault);

                // Fade the border from outer to inner.
                for (var b = border; b > 0; b--)
                {
                    var colorIndex = (float)1 / border * b;
                    var colorStart = colors.Length > 1 ? colors.Last(c => c.Key <= colorIndex) : colors.First();
                    var colorEnd = colors.Length > 1 ? colors.First(c => c.Key >= colorIndex) : colors.First();
                    var colorOffset = 1 / Math.Max((float).0000001, colorEnd.Key - colorStart.Key) * (colorIndex - colorStart.Key);
                    var color = colorStart.Value.FadeTo(colorEnd.Value, colorOffset);

                    const float lineWidthOffset = (float).65; // This is approximate
                    using (var pen = new Pen(color, b / lineWidthOffset) { LineJoin = LineJoin.Round })
                        gfxWork.DrawPath(pen, path);
                }

                // Draw the text
                gfxWork.FillPath(brush, path);
                var bounds = workImage.DetectPadding();
                var offsetX = ((measure.Width - bounds.Right) - bounds.X) / 2;

                // Apply the generated image
                gfx.DrawImage(workImage, x + offsetX, y);
            }
        }
    }
}