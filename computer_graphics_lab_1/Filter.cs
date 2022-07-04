using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter
{
    public class Filter
    {
        private static Bitmap image;
        private static float[,] kernel = null;
        private static int k;
        private static int Clamp(int val, int min, int max)
        {
            if (val > max)
            {
                return max;
            } 
            else if (val < min)
            {
                return min;
            }
            return val;
        }

        public static Bitmap InvertExecute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color sourceColor, resultColor;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    sourceColor = sourceImage.GetPixel(i, j);
                    resultColor = Color.FromArgb(255 - sourceColor.R,
                                                 255 - sourceColor.G,
                                                 255 - sourceColor.B);
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            return resultImage;
        }

        public static Bitmap GrayScaleExecute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color sourceColor, resultColor;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    sourceColor = sourceImage.GetPixel(i, j);
                    resultColor = Color.FromArgb((int)(0.299 * sourceColor.R + 0.587 * sourceColor.G + 0.114 * sourceColor.B),
                                                 (int)(0.299 * sourceColor.R + 0.587 * sourceColor.G + 0.114 * sourceColor.B),
                                                 (int)(0.299 * sourceColor.R + 0.587 * sourceColor.G + 0.114 * sourceColor.B));
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            return resultImage;
        }

        public static Bitmap SepiaExecute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color sourceColor, resultColor;
            int intensity;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    sourceColor = sourceImage.GetPixel(i, j);
                    intensity = (int)(0.299 * sourceColor.R + 0.587 * sourceColor.G + 0.114 * sourceColor.B);
                    resultColor = Color.FromArgb(Clamp(intensity + 2 * 20, 0, 255),
                                                 Clamp(intensity + 10, 0, 255),
                                                 Clamp(intensity - 20, 0, 255));
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            return resultImage;
        }

        public static Bitmap BrightnessExecute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color sourceColor, resultColor;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    sourceColor = sourceImage.GetPixel(i, j);
                    resultColor = Color.FromArgb(Clamp(sourceColor.R + k, 0, 255),
                                                 Clamp(sourceColor.G + k, 0, 255),
                                                 Clamp(sourceColor.B + k, 0, 255));
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            return resultImage;
        }

        public static Bitmap ShiftExecute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color sourceColor, resultColor;
            int k = 50;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    if (i < k)
                    {
                        resultColor = Color.FromArgb(0, 0, 0);
                        resultImage.SetPixel(i, j, resultColor);
                    } 
                    else
                    {
                        sourceColor = sourceImage.GetPixel(i - 50, j);
                        resultColor = Color.FromArgb(sourceColor.R,
                                                     sourceColor.G,
                                                     sourceColor.B);
                        resultImage.SetPixel(i, j, resultColor);
                    }
                }
            }
            return resultImage;
        }

        private static Color calculateNewPixelColor(int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            float resultR = 0;
            float resultG = 0;
            float resultB = 0;
            int idX, idY;
            Color sourceColor;

            for (int i = -radiusX; i <= radiusX; i++)
            {
                for (int j = -radiusY; j <= radiusY; j++)
                {
                    idX = Clamp(x + i, 0, image.Width - 1);
                    idY = Clamp(y + j, 0, image.Height - 1);
                    sourceColor = image.GetPixel(idX, idY);
                    resultR += sourceColor.R * kernel[i + radiusX, j + radiusY];
                    resultG += sourceColor.G * kernel[i + radiusX, j + radiusY];
                    resultB += sourceColor.B * kernel[i + radiusX, j + radiusY];
                }
            }

            return Color.FromArgb(Clamp((int)resultR, 0, 255),
                                  Clamp((int)resultG, 0, 255),
                                  Clamp((int)resultB, 0, 255));
        }

        public static Bitmap EmbossingExecute(Bitmap sourceImage)
        {
            image = sourceImage;
            float[,] kernelFilter = {{0, 1, 0}, {-1, 0, 1}, {0, -1, 0}};
            kernel = kernelFilter;
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color resultColor;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultColor = calculateNewPixelColor(i, j);
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            k = 100;
            resultImage = BrightnessExecute(resultImage);
            resultImage = GrayScaleExecute(resultImage);
            return resultImage;
        }

        public static Bitmap MotionBlurExecute(Bitmap sourceImage)
        {
            image = sourceImage;
            int sizeX = 9, sizeY = 9;
            kernel = new float[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if (i == j)
                    {
                        kernel[i, j] = 1.0f / sizeX;
                    } 
                    else
                    {
                        kernel[i, j] = 0.0f;
                    }
                }
            }
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color resultColor;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultColor = calculateNewPixelColor(i, j);
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            return resultImage;
        }

        public static Bitmap GrayWorldExecute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color sourceColor, resultColor;
            int N = sourceImage.Width * sourceImage.Height;
            float R_ = 0.0f, G_ = 0.0f, B_ = 0.0f, avg;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    sourceColor = sourceImage.GetPixel(i, j);
                    R_ += sourceColor.R;
                    G_ += sourceColor.G;
                    B_ += sourceColor.B;
                }
            }
            R_ /= N; G_ /= N; B_ /= N;
            avg = (R_ + G_ + B_) / 3;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    sourceColor = sourceImage.GetPixel(i, j);
                    resultColor = Color.FromArgb(Clamp((int)(sourceColor.R * avg / R_), 0, 255),
                                                 Clamp((int)(sourceColor.G * avg / G_), 0, 255),
                                                 Clamp((int)(sourceColor.B * avg / B_), 0, 255));
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            return resultImage;
        }

        public static Bitmap AutoLevelsExecute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color sourceColor, resultColor;
            float resultR, resultG, resultB;
            int Rmin = 255, Rmax = 0;
            int Gmin = 255, Gmax = 0;
            int Bmin = 255, Bmax = 0;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    sourceColor = sourceImage.GetPixel(i, j);
                    if (sourceColor.R > Rmax) Rmax = sourceColor.R;
                    if (sourceColor.R < Rmin) Rmin = sourceColor.R;
                    if (sourceColor.G > Gmax) Gmax = sourceColor.G;
                    if (sourceColor.G < Gmin) Gmin = sourceColor.G;
                    if (sourceColor.B > Bmax) Bmax = sourceColor.B;
                    if (sourceColor.B < Bmin) Bmin = sourceColor.B;
                }
            }
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    sourceColor = sourceImage.GetPixel(i, j);
                    resultR = Rmax == Rmin ? sourceColor.R : (sourceColor.R - Rmin) * 255.0f / (Rmax - Rmin);
                    resultG = Gmax == Gmin ? sourceColor.G : (sourceColor.G - Gmin) * 255.0f / (Gmax - Gmin);
                    resultB = Bmax == Bmin ? sourceColor.B : (sourceColor.B - Bmin) * 255.0f / (Bmax - Bmin);
                    resultColor = Color.FromArgb((int)resultR,
                                                 (int)resultG,
                                                 (int)resultB);
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            return resultImage;
        }

        public static Bitmap PerfectReflectorExecute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color sourceColor, resultColor;
            float resultR, resultG, resultB;
            int Rmax = 0, Gmax = 0, Bmax = 0;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    sourceColor = sourceImage.GetPixel(i, j);
                    if (sourceColor.R > Rmax) Rmax = sourceColor.R;
                    if (sourceColor.G > Gmax) Gmax = sourceColor.G;
                    if (sourceColor.B > Bmax) Bmax = sourceColor.B;
                }
            }
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    sourceColor = sourceImage.GetPixel(i, j);
                    resultR = Rmax == 0 ? 0 : sourceColor.R * 255.0f / Rmax;
                    resultG = Gmax == 0 ? 0 : sourceColor.G * 255.0f / Gmax;
                    resultB = Bmax == 0 ? 0 : sourceColor.B * 255.0f / Bmax;
                    resultColor = Color.FromArgb((int)resultR,
                                                 (int)resultG,
                                                 (int)resultB);
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            return resultImage;
        }

        public static Bitmap DilationExecute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color sourceColor, resultColor;
            int maxColor;
            int[,] structElem = { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
            int radiusX = structElem.GetLength(0) / 2;
            int radiusY = structElem.GetLength(1) / 2;
            int idX, idY;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    maxColor = 0;
                    for (int k = -radiusX; k <= radiusX; k++)
                    {
                        for (int l = -radiusY; l <= radiusY; l++)
                        {
                            idX = Clamp(i + k, 0, sourceImage.Width - 1);
                            idY = Clamp(j + l, 0, sourceImage.Height - 1);
                            sourceColor = sourceImage.GetPixel(idX, idY);
                            if ((structElem[radiusX + k, radiusY + l] == 1) && (sourceColor.R > maxColor))
                            {
                                maxColor = sourceColor.R;
                            }
                        }
                    }
                    resultColor = Color.FromArgb(maxColor, maxColor, maxColor);
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            return resultImage;
        }

        public static Bitmap ErosionExecute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color sourceColor, resultColor;
            int minColor;
            int[,] structElem = { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
            int radiusX = structElem.GetLength(0) / 2;
            int radiusY = structElem.GetLength(1) / 2;
            int idX, idY;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    minColor = 255;
                    for (int k = -radiusX; k <= radiusX; k++)
                    {
                        for (int l = -radiusY; l <= radiusY; l++)
                        {
                            idX = Clamp(i + k, 0, sourceImage.Width - 1);
                            idY = Clamp(j + l, 0, sourceImage.Height - 1);
                            sourceColor = sourceImage.GetPixel(idX, idY);
                            if ((structElem[radiusX + k, radiusY + l] == 1) && (sourceColor.R < minColor))
                            {
                                minColor = sourceColor.R;
                            }
                        }
                    }
                    resultColor = Color.FromArgb(minColor, minColor, minColor);
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            return resultImage;
        }

        public static Bitmap MedianExecute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color sourceColor, resultColor;
            int N = 9;
            int[] arrayR = new int[N];
            int[] arrayG = new int[N];
            int[] arrayB = new int[N];
            int radius = 1;
            int idX, idY;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    for (int k = -radius; k <= radius; k++)
                    {
                        for (int l = -radius; l <= radius; l++)
                        {
                            idX = Clamp(i + k, 0, sourceImage.Width - 1);
                            idY = Clamp(j + l, 0, sourceImage.Height - 1);
                            sourceColor = sourceImage.GetPixel(idX, idY);
                            arrayR[radius + k + 3 * (radius + l)] = sourceColor.R;
                            arrayG[radius + k + 3 * (radius + l)] = sourceColor.G;
                            arrayB[radius + k + 3 * (radius + l)] = sourceColor.B;
                        }
                    }
                    Array.Sort(arrayR);
                    Array.Sort(arrayG);
                    Array.Sort(arrayB);
                    resultColor = Color.FromArgb(arrayR[N / 2], arrayG[N / 2], arrayB[N / 2]);
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            return resultImage;
        }

        public static Bitmap SobelExecute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color sourceColor, resultColor;
            float[,] G_y = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
            float[,] G_x = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int radius = 1;
            int idX, idY;
            float resColorXR, resColorXG, resColorXB;
            float resColorYR, resColorYG, resColorYB;
            float resColorR, resColorG, resColorB;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resColorXR = 0; resColorXG = 0; resColorXB = 0;
                    resColorYR = 0; resColorYG = 0; resColorYB = 0;
                    for (int k = -radius; k <= radius; k++)
                    {
                        for (int l = -radius; l <= radius; l++)
                        {
                            idX = Clamp(i + k, 0, sourceImage.Width - 1);
                            idY = Clamp(j + l, 0, sourceImage.Height - 1);
                            sourceColor = sourceImage.GetPixel(idX, idY);
                            resColorXR += G_x[radius + k, radius + l] * sourceColor.R;
                            resColorXG += G_x[radius + k, radius + l] * sourceColor.G;
                            resColorXB += G_x[radius + k, radius + l] * sourceColor.B;
                            resColorYR += G_y[radius + k, radius + l] * sourceColor.R;
                            resColorYG += G_y[radius + k, radius + l] * sourceColor.G;
                            resColorYB += G_y[radius + k, radius + l] * sourceColor.B;
                        }
                    }
                    resColorR = (float)Math.Sqrt((resColorXR * resColorXR + resColorYR * resColorYR));
                    resColorG = (float)Math.Sqrt((resColorXG * resColorXG + resColorYG * resColorYG));
                    resColorB = (float)Math.Sqrt((resColorXB * resColorXB + resColorYB * resColorYB));
                    resultColor = Color.FromArgb(Clamp((int)resColorR, 0, 255),
                                                 Clamp((int)resColorG, 0, 255),
                                                 Clamp((int)resColorB, 0, 255));
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            return resultImage;
        }

        public static Bitmap ScharrExecute(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Color sourceColor, resultColor;
            float[,] G_y = { { -3, -10, -3 }, { 0, 0, 0 }, { 3, 10, 3 } };
            float[,] G_x = { { -3, 0, 3 }, { -10, 0, 10 }, { -3, 0, 3 } };
            int radius = 1;
            int idX, idY;
            float resColorXR, resColorXG, resColorXB;
            float resColorYR, resColorYG, resColorYB;
            float resColorR, resColorG, resColorB;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resColorXR = 0; resColorXG = 0; resColorXB = 0;
                    resColorYR = 0; resColorYG = 0; resColorYB = 0;
                    for (int k = -radius; k <= radius; k++)
                    {
                        for (int l = -radius; l <= radius; l++)
                        {
                            idX = Clamp(i + k, 0, sourceImage.Width - 1);
                            idY = Clamp(j + l, 0, sourceImage.Height - 1);
                            sourceColor = sourceImage.GetPixel(idX, idY);
                            resColorXR += G_x[radius + k, radius + l] * sourceColor.R;
                            resColorXG += G_x[radius + k, radius + l] * sourceColor.G;
                            resColorXB += G_x[radius + k, radius + l] * sourceColor.B;
                            resColorYR += G_y[radius + k, radius + l] * sourceColor.R;
                            resColorYG += G_y[radius + k, radius + l] * sourceColor.G;
                            resColorYB += G_y[radius + k, radius + l] * sourceColor.B;
                        }
                    }
                    resColorR = (float)Math.Sqrt((resColorXR * resColorXR + resColorYR * resColorYR));
                    resColorG = (float)Math.Sqrt((resColorXG * resColorXG + resColorYG * resColorYG));
                    resColorB = (float)Math.Sqrt((resColorXB * resColorXB + resColorYB * resColorYB));
                    resultColor = Color.FromArgb(Clamp((int)resColorR, 0, 255),
                                                 Clamp((int)resColorG, 0, 255),
                                                 Clamp((int)resColorB, 0, 255));
                    resultImage.SetPixel(i, j, resultColor);
                }
            }
            return resultImage;
        }
    }
}
