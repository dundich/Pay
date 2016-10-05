using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Maybe2
{
    public static class Coder
    {
        const string PSW_VALID = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ123456789";

        public static string GeneratePassword(this string valid, int length = 6)
        {
            valid = valid.PackToNull() ?? PSW_VALID;

            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }


        static System.Text.Encoding ASCII_1251 = System.Text.Encoding.GetEncoding(1251);

        /// <summary>
        /// Чтение данных из DBF в кодировке DOS866
        /// </summary>
        /// <param name="source">bytes</param>
        /// <returns>UTF8</returns>
        public static string Dos866ToString(this string source)
        {
            Encoding win1251 = ASCII_1251;// Encoding.GetEncoding("windows-1251");
            byte[] srcBytes = win1251.GetBytes(source);
            byte[] dstBytes = Encoding.Convert(Encoding.GetEncoding(866), win1251, srcBytes);
            return win1251.GetString(dstBytes);
        }

        /// <summary>
        /// 23-35-4E-2B -> "vdvdv" 
        /// </summary>                
        public static string FromHexString(this string hexString, char separator = '-')
        {
            if (hexString == null) return null;
            var bytes = hexString.Split(separator).Select(c => Convert.ToByte(c, 16)).ToArray();
            return ASCII_1251.GetString(bytes);
        }

        /// <summary>
        /// "vdvdv" -> 23-35-4E-2B
        /// </summary>
        public static string ToHexString(this string str, char separator = '-')
        {
            if (str == null) return null;
            var bytes = ASCII_1251.GetBytes(str);
            var result = BitConverter.ToString(bytes);
            if (separator != '-')
                result = result.Replace('-', separator);
            return result;
        }


        public static string ToBase64UrlEncoded(this string s)
        {
            if (s == null) return null;

            var bytes = Encoding.UTF8.GetBytes(s);

            s = Convert.ToBase64String(bytes);
            s = s.Split('=')[0]; // Remove any trailing '='s
            s = s.Replace('+', '-'); // 62nd char of encoding
            s = s.Replace('/', '_'); // 63rd char of encoding

            return s;
        }

        public static string FromBase64UrlEncoded(this string s)
        {
            if (s == null) return null;

            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding

            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: s += "=="; break; // Two pad chars
                case 3: s += "="; break; // One pad char
                default: throw new Exception("Illegal base64url string!");
            }

            var bytes = Convert.FromBase64String(s);
            return Encoding.UTF8.GetString(bytes);
        }


        public static string EncodeTo64(this string data)
        {
            if (data == null) return null;
            try
            {
                byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(data);
                return Convert.ToBase64String(encbuff);
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }

        public static string DecodeFrom64(this string data)
        {
            if (data == null) return null;
            try
            {
                byte[] decbuff = Convert.FromBase64String(data);
                return System.Text.Encoding.UTF8.GetString(decbuff);
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }


        public static string Compress(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;
            MemoryStream outStream = new MemoryStream();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

        public static string Decompress(this string compressedText)
        {
            if (string.IsNullOrWhiteSpace(compressedText))
                return compressedText;

            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);

                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }
                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }

        #region data
        static Dictionary<char, char> rus_lat_keys = new Dictionary<char, char>
        {
            {'Й','Q'},
            {'Ц','W'},
            {'У','E'},
            {'К','R'},
            {'Е','T'},
            {'Н','Y'},
            {'Г','U'},
            {'Ш','I'},
            {'Щ','O'},
            {'З','P'},
            {'Ф','A'},
            {'Ы','S'},
            {'В','D'},
            {'А','F'},
            {'П','G'},
            {'Р','H'},
            {'О','J'},
            {'Л','K'},
            {'Д','L'},
            {'Я','Z'},
            {'Ч','X'},
            {'С','C'},
            {'М','V'},
            {'И','B'},
            {'Т','N'},
            {'Ь','M'},


            {'й','q'},
            {'ц','w'},
            {'у','e'},
            {'к','r'},
            {'е','t'},
            {'н','y'},
            {'г','u'},
            {'ш','i'},
            {'щ','o'},
            {'з','p'},
            {'ф','a'},
            {'ы','s'},
            {'в','d'},
            {'а','f'},
            {'п','g'},
            {'р','h'},
            {'о','j'},
            {'л','k'},
            {'д','l'},
            {'я','z'},
            {'ч','x'},
            {'с','c'},
            {'м','v'},
            {'и','b'},
            {'т','n'},
            {'ь','m'}
         };


        static Dictionary<char, char> lat_rus_keys = new Dictionary<char, char>
        {
            {'Q','Й'},
            {'W','Ц'},
            {'E','У'},
            {'R','К'},
            {'T','Е'},
            {'Y','Н'},
            {'U','Г'},
            {'I','Ш'},
            {'O','Щ'},
            {'P','З'},
            {'A','Ф'},
            {'S','Ы'},
            {'D','В'},
            {'F','А'},
            {'G','П'},
            {'H','Р'},
            {'J','О'},
            {'K','Л'},
            {'L','Д'},
            {'Z','Я'},
            {'X','Ч'},
            {'C','С'},
            {'V','М'},
            {'B','И'},
            {'N','Т'},
            {'M','Ь'},


            {'q','й'},
            {'w','ц'},
            {'e','у'},
            {'r','к'},
            {'t','е'},
            {'y','н'},
            {'u','г'},
            {'i','ш'},
            {'o','щ'},
            {'p','з'},
            {'a','ф'},
            {'s','ы'},
            {'d','в'},
            {'f','а'},
            {'g','п'},
            {'h','р'},
            {'j','о'},
            {'k','л'},
            {'l','д'},
            {'z','я'},
            {'x','ч'},
            {'c','с'},
            {'v','м'},
            {'b','и'},
            {'n','т'},
            {'m','ь'}
        };
        #endregion

        public static string switchRusToLat(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;
            return new string(s.Select(c => rus_lat_keys.GetOrDefault(c, c)).ToArray());
        }

        public static string switchLatToRus(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;
            return new string(s.Select(c => lat_rus_keys.GetOrDefault(c, c)).ToArray());
        }
    }
}