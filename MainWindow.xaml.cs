using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace jpeg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static readonly int[] Q_LUMINOSITY =
{
            16, 11, 10, 16, 24, 40, 51, 61,
            12, 12, 14, 19, 26, 58, 60, 55,
            14, 13, 16, 24, 40, 57, 69, 56,
            14, 17, 22, 29, 51, 87, 80, 62,
            18, 22, 37, 56, 68, 109, 103, 77,
            24, 35, 55, 64, 81, 104, 113, 92,
            49, 64, 78, 87, 103, 121, 120, 101,
            72, 92, 95, 98, 112, 100, 103, 99
        };
        public static readonly int[] Q_CHROMINANCE =
        {
            17, 18, 24, 47, 99, 99, 99, 99,
            18, 21, 26, 66, 99, 99, 99, 99,
            24, 26, 56, 99, 99, 99, 99, 99,
            47, 66, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99
        };

        public static readonly int[] ZIGZAG_TABLE = { 0, 1, 8, 16, 9,2,3,10,17,24,32,
            25,18,11,4,5,12,19,26,33,40,48,41,34,27,20,13,6,7,14,21,28,35,42,49,56,57,
            50,43,36,29,22,15,23,30,37,44,51,58,59,52,45,38,31,39,46,53,60,61,54,47,55,62,63 };

        BitmapImage srcBmp;

        WriteableBitmap yBmp;
        WriteableBitmap crBmp;
        WriteableBitmap cbBmp;

        Byte[] yValues;
        Byte[] crValues;
        Byte[] cbValues;

        public MainWindow()
        {
            InitializeComponent();

            byte[] tst0 =
            {
                1, 3, 4, 10, 11, 21, 22, 36,
                2, 5, 9, 12, 20, 23, 35, 37,
                6, 8, 13, 19, 24, 34, 38, 49,
                7, 14, 18, 25, 33, 39, 48, 50,
                15, 17, 26, 32, 40, 47, 51, 58,
                16, 27, 31, 41, 46, 52, 57, 59,
                28, 30, 42, 45, 53, 56, 60, 63,
                29, 43, 44, 54, 55, 61, 62, 64
            };

            byte[] tst =
{
                65, 56, 44, 33, 22, 18, 13, 6,
                65, 56, 44, 33, 22, 18, 13, 6,
                65, 56, 44, 33, 22, 18, 13, 6,
                65, 56, 44, 33, 22, 18, 13, 6,
                65, 56, 44, 33, 22, 18, 13, 6,
                65, 56, 44, 33, 22, 18, 13, 6,
                65, 56, 44, 33, 22, 18, 13, 6,
                65, 56, 44, 33, 22, 18, 13, 6,
            };

            byte[] tst1 =
{
                1, 3, 4, 10, 11, 21, 22, 36, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                2, 5, 9, 12, 20, 23, 35, 37, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                6, 8, 13, 19, 24, 34, 38, 49, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
                7, 14, 18, 25, 33, 39, 48, 50, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
                15, 17, 26, 32, 40, 47, 51, 58, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
                16, 27, 31, 41, 46, 52, 57, 59, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60,
                28, 30, 42, 45, 53, 56, 60, 63, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70,
                29, 43, 44, 54, 55, 61, 62, 64, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80,
                129, 143, 144, 154, 155, 161, 162, 164, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180,
                229, 243, 244, 254, 255, 61, 62, 64, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80
            };


////            double[] fwd = DCT.forwardSquare(tst, 8);
////            double[] inv = DCT.inverseSquare(fwd, 8);

////            int[] quant = Quantizer.quantize(fwd, Q_LUMINOSITY);
////            double[] dequant = Quantizer.unquantize(quant, Q_LUMINOSITY);

////            double[] final = DCT.inverseSquare(dequant, 8);

//            int[] zagged = zigzagify(quant, ZIGZAG_TABLE);
//            int[] unz = unzigzagify(zagged, ZIGZAG_TABLE);

//            byte[] writable = new byte[zagged.Length];
//            for(int i = 0; i < zagged.Length; i++)
//            {
//                writable[i] = (byte)zagged[i];
//            }

//            byte[] mrled = mxc.MRLE(writable);
//            byte[] unmrl = mxc.unMRLE(mrled);


//            int[][] squared = splitToSquares(tst1, 19, 10, 8);
//            int[] unsquared = unsquare(squared, 19, 10, 8);
//            Console.WriteLine("---");


        }

        int[] zigzagify(int[] victim, int[] zigzag)
        {
            int[] result = new int[victim.Length];
            for(int i = 0; i < victim.Length; i++)
            {
                result[zigzag[i]] = victim[i];
            }
            return result;
        }

        int[] unzigzagify(int[] zagged, int[] zigzag)
        {
            int[] result = new int[zagged.Length];
            for (int i = 0; i < zagged.Length; i++)
            {
                result[i] = zagged[zigzag[i]];
            }
            return result;

        }

        //int[][] subset(byte[] px, int width, int height, int side = 8)
        //{
        //    int xBlocks = width / side + (width % side > 0 ? 1 : 0);
        //    int yBlocks = height / side + (height % side > 0 ? 1 : 0);
            
        //    int[][] result = new int[xBlocks*yBlocks][];
        //    for(int x = 0; x < xBlocks; x++)
        //    {
        //        for(int y = 0; y < yBlocks; y++)
        //        {
        //            int iAdd = x * side;
        //            int jAdd = y * side;
        //            int[] square = new int[side * side];
        //            for(int i = 0; i<side; i++)
        //            {
        //                for(int j = 0; j < side; j++)
        //                {

        //                }
        //            }
        //        }
        //    }

        //    return result;
        //}

        int[][] splitToSquares(byte[] px, int width, int height, int squareSide)
        {
            int xBlocks = width / squareSide + (width % squareSide > 0 ? 1 : 0);
            int yBlocks = height / squareSide + (height % squareSide > 0 ? 1 : 0);

            int[][] result = new int[xBlocks*yBlocks][];

            for (int y = 0; y < yBlocks; y++)
            {
                for (int x = 0; x < xBlocks; x++)
                {
                    int vAdd = y * squareSide;
                    int uAdd = x * squareSide;
                    int[] tmp = new int[squareSide * squareSide];
                    for(int v = 0; v < squareSide; v++)
                    {
                        if(v+ vAdd >= height)
                        {
                            break;
                        }
                        for (int u = 0; u < squareSide; u++)
                        {
                            if(u + uAdd >= width)
                            {
                                break;
                            }
//                            Console.WriteLine(((v + vAdd) * width) + (u + uAdd) + " = " + (y * xBlocks + x) + "," + (v * squareSide + u));
                             tmp[v * squareSide + u] = px[(v + vAdd) * width + (u + uAdd)];
                        }
                    }
                    result[y * xBlocks + x] = tmp;
                }
            }
            return result;
        }

        private static int[] GetSubset(byte[] img, int offsetX, int offsetY, int imgHeight, int imgWidth, int side = 8)
        {
            int[] subset = new int[side * side];
            for (int i = 0; i < side; ++i)
            {
                for (int j = 0; j < side; ++j)
                {
                    int adjusterX = imgWidth - offsetX + i;
                    int adjusterY = imgHeight - offsetY + j;
                    subset[i * side + j] = img[(offsetY + j < imgHeight ? offsetY + j : imgHeight - adjusterY) 
                        * imgWidth + (offsetX + i < imgWidth ? offsetX + i : imgWidth - adjusterX)];
                }
            }
            return subset;
        }

        public static int[,] CreateSubsets(byte[] img, int width, int height, int squareSide = 8)
        {
            int xBlocks = width / squareSide + (width % squareSide > 0 ? 1 : 0);
            int yBlocks = height / squareSide + (height % squareSide > 0 ? 1 : 0);

            int[,] subsets = new int[xBlocks * yBlocks, squareSide * squareSide];
            for (int i = 0; i < width; i += squareSide)
            {
                for (int j = 0; j < height; j += squareSide)
                {
                    int[] tmp = GetSubset(img, i, j, height, width, squareSide);
                    for (int k = 0; k < tmp.Length; k++)
                    {
                        subsets[j / squareSide * xBlocks + i / squareSide, k] = tmp[k];
                    }
                    }
                }
            return subsets;
        }

        public static int[] ReassembleSubsets(int[,] subsets, int width, int height, int squareSide = 8)
        {
            int xBlocks = width / squareSide + (width % squareSide > 0 ? 1 : 0);
            int yBlocks = height / squareSide + (height % squareSide > 0 ? 1 : 0);

            int[] img = new int[width * height];
            for (int i = 0; i < xBlocks; ++i)
            {
                for (int j = 0; j < yBlocks; ++j)
                {
                    for (int k = 0; k < squareSide; ++k)
                    {
                        for (int h = 0; h < squareSide; ++h)
                        {
                            img[(j * squareSide + h) * width + (i * squareSide + k)] 
                                = subsets[j * xBlocks + i, h * squareSide + k];
                        }
                    }
                }
            }
            return img;
        }

        int[] unsquare(int[][] squares, int width, int height, int squareSide = 8)
        {
            int xBlocks = width / squareSide + (width % squareSide > 0 ? 1 : 0);
            int yBlocks = height / squareSide + (height % squareSide > 0 ? 1 : 0);

            int[] result = new int[width*height];

            for (int y = 0; y < yBlocks; y++)
            {
                for (int x = 0; x < xBlocks; x++)
                {
                    int vAdd = y * squareSide;
                    int uAdd = x * squareSide;
                    for (int v = 0; v < squareSide; v++)
                    {
                        if (v + vAdd >= height)
                        {
                            break;
                        }
                        for (int u = 0; u < squareSide; u++)
                        {
                            if (u + uAdd >= width)
                            {
                                break;
                            }
//                            Console.WriteLine(((v + vAdd) * width) + (u + uAdd) + " = " + (y * xBlocks + x) + "," +( v * squareSide + u));
                            result[((v + vAdd) * width) + (u + uAdd)] = squares[y * xBlocks + x][v * squareSide + u];
                        }
                    }
                }
            }
            return result;
        }
        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Common Images|*.bmp; *.png; *.jpeg; *.jpg";
            Nullable<Boolean> didSelect = dlg.ShowDialog();
            if (!didSelect.Value)
            {
                return;
            }
            BitmapImage inFile = new BitmapImage();
            inFile.BeginInit();
            inFile.UriSource = new Uri(dlg.FileName);
            inFile.EndInit();
            srcBmp = inFile;

            Byte[] srcPx = new Byte[srcBmp.PixelWidth * srcBmp.PixelHeight * srcBmp.Format.BitsPerPixel / 8];
            srcBmp.CopyPixels(srcPx, srcBmp.PixelWidth * srcBmp.Format.BitsPerPixel / 8, 0);

            int ps = 1;
            
            yValues = StandardBGRAtoY(srcPx, srcBmp.PixelWidth, srcBmp.PixelHeight, ps);

            Console.WriteLine(srcBmp.Format);

            yBmp = new WriteableBitmap(srcBmp.PixelWidth/ps, srcBmp.PixelHeight/ps,
                srcBmp.DpiX/ps, srcBmp.DpiY/ps, PixelFormats.Gray8, BitmapPalettes.Gray256);
            yBmp.WritePixels(new Int32Rect(0, 0, srcBmp.PixelWidth/ps, srcBmp.PixelHeight/ps),
                yValues, yBmp.PixelWidth * yBmp.Format.BitsPerPixel / 8, 0);

            yImage.Source = yBmp;

            ps = 2;

            crValues = StandardBGRAtoCr(srcPx, srcBmp.PixelWidth, srcBmp.PixelHeight, ps);
            cbValues = StandardBGRAtoCb(srcPx, srcBmp.PixelWidth, srcBmp.PixelHeight, ps);

            crBmp = new WriteableBitmap(srcBmp.PixelWidth / ps, srcBmp.PixelHeight / ps,
            srcBmp.DpiX / ps, srcBmp.DpiY / ps, PixelFormats.Gray8, BitmapPalettes.Gray256);
            crBmp.WritePixels(new Int32Rect(0, 0, srcBmp.PixelWidth / ps, srcBmp.PixelHeight / ps),
                crValues, crBmp.PixelWidth * crBmp.Format.BitsPerPixel / 8, 0);

            crImage.Source = crBmp;

            cbBmp = new WriteableBitmap(srcBmp.PixelWidth / ps, srcBmp.PixelHeight / ps,
            srcBmp.DpiX / ps, srcBmp.DpiY / ps, PixelFormats.Gray8, BitmapPalettes.Gray256);
            cbBmp.WritePixels(new Int32Rect(0, 0, srcBmp.PixelWidth / ps, srcBmp.PixelHeight / ps),
                cbValues, cbBmp.PixelWidth * cbBmp.Format.BitsPerPixel / 8, 0);

            cbImage.Source = cbBmp;
        }

        private Byte[] StandardBGRAtoY(byte[] pixels, int xMax, int yMax, int pixelStep = 1)
        {
            if (pixels == null)
            {
                return null;
            }

            Byte[] result = new Byte[pixels.Length / (4 * pixelStep * pixelStep)];

            for (int j = 0; j < result.Length; j++)
            {
                result[j] = 45;
            }

            int i = 0;
            for (int y = 0; y < (yMax); y += pixelStep)
            {
                for (int x = 0; x < (xMax); x += pixelStep)
                {
                    result[i] = (byte)Math.Min(Math.Max((0 + (float)(pixels[(y * 4 * (xMax)) + x * 4 + 2] * 0.299)
                        + (float)(pixels[(y * 4 * (xMax)) + x * 4 + 1]) * 0.587
                        + (float)(pixels[(y * 4 * (xMax)) + x * 4]) * 0.114), 0), 255);
                    i++;
                }
            }

            return result;
        }

        private Byte[] StandardBGRAtoCr(byte[] pixels, int xMax, int yMax, int pixelStep = 1)
        {
            if (pixels == null)
            {
                return null;
            }

            Byte[] result = new Byte[pixels.Length / (4 * pixelStep * pixelStep)];

            for(int j = 0; j < result.Length; j++)
            {
                result[j] = 45;
            }

            int i = 0;
            for (int y = 0; y < (yMax); y+=pixelStep)
            {
                for (int x = 0; x < (xMax); x+=pixelStep)
                {
                    result[i] = (byte)Math.Min(Math.Max((128 + (float)(pixels[(y * 4 * (xMax)) + x * 4 + 2] * 0.5)
                        - (float)(pixels[(y * 4 * (xMax)) + x * 4 + 1]) * 0.418688
                        - (float)(pixels[(y * 4 * (xMax)) + x * 4]) * 0.081312), 0), 255);
                    i++;
                }
            }

            return result;
        }

        private Byte[] StandardBGRAtoCb(byte[] pixels, int xMax, int yMax, int pixelStep = 1)
        {
            if (pixels == null)
            {
                return null;
            }

            Byte[] result = new Byte[pixels.Length / (4 * pixelStep * pixelStep)];

            for (int j = 0; j < result.Length; j++)
            {
                result[j] = 45;
            }

            int i = 0;
            for (int y = 0; y < (yMax); y += pixelStep)
            {
                for (int x = 0; x < (xMax); x += pixelStep)
                {
                    result[i] = (byte)Math.Min(Math.Max((128 - (float)(pixels[(y * 4 * (xMax)) + x * 4 + 2] * 0.168736)
                        - (float)(pixels[(y * 4 * (xMax)) + x * 4 + 1]) * 0.331268
                        + (float)(pixels[(y * 4 * (xMax)) + x * 4]) * 0.5), 0), 255);
                    i++;
                }
            }

            return result;
        }

        private void reassembleBtn_Click(object sender, RoutedEventArgs e)
        {
            yImage.Source = assembleFromJpegStreams(yValues, crValues, cbValues, yBmp.PixelWidth, yBmp.PixelHeight);
        }

        WriteableBitmap assembleFromJpegStreams(byte[] yPx, byte[] crPx, byte[] cbPx, int xSize, int ySize)
        {
            if(yPx.Length != xSize * ySize)
            {
                throw new ArgumentException("Wrong size given?");
            }
            if(yPx.Length != crPx.Length*4 || yPx.Length != cbPx.Length * 4)
            {
                throw new ArgumentException("cr and/or cb component not correctly subsampled.");
            }

            byte[] result = new byte[yPx.Length * 4];

            int i = 0;
            for(int y = 0; y < ySize; y++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    result[i + 2] = (byte)Math.Min(Math.Max(yPx[y * (xSize) + x] + 1.402 * ((float)crPx[y/2 * (xSize)/2 + x/2] - 128), 0), 255);
                    result[i + 1] = (byte)Math.Min(Math.Max((yPx[y * (xSize) + x] - 0.344136 * (cbPx[(y/2 * (xSize)/2 + x/2)] - 128) 
                        - 0.714136 * (float)(crPx[(y/2 * (xSize)/2 + x/2)] - 128)), 0), 255);
                    result[i] = (byte)Math.Min(Math.Max(yPx[y * (xSize) + x] + 1.772 * (cbPx[(y/2 * (xSize)/2 + x/2)] - 128), 0), 255);

                    result[i + 3] = 255;
                    i += 4;
                }
            }

            WriteableBitmap ret = new WriteableBitmap(xSize, ySize,
            300, 300, PixelFormats.Bgra32, null);
            ret.WritePixels(new Int32Rect(0, 0, xSize, ySize),
                result, xSize * ret.Format.BitsPerPixel / 8, 0);

            return ret;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            int[][] ySplit = splitToSquares(yValues, yBmp.PixelWidth, yBmp.PixelHeight, 8);
            int[][] crSplit = splitToSquares(crValues, crBmp.PixelWidth, crBmp.PixelHeight, 8);
            int[][] cbSplit = splitToSquares(cbValues, cbBmp.PixelWidth, cbBmp.PixelHeight, 8);

            Console.WriteLine(ySplit.Length);

            for(int i = 0; i < ySplit.Length; i++)
            {
                ySplit[i] = Quantizer.quantize(DCT.forwardSquare(ySplit[i], 8), Q_LUMINOSITY);
            }
            for(int i = 0; i < crSplit.Length; i++)
            {
                crSplit[i] = Quantizer.quantize(DCT.forwardSquare(crSplit[i], 8), Q_CHROMINANCE);
                cbSplit[i] = Quantizer.quantize(DCT.forwardSquare(cbSplit[i], 8), Q_CHROMINANCE);
            }

            int[] yReady = unsquare(ySplit, yBmp.PixelWidth, yBmp.PixelHeight, 8);
            int[] crReady = unsquare(crSplit, crBmp.PixelWidth, crBmp.PixelHeight, 8);
            int[] cbReady = unsquare(cbSplit, cbBmp.PixelWidth, cbBmp.PixelHeight, 8);

            byte[] yBytes = new byte[yReady.Length];
            byte[] crBytes = new byte[crReady.Length];
            byte[] cbBytes = new byte[cbReady.Length];

            for(int i = 0;i < yReady.Length; i++)
            {
                yBytes[i] = (byte)yReady[i];
            }
            for(int i = 0; i < crReady.Length; i++)
            {
                crBytes[i] = (byte)crReady[i];
                cbBytes[i] = (byte)cbReady[i];
            }

            mxc.writeFile(yBytes, crBytes, cbBytes, yBmp.PixelWidth, yBmp.PixelHeight, "export.mxc");

            Console.WriteLine("---_---");
        }

        private int[] DoubleArrToIntArr(double[] dbl)
        {
            int[] result = new int[dbl.Length];
            for(int i = 0; i < dbl.Length; i++)
            {
                result[i] = (int)Math.Round(dbl[i]);
            }
            return result;
        }

        private byte[] IntArrToByteArr(int[] ints)
        {
            byte[] result = new byte[ints.Length];
            for(int i = 0; i < ints.Length; i++)
            {
                result[i] = (byte)ints[i];
            }
            return result;
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            MXC_CONTAINER mxcCont = mxc.readFile("export.mxc");

            int ps = 1;

            int[][] ySquares = splitToSquares(mxcCont.yPx, mxcCont.width, mxcCont.height, 8);
            for (int i = 0; i < ySquares.Length; i++)
            {
                ySquares[i] = DoubleArrToIntArr(DCT.inverseSquare(Quantizer.unquantize(ySquares[i], Q_LUMINOSITY), 8));

            }

            yValues = IntArrToByteArr(unsquare(ySquares, mxcCont.width, mxcCont.height, 8));

            yBmp = new WriteableBitmap(mxcCont.width, mxcCont.height,
                300, 300, PixelFormats.Gray8, BitmapPalettes.Gray256);
            yBmp.WritePixels(new Int32Rect(0, 0, mxcCont.width, mxcCont.height),
                yValues, yBmp.PixelWidth * yBmp.Format.BitsPerPixel / 8, 0);

            yImage.Source = yBmp;

            ps = 2;

            int[][] crSquares = splitToSquares(mxcCont.crPx, mxcCont.width / 2, mxcCont.height / 2, 8);
            for(int i = 0; i < crSquares.Length; i++)
            {
                crSquares[i] = DoubleArrToIntArr(DCT.inverseSquare(Quantizer.unquantize(crSquares[i], Q_CHROMINANCE), 8));

            }
            int[][] cbSquares = splitToSquares(mxcCont.cbPx, mxcCont.width / 2, mxcCont.height / 2, 8);
            for (int i = 0; i < cbSquares.Length; i++)
            {
                cbSquares[i] = DoubleArrToIntArr(DCT.inverseSquare(Quantizer.unquantize(cbSquares[i], Q_CHROMINANCE), 8));
            }

            crValues = IntArrToByteArr(unsquare(crSquares, mxcCont.width /2, mxcCont.height / 2));
            cbValues = IntArrToByteArr(unsquare(cbSquares, mxcCont.width / 2, mxcCont.height / 2));

            crBmp = new WriteableBitmap(mxcCont.width / 2, mxcCont.height / 2,
            150, 150, PixelFormats.Gray8, BitmapPalettes.Gray256);
            crBmp.WritePixels(new Int32Rect(0, 0, mxcCont.width / 2, mxcCont.height / 2),
                crValues, crBmp.PixelWidth * crBmp.Format.BitsPerPixel / 8, 0);

            crImage.Source = crBmp;

            cbBmp = new WriteableBitmap(mxcCont.width / 2, mxcCont.height / 2,
            150, 150, PixelFormats.Gray8, BitmapPalettes.Gray256);
            cbBmp.WritePixels(new Int32Rect(0, 0, mxcCont.width / 2, mxcCont.height / 2),
                cbValues, cbBmp.PixelWidth * cbBmp.Format.BitsPerPixel / 8, 0);

            cbImage.Source = cbBmp;
        }
    }
}
