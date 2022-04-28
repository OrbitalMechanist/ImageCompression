using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jpeg
{
    public class MXC_CONTAINER
    {
        public int width;
        public int height;
        public int yMrledLen;
        public int crMrledLen;
        public int cbMrledLen;
        public byte[] yPx;
        public byte[] crPx;
        public byte[] cbPx;
    }
    internal class mxc
    {
        public static byte[] MRLE(byte[] ogStream, byte runmarker = 0)
        {
            List<byte> result = new List<byte>();
            int i = 0;
            while (i < ogStream.Length)
            {
                byte count = 1;
                byte c = ogStream[i];
                while (i + count < ogStream.Length && c == ogStream[i + count] && count < 255)
                {
                    count++;
                }
                if(count == 1 && c != runmarker)
                {
                    result.Add(c);
                } else if (count == 2 && c != runmarker)
                {
                    result.Add(c);
                    result.Add(c);
                } else
                {
                    result.Add(runmarker);
                    result.Add(count);
                    result.Add(c);
                }
                i += count;
            }
            result.TrimExcess();
            return result.ToArray();
        }

        public static byte[] unMRLE(byte[] compressed, byte runmarker = 0)
        {
            List<byte> result = new List<byte>();

            for(int i = 0; i < compressed.Length;)
            {
                byte b = compressed[i];
                if (b == runmarker)
                {
                    for (int j = 0; j < compressed[i + 1]; j++)
                    {
                        result.Add(compressed[i + 2]);
                    }
                    i += 3;
                } else
                {
                    result.Add(b);
                    i++;
                }
            }

            return result.ToArray();
        }

        public static void writeFile(byte[] yWritable, byte[] crWriteable, byte[] cbWriteable, int width, int height, string path)
        {
            byte[] yMrle = MRLE(yWritable);
            byte[] cbMrle = MRLE(cbWriteable);
            byte[] crMrle = MRLE(crWriteable);

            byte[] data = new byte[4 + 4 + 4 + 4 + 4 + yMrle.Length + cbMrle.Length + crMrle.Length];

            data[0] = (byte)(width & 0xFF);
            data[1] = (byte)((width & 0xFF00) >> 8);
            data[2] = (byte)((width & 0xFF0000) >> 16);
            data[3] = (byte)((width & 0xFF000000) >> 24);

            data[4] = (byte)(height & 0xFF);
            data[5] = (byte)((height & 0xFF00) >> 8);
            data[6] = (byte)((height & 0xFF0000) >> 16);
            data[7] = (byte)((height & 0xFF000000) >> 24);

            data[8] = (byte)(yMrle.Length & 0xFF);
            data[9] = (byte)((yMrle.Length & 0xFF00) >> 8);
            data[10] = (byte)((yMrle.Length & 0xFF0000) >> 16);
            data[11] = (byte)((yMrle.Length & 0xFF000000) >> 24);

            data[12] = (byte)(crMrle.Length & 0xFF);
            data[13] = (byte)((crMrle.Length & 0xFF00) >> 8);
            data[14] = (byte)((crMrle.Length & 0xFF0000) >> 16);
            data[15] = (byte)((crMrle.Length & 0xFF000000) >> 24);

            Console.WriteLine(cbMrle.Length);
            data[16] = (byte)(cbMrle.Length & 0xFF);
            data[17] = (byte)((cbMrle.Length & 0xFF00) >> 8);
            data[18] = (byte)((cbMrle.Length & 0xFF0000) >> 16);
            data[19] = (byte)((cbMrle.Length & 0xFF000000) >> 24);

            int t = 20;
            foreach(byte b in yMrle)
            {
                data[t] = b;
                t++;
            }
            foreach (byte b in crMrle)
            {
                data[t] = b;
                t++;
            }
            foreach (byte b in cbMrle)
            {
                data[t] = b;
                t++;
            }
            File.WriteAllBytes(path, data);
        }

        public static MXC_CONTAINER readFile(string path)
        {
            MXC_CONTAINER result = new MXC_CONTAINER();

            byte[] data = File.ReadAllBytes(path);

            int width = data[0] + (data[1] << 8) + (data[2] << 16) + (data[3] << 24);
            int height = data[4] + (data[5] << 8) + (data[6] << 16) + (data[7] << 24);

            int yMrleLen = data[8] + (data[9] << 8) + (data[10] << 16) + (data[11] << 24);
            int crMrleLen = data[12] + (data[13] << 8) + (data[14] << 16) + (data[15] << 24);
            int cbMrleLen = data[16] + (data[17] << 8) + (data[18] << 16) + (data[19] << 24);

            int t = 20;
            byte[] yPx = new byte[yMrleLen];
            for (int i = 0; i < yMrleLen; i++)
            {
                yPx[i] = data[t];
                t++;
            }
            byte[] crPx = new byte[crMrleLen];
            for (int i = 0; i < crMrleLen; i++)
            {
                crPx[i] = data[t];
                t++;
            }
            byte[] cbPx = new byte[cbMrleLen];
            for (int i = 0; i < cbMrleLen; i++)
            {
                cbPx[i] = data[t];
                t++;
            }

            result.width = width;
            result.height = height;
            result.yMrledLen = yMrleLen;
            result.cbMrledLen = cbMrleLen;
            result.crMrledLen = crMrleLen;
            result.yPx = unMRLE(yPx);
            result.crPx = unMRLE(crPx);
            result.cbPx = unMRLE(cbPx);

            return result;
        }
    }
}
