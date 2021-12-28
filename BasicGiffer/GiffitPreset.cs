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
        private int _sindex = 0;
        public IQuantizer quantizer = null;
        public IDitherer ditherer = null;
        public PixelFormat pixFormat = PixelFormat.Format8bppIndexed;
        public bool OptimisedQuantizer = false;
        public bool HighQuality = false;

        public static List<string> StyleNames = new List<string>{
        "Graphix (1bpp)",
        "Lightness (1bpp)",
        "Polka Dot (1bpp)",
        "Medium Dot (1bpp)",
        "Small Dot (1bpp)",
        "Vintage (1bpp)",
        "Mono Pop (4bpp)",
        "Midnight Blues (4bpp)",
        "Half Grey (4bpp)",
        "Grayscale (8bpp)",
        "B&W Film (8bpp)",
        "Filmic (8bpp)",
        "Octree #256 (8bpp)",
        "Colour #332 (8bpp)",
        "High colour (additive 8bpp)",
        "System (no preview)(8bpp)"
        };


        public int DefaultStyle { get => StyleNames.Count - 1; }
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
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite();
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer =  OrderedDitherer.BlueNoise.ConfigureStrength(0.9f);
                        break;
                    case 1: // lightness
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite(System.Drawing.Color.Black, 92);
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = ErrorDiffusionDitherer.Atkinson.ConfigureErrorDiffusionMode(true);
                        break;
                    case 2: // Polka dot
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite(System.Drawing.Color.Black, 112);
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = OrderedDitherer.DottedHalftone.ConfigureStrength(.9f);
                        break;
                    case 3: // medium dot
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite();
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = ErrorDiffusionDitherer.StevensonArce;
                        break;
                    case 4: // small dot
                        quantizer = PredefinedColorsQuantizer.BlackAndWhite(System.Drawing.Color.Black, 24);
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = ErrorDiffusionDitherer.Burkes;
                        break;
                    case 5: // vintage
                        quantizer = OptimizedPaletteQuantizer.Octree(2);
                        pixFormat = PixelFormat.Format1bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise.ConfigureStrength(.66f);
                        OptimisedQuantizer = true;
                        break;
                    case 6: // mono pop
                        quantizer = PredefinedColorsQuantizer.Grayscale4(System.Drawing.Color.Black, true);
                        pixFormat = PixelFormat.Format4bppIndexed;
                        ditherer = OrderedDitherer.Bayer8x8.ConfigureStrength(.7f);
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
                        quantizer = PredefinedColorsQuantizer.Grayscale16(System.Drawing.Color.Black, true);
                        pixFormat = PixelFormat.Format4bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise;
                        break;
                    case 9: // Gray
                        quantizer = PredefinedColorsQuantizer.Grayscale();
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = ErrorDiffusionDitherer.FloydSteinberg;
                        break;
                    case 10: // film 200
                        quantizer = PredefinedColorsQuantizer.Grayscale();
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = new RandomNoiseDitherer(0.25f);
                        break;
                    case 11: // filmic
                        quantizer = OptimizedPaletteQuantizer.Octree(32);
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise.ConfigureStrength(.88f);
                        OptimisedQuantizer = true;
                        break;
                    case 12: // index
                        quantizer = OptimizedPaletteQuantizer.Octree();
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = OrderedDitherer.BlueNoise.ConfigureStrength(.9f);
                        OptimisedQuantizer = true;
                        break;
                    case 13: // 332
                        quantizer = PredefinedColorsQuantizer.Rgb332(System.Drawing.Color.Black, true);
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = ErrorDiffusionDitherer.FloydSteinberg;
                        break;
                    case 14: // High fidelity
                        HighQuality = true;
                        quantizer = OptimizedPaletteQuantizer.Wu();
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = ErrorDiffusionDitherer.FloydSteinberg;
                        break;
                    case 15: // system default
                        break;
                    default: 
                        quantizer = PredefinedColorsQuantizer.Rgb332();
                        pixFormat = PixelFormat.Format8bppIndexed;
                        ditherer = ErrorDiffusionDitherer.FloydSteinberg;
                        break;
                }
            }

        }
    }
}
