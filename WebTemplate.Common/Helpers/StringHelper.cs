using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace WebTemplate.Common.Helpers
{
    public static class StringHelper
    {

        /// <summary>
        /// 정수변환
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this string value)
        {
            value = value.Replace(".00", "");
            if (string.IsNullOrEmpty(value)) { return 0; }
            int iout = 0;
            if (int.TryParse(value, out iout))
                return iout;
            return iout;
        }

        public static int ToIntComma(this string value)
        {
            var arv = value.Split('.');
            if (arv.Length > 1)
                value = arv[0];

            if (string.IsNullOrEmpty(value)) { return 0; }
            int iout = 0;
            if (int.TryParse(value, out iout))
                return iout;
            return iout;
        }


        public static string GetKoreaDate(this DateTime value)
        {
            CultureInfo cultures = CultureInfo.CreateSpecificCulture("ko-KR");

            return value.ToString(string.Format("yyyy년 MM월dd일 HH시mm분 ddd요일", cultures));

        }


        /// <summary>
        /// 콤마찍기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToComma(this double value)
        {
            return String.Format("{0:##,##}", value);

        }

        public static string ToComma(this int value)
        {
            return String.Format("{0:##,##}", value);

        }



        /// <summary>
        /// 숫자만 뽑기
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToNumber(this string value)
        {
            if (string.IsNullOrEmpty(value)) { return 0; }
            var str = Regex.Replace(value, @"\D", "");
            int iout = 0;
            if (int.TryParse(str, out iout))
                return iout;
            return iout;
        }




        /// <summary>
        /// 정수인지 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumber(this string value)
        {
            if (string.IsNullOrEmpty(value)) { return false; }
            var pattern = "^[0-9]*$";
            return new Regex(pattern).IsMatch(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] SplitStart(this string value, char separator)
        {
            string[] fields = new string[2];
            int fi = 0;
            foreach (char c in value)
            {
                if (c.Equals(separator) && fi <= 0) { fi++; continue; }
                fields[fi] += c;
            }
            return fields;
        }

        /// <summary>
        /// Null여부체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {

            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            var szTemp = value.Trim();
            if (string.IsNullOrEmpty(szTemp))
            {
                return true;
            }

            szTemp = value.Replace(" ", "");
            if (string.IsNullOrEmpty(szTemp))
            {
                return true;
            }

            szTemp = value.Replace("\n", "").Replace("\r", "");
            if (string.IsNullOrEmpty(szTemp))
            {
                return true;
            }

            return false;
        }


        public static int CalcByteCount(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0;
            }

            int byteCount = 0;
            foreach (char c in input.ToCharArray())
            {
                byteCount = CalcByteCount(c, byteCount);
            }
            return byteCount;
        }

        const int maxAnsiCode = 255;
        public static int CalcByteCount(this char c, int byteCount = 0)
        {
            if (c > maxAnsiCode)
                byteCount += 2;
            else byteCount += 1;
            return byteCount;
        }

        public static int CalcByteForCount(this string text, int max)
        {
            var count = 0;
            var nb = 0;
            var temp = 0;
            var cs = text.ToCharArray();
            for (int i = 0; i < cs.Length; i++)
            {
                temp = cs[i].CalcByteCount(nb);
                if (temp > max) { break; }

                nb = temp;
                count = i;
            }
            return count + 1;
        }

        public static string SubstringExtension(this string val, int subcnt)
        {
            string strrt = string.Empty;

            if (val.Length > subcnt)
                strrt = val.Substring(0, subcnt) + "..";
            else
                strrt = val;
            return strrt;
        }


        public static string Serialize<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();

                XmlWriterSettings settings = new XmlWriterSettings();
                // settings.NewLineHandling = NewLineHandling.Entitize;

                settings.Encoding = Encoding.UTF8;
                settings.Encoding = Encoding.GetEncoding("utf-8");
                settings.Indent = true;

                using (var writer = XmlWriter.Create(stringWriter, settings))
                {
                    xmlserializer.Serialize(writer, value);

                    return stringWriter.ToString().Replace("utf-16", "utf-8").Replace("&lt;", "<").Replace("&gt;", ">");

                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }


        public static bool TryConvertDate(this string source, out DateTime dt)
        {
            if (DateTime.TryParse(source, out dt))
            {
                return true;
            }
            else
            {
                var dc = $"{source.Substring(0, 4)}-{source.Substring(4, 2)}-{source.Substring(6, 2)}";
                if (source.Length == 12)
                    dc += $" {source.Substring(8, 2)}:{source.Substring(10, 2)}";
                if (DateTime.TryParse(dc, out dt))
                    return true;
                else
                    return false;
            }
        }

        public static string GenerateCdata(this string content)
        {
            string cdata = "<![CDATA[{MENT}]]>";
            return cdata.Replace("{MENT}", content);//.Replace("<","").Replace(">", ""));
        }

        public static string Base64Encode(this string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in Base64Encode: " + e.Message);
            }
        }

        public static string Base64Decode(this string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in Base64Decode: " + e.Message);
            }
        }

    }
}
