using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jpeg
{
    internal class Quantizer
    {
        public static int[] quantize(double[] victim, int[] coeffs)
        {
            int[] reslut = new int[coeffs.Length];
            for (int i = 0; i < coeffs.Length; i++)
            {
                reslut[i] = (int)(victim[i] / coeffs[i]) + 128;
            }
            return reslut;
        }

        public static double[] unquantize(int[] victim, int[] coeffs)
        {
            double[] result = new double[coeffs.Length];
            for(int i = 0;i < coeffs.Length; i++)
            {
                result[i] = (victim[i] - 128) * coeffs[i];
            }
            return result;
        }
    }
}
