using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jpeg
{
    internal class DCT
    {
        const double PI = Math.PI;
        public static double[] forwardSquare(int[] h, int n)
        {
            double[] H = new double[h.Length];
            for(int v = 0; v < n; v++)
            {
                for(int u = 0; u < n; u++)
                {
                    double tmp = 0;
                    for(int y = 0; y < n; y++)
                    {
                        for (int x = 0; x < n; x++)
                        {
                            double val = Math.Cos((u * PI * (2 * x + 1)) / (double)(2 * n)) 
                                * Math.Cos(((v * PI * (2 * y + 1)) / (double)(2 * n))) * (double)h[y * n + x];
                            tmp += val;
                        }
                    }
                    H[v * n + u] = (tmp * (C(u) * C(v) * (2 / (double)n)));
                }
            }
            return H;
        }

        public static double[] inverseSquare(double[] H, int n) {
            double[] h = new double[H.Length];
            for(int f  = 0; f < H.Length; f++)
            {
                h[f] = 12;
            }
            for(int y = 0; y<n; y++) {
                for(int x = 0; x < n; x++)
                {
                    double tmp = 0;
                    for(int v =0; v<n; v++)
                    {
                        for(int u = 0; u < n; u++)
                        {
                            tmp += C(u) * C(v) * Math.Cos((u * PI * (2 * x + 1)) / (2 * n)) 
                                * Math.Cos((v * PI * (2 * y + 1)) / (2 * n)) * (double)H[v * n + u];
                        }
                    }
                    h[y * n + x] = ((2 / (double)n) * tmp);
                }
            }
            return h;
        }

        static double C(int x)
        {
            if(x == 0)
            {
                return 1/Math.Sqrt(2);
            } else
            {
                return 1;
            }
        }
    }
}
