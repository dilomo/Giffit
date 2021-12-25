using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGySoft.Drawing;
using KGySoft.Drawing.Imaging;
using System.Drawing.Imaging;

namespace Giffit
{
    public class GiffitPreset
    {
        public decimal Scaling = 1.0m;
        public int _sindex = 0;
        public IQuantizer quantizer = null;
        public IDitherer ditherer = null;
        public PixelFormat pixFormat = PixelFormat.Format24bppRgb;

        public static List<string> StyleNames = new List<string>{
        "Graphix (1bpp)",
        "Lightness (1bpp)",
        "Polka Dot (1bpp)",
        "Medium Dot (1bpp)",
        "Small Dot (1bpp)",
        "Vintage (1bpp)",
        "Etchy (4bpp)",
        "Midnight Blues (4bpp)",
        "Mono Rough (4bpp)",
        "Grayscale (8bpp)",
        "Film Grain 200 (8bpp)",
        "Expired Film (8bpp)",
        "Full Index (8bpp)",
        "Colour #565 (16bpp)",
        "Transparante (16bppA)",
        "Colour #888 (24bpp)",
        "No Mercy (32bppA)",
        "Native (8bpp) (No preview) "
        };

        public int DefaultStyle { get => StyleNames.Count - 1; }
        public int StyleIndex
        {
            get { return _sindex; }
            set
            {
                _sindex = value;

                switch (value)
                {
                    case 0: // graphix
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite();
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise;
                        break;
                    case 1: // True mono
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite();
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = ErrorDiffusionDitherer.Atkinson;
                        break;
                    case 2: // Polka dot
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite();
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = OrderedDitherer.DottedHalftone;
                        break;
                    case 3: // medium dot
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite();
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = ErrorDiffusionDitherer.StevensonArce;
                        break;
                    case 4: // small dot
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite();
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = ErrorDiffusionDitherer.Burkes;
                        break;
                    case 5: // vintage
                        quantizer = OptimizedPaletteQuantizer.Octree(2);
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise.ConfigureStrength(.66f);
                        break;
                    case 6: // etchy
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite();
                        pixFormat = PixelFormat.Format4bppIndexed;
                        ditherer = ErrorDiffusionDitherer.StevensonArce;
                        break;
                    case 7: // blues
                        Palette blues = new Palette(new Color32[] {
                        new Color32(System.Drawing.Color.AliceBlue),
                        new Color32(System.Drawing.Color.SteelBlue),
                        new Color32(System.Drawing.Color.LightGoldenrodYellow),
                        new Color32(System.Drawing.Color.White),
                        new Color32(System.Drawing.Color.Black)});
                        quantizer = PredefinedColorsQuantizer.FromCustomPalette(blues);
                        pixFormat = PixelFormat.Format4bppIndexed;
                        ditherer = ErrorDiffusionDitherer.Burkes;
                        break;
                    case 8: // mono
                        quantizer = PredefinedColorsQuantizer.Grayscale16();
                        pixFormat = PixelFormat.Format4bppIndexed;
                        ditherer = ErrorDiffusionDitherer.FloydSteinberg;
                        break;
                    case 9: // Gray
                        quantizer = PredefinedColorsQuantizer.Grayscale();
                        pixFormat = PixelFormat.Format8bppIndexed;
                        break;
                    case 10: // film 200
                        quantizer = PredefinedColorsQuantizer.Grayscale();
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = new RandomNoiseDitherer(0.25f);
                        break;
                    case 11: // expired
                        quantizer = OptimizedPaletteQuantizer.Octree(36);
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise;
                        break;
                    case 12: // index
                        quantizer = OptimizedPaletteQuantizer.Octree();
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise;
                        break;
                    case 13: // 565 
                        quantizer = PredefinedColorsQuantizer.Rgb565();
                        pixFormat = PixelFormat.Format16bppRgb565;
                        ditherer = ErrorDiffusionDitherer.FloydSteinberg;
                        break;
                    case 14: // transparante
                        quantizer = PredefinedColorsQuantizer.Argb1555();
                        pixFormat = PixelFormat.Format16bppArgb1555;
                        ditherer = ErrorDiffusionDitherer.FloydSteinberg;
                        break;
                    case 15: // 888
                        quantizer = PredefinedColorsQuantizer.Rgb888();
                        pixFormat = PixelFormat.Format24bppRgb;
                        break;
                    case 16: // ultra
                        quantizer = PredefinedColorsQuantizer.Argb8888();
                        pixFormat = PixelFormat.Format32bppPArgb;
                        break;
                    case 17: // system
                        break;
                    default:
                        quantizer = PredefinedColorsQuantizer.Rgb565();
                        pixFormat = PixelFormat.Format16bppRgb565;
                        ditherer = ErrorDiffusionDitherer.FloydSteinberg;
                        break;
                }
            }

        }
    }
}
