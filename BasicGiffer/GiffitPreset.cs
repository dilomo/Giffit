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
        "Film Grain 800 (4bpp)",
        "Film Grain 200 (8bpp)",
        "Expired Film (8bpp)",
        "Full Index (8bpp)",
        "Colour #565 (16bpp)",
        "Transparante (16bppA)",
        "Colour #888 (24bpp)",
        "No Mercy (32bppA)",
        "Native (8bpp) (No preview) "
        };

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
                    case 6: // film
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite();
                        pixFormat = PixelFormat.Format4bppIndexed;
                        ditherer = ErrorDiffusionDitherer.StevensonArce;
                        break;
                    case 7: // film 200
                        quantizer = PredefinedColorsQuantizer.Grayscale();
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = new RandomNoiseDitherer(0.25f);
                        break;
                    case 8: // expired
                        quantizer = OptimizedPaletteQuantizer.Octree(36);
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise;
                        break;
                    case 9: // index
                        quantizer = OptimizedPaletteQuantizer.Octree();
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise;
                        break;
                    case 10: // 565 
                        quantizer = PredefinedColorsQuantizer.Rgb565();
                        pixFormat = PixelFormat.Format16bppRgb565;
                        ditherer = ErrorDiffusionDitherer.FloydSteinberg;
                        break;
                    case 11: // transparante
                        quantizer = PredefinedColorsQuantizer.Argb1555();
                        pixFormat = PixelFormat.Format16bppArgb1555;
                        ditherer = ErrorDiffusionDitherer.FloydSteinberg;
                        break;
                    case 12: // 888
                        quantizer = PredefinedColorsQuantizer.Rgb888();
                        pixFormat = PixelFormat.Format24bppRgb;
                        break;
                    case 13: // ultra
                        quantizer = PredefinedColorsQuantizer.Argb8888();
                        pixFormat = PixelFormat.Format32bppPArgb;
                        break;
                    case 14: // system
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
