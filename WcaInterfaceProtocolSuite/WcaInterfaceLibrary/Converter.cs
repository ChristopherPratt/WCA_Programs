using System;
using System.Collections.Generic;
using System.Text;

namespace WcaInterfaceLibrary
{
    public class Converter
    {
        public static string ByteArrayToASCIIString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            if (arr != null && arr.Length > 0)
            {
                return enc.GetString(arr);
            }

            return "";
        }

        public static string ByteArrayToASCIIString(byte[] arr, int length)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            if (arr != null && arr.Length > 0)
            {
                return enc.GetString(arr, 0, length);
            }

            return "";
        }

        public static string ByteArray2String(byte[] bytes)
        {
            string ret = "";

            if (bytes != null && bytes.Length > 0)
            {
                int size = bytes.Length;

                foreach (byte b in bytes)
                {
                    if (size-- > 0)
                    {
                        ret += b.ToString("X2") + " ";
                    }
                }
            }
            return ret;
        }

        public static byte[] StringToByteArray(string str)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }

        public static string ByteArrayToString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            if (arr != null && arr.Length > 0)
            {
                return enc.GetString(arr);
            }

            return "";
        }

        public static string ByteArrayToString(byte[] arr, int byteStartIndex, int byteEndIndex)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            if (arr != null && arr.Length > 0)
            {
                int byteCount = byteEndIndex - byteStartIndex;
                return enc.GetString(arr, byteStartIndex, byteCount);
            }

            return "";
        }

        public static byte[] ChangeEndian(byte[] ba)
        {
            if (ba != null && ba.Length > 0)
            {
                byte[] n = new byte[ba.Length];
                int l = ba.Length;

                for (int i = 0; i < l; ++i)
                {
                    n[l - i - 1] = ba[i];
                }
                return n;
            }
            return null;
        }
    }
    
}
