using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools
{
    public class Util 
    {
        public static void TimePause() {
            Time.timeScale = 0;
        }

        public static void TimeRun() {
            Time.timeScale = 1;
        }

        public static string merageLogStr(params object[] paramsList)
        {
            if (paramsList.Length < 1)
                return "";

            string str = paramsList[0].ToString();
            for (int i = 1; i < paramsList.Length; ++i)
            {
                int index = str.IndexOf("{}");
                if (index != -1)
                    str = str.Remove(index, 2).Insert(index, paramsList[i].ToString());
                else
                    str += paramsList[i].ToString();
            }
            return str;
        }
        public static string GetPath(string strPath,bool flag) {
            strPath = string.Format("{0}/{1}", Application.streamingAssetsPath, strPath);
            return strPath;
        }

        static int tempSwap;
        public static void Swap(ref int left, ref int right) {
            tempSwap = left;
            left = right;
            right = tempSwap;
        }

        public static int GetRand(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static string CompressString(string input)
        {
            // 将字符串转换为字节数组
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            // 创建一个内存流
            MemoryStream ms = new MemoryStream();
            // 创建一个GZipStream对象
            GZipStream gz = new GZipStream(ms, CompressionMode.Compress);
            // 将字节数组写入GZipStream对象中
            gz.Write(bytes, 0, bytes.Length);
            // 关闭GZipStream对象
            gz.Close();
            // 获取内存流中的压缩后的字节数组
            byte[] compressedBytes = ms.ToArray();
            // 关闭内存流
            ms.Close();
            // 将压缩后的字节数组转换为base64字符串
            string compressedString = Convert.ToBase64String(compressedBytes);
            return compressedString;
        }

        // 解压缩字符串
        public static string DecompressString(string compressedString)
        {
            // 将base64字符串转换为字节数组
            byte[] compressedBytes = Convert.FromBase64String(compressedString);
            // 创建一个内存流
            MemoryStream ms = new MemoryStream(compressedBytes);
            // 创建一个GZipStream对象
            GZipStream gz = new GZipStream(ms, CompressionMode.Decompress);
            // 创建一个字节缓冲区
            byte[] buffer = new byte[1024];
            // 创建一个新的内存流
            MemoryStream output = new MemoryStream();
            // 从GZipStream对象中读取解压后的字节并写入新的内存流中
            int count = 0;
            while ((count = gz.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, count);
            }
            // 关闭GZipStream对象
            gz.Close();
            // 关闭内存流
            ms.Close();
            // 获取新内存流中的解压后的字节数组
            byte[] decompressedBytes = output.ToArray();
            // 关闭新内存流
            output.Close();
            // 将解压后的字节数组转换为字符串
            string input = Encoding.UTF8.GetString(decompressedBytes);
            return input;
        }
    }
}
