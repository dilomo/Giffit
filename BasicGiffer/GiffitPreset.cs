using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGySoft.Drawing;
using KGySoft.Drawing.Imaging;
using System.Drawing.Imaging;
using System.Drawing;

namespace Giffit
{
    public class GiffitPreset
    {
        public decimal Scaling = 1.0m;
        public decimal Brightness = 1.0m;
        private int _sindex = 0;
        public IQuantizer quantizer = null;
        public IDitherer ditherer = null;
        public PixelFormat pixFormat = PixelFormat.Format8bppIndexed;
        public bool OptimisedQuantizer = false;
        public bool HighQuality = false;
        public System.Drawing.Color Background = System.Drawing.Color.White;
        public byte AlphaThold = 128;

        public static List<string> StyleNames = new List<string>{
        "Graphix (1bpp)",
        "Lightness (1bpp)",
        "Polka Dot (1bpp)",
        "Medium Dot (1bpp)",
        "Small Dot (1bpp)",
        "Vintage (4bpp)",
        "Mono Pop (4bpp)",
        "Midnight Blues (4bpp)",
        "Half Grey (4bpp)",
        "Grayscale (8bpp)",
        "B&W Film (8bpp)",
        "Rough Colour (4bpp)",
        "Filmic (8bpp)",
        "Octree #256 (8bpp)",
        "Colour #332 (opaque 8bpp)",
        "Windows '98 (8bpp)",
        "High Fidelity (additive 8bpp)"
        };


        public int DefaultStyle { get => 14; }
        public int StyleIndex
        {
            get { return _sindex; }
            set
            {
                _sindex = value;
                HighQuality = false;

                switch (value)
                {
                    case 0: // graphix
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite(Background, AlphaThold);
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer =  OrderedDitherer.BlueNoise.ConfigureStrength(0.9f);
                        break;
                    case 1: // lightness
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite(Background, 92);
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = ErrorDiffusionDitherer.Atkinson.ConfigureErrorDiffusionMode(true);
                        break;
                    case 2: // Polka dot
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite(Background, 112);
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = OrderedDitherer.DottedHalftone.ConfigureStrength(.9f);
                        break;
                    case 3: // medium dot
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite(Background, AlphaThold);
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = ErrorDiffusionDitherer.StevensonArce;
                        break;
                    case 4: // small dot
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite(Background, 24);
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = ErrorDiffusionDitherer.Burkes;
                        break;
                    case 5: // vintage
                        quantizer = OptimizedPaletteQuantizer.Octree(4, Background, AlphaThold);
                        pixFormat = PixelFormat.Format4bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise.ConfigureStrength(.66f);
                        OptimisedQuantizer = true;
                        break;
                    case 6: // mono pop
                        quantizer = PredefinedColorsQuantizer.Grayscale4(Background, true);
                        pixFormat = PixelFormat.Format4bppIndexed;
                        ditherer = OrderedDitherer.Bayer8x8.ConfigureStrength(.7f);
                        break;
                    case 7: // blues
                        Color[] colour5 = new Color[] {
                            System.Drawing.Color.Black,
                            System.Drawing.Color.White,
                            System.Drawing.Color.AliceBlue,
                            System.Drawing.Color.SteelBlue,
                            System.Drawing.Color.LightGoldenrodYellow           
                        };
                        quantizer = PredefinedColorsQuantizer.FromCustomPalette(colour5, Background, AlphaThold);
                        pixFormat = PixelFormat.Format4bppIndexed;
                        ditherer = ErrorDiffusionDitherer.Burkes;
                        break;
                    case 8: // mono
                        quantizer = PredefinedColorsQuantizer.Grayscale16(Background, true);
                        pixFormat = PixelFormat.Format4bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise;
                        break;
                    case 9: // Gray
                        quantizer = PredefinedColorsQuantizer.Grayscale(Background);
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = OrderedDitherer.Bayer8x8; // because preview does not work otherwise and is using the previous detherer
                        break;
                    case 10: // film 200
                        quantizer = PredefinedColorsQuantizer.Grayscale(Background);
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = new RandomNoiseDitherer(0.25f);
                        break;
                    case 11: // rought c
                        Color[] colour9 = new Color[] {
                            System.Drawing.Color.Black,
                            System.Drawing.Color.White,
                            System.Drawing.Color.Transparent,
                            System.Drawing.Color.Red,
                            System.Drawing.Color.Lime,
                            System.Drawing.Color.Blue,
                            System.Drawing.Color.Cyan,
                            System.Drawing.Color.Yellow,
                            System.Drawing.Color.Magenta
                        };
                        quantizer = PredefinedColorsQuantizer.FromCustomPalette(colour9, Background, AlphaThold);
                        pixFormat = PixelFormat.Format4bppIndexed;
                        ditherer = new InterleavedGradientNoiseDitherer();
                        break;
                    case 12: // filmic
                        quantizer = OptimizedPaletteQuantizer.Octree(32, Background, AlphaThold);
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise.ConfigureStrength(.88f);
                        OptimisedQuantizer = true;
                        break;
                    case 13: // index
                        quantizer = OptimizedPaletteQuantizer.Octree(256, Background, AlphaThold);
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise;
                        OptimisedQuantizer = true;
                        break;
                    case 14: // 332
                        quantizer = PredefinedColorsQuantizer.Rgb332(Background, true);
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = ErrorDiffusionDitherer.FloydSteinberg;
                        break;
                     case 15: // system default
                        quantizer = PredefinedColorsQuantizer.SystemDefault8BppPalette(Background, AlphaThold);
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise;
                        break;
                    case 16: // High fidelity
                        HighQuality = true;
                        quantizer = OptimizedPaletteQuantizer.Wu(256, Background, AlphaThold);
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = ErrorDiffusionDitherer.FloydSteinberg;
                        break;
                    default: 
                        quantizer = PredefinedColorsQuantizer.Rgb332(Background, false);
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise;
                        break;
                }
            }

        }
    }
}
