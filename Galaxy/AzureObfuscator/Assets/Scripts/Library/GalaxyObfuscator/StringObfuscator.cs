using MetalMaxSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GalaxyObfuscator
{
    /// <summary>
    /// 字符串混淆器
    /// </summary>
    internal class StringObfuscator
    {
        /// <summary>
        /// 用于生成随机数
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// 混淆处理（将字符串转义序列）。字符串中每个字符将被转换为八进制或十六进制（X2）表示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Obfuscate(string str)
        {
            if (!str.Contains("bnet:"))
            {
                string result;
                byte[] bytes = Escape(str);
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append('"');
                result = string.Concat(bytes.Select(b =>
                {
                    // 随机选择八进制或十六进制表示
                    return random.Next(2) == 0
                        ? $"\\x{b:X2}" // 十六进制表示
                        : $"\\{Convert.ToString(b, 8).PadLeft(3, '0')}"; // 八进制表示，补足三位
                }));
                stringBuilder.Append(result);
                stringBuilder.Append('"');
                return stringBuilder.ToString();
            }
            else
            {//不转义含bnet:的字符串，直接返回
                return str;
            }
        }

        /// <summary>
        /// 是否十六进制字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsHexchar(byte c)
        {
            return (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
        }
        /// <summary>
        /// 是否八进制字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsOctchar(byte c)
        {
            return (c >= '0' && c <= '7');
        }
        /// <summary>
        /// 是否十进制字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsDecchar(byte c)
        {
            return (c >= '0' && c <= '9');
        }
        /// <summary>
        /// 对字符串中的特殊字符（如中文）进行转义处理，并将其转换为字节数组。该函数支持多种转义序列，包括十六进制、八进制和常见的转义字符（如 \n, \t, \r 等）。
        /// 通过这种方式，可以确保字符串在某些特定上下文中（如网络传输或文件存储）能够正确解析。
        /// </summary>
        /// <param name="Text">任意字符串</param>
        /// <returns></returns>
        private static byte[] Escape(string Text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Text);
            List<byte> result = new List<byte>();

            for (int i = 0; i < bytes.Length;)
            {
                if (bytes[i] != '\\')
                {
                    result.Add(bytes[i]);
                    i++;
                }
                else
                {
                    i++;
                    if (i < bytes.Length)
                    {
                        string s = "";
                        if (bytes[i] == 'x')
                        {
                            s = "";
                            i++;
                            int k = 0;
                            while (k < 2 && i + k < bytes.Length && IsHexchar(bytes[i + k])) k++;
                            for (int j = 0; j < k; j++) s += (char)bytes[i + j];
                            byte hexvalue = Convert.ToByte(s, 16);
                            result.Add(hexvalue);
                            i += k;
                        }
                        else if (bytes[i] == '0')
                        {
                            s = "";
                            i++;
                            int k = 0;
                            while (k < 3 && i + k < bytes.Length && IsOctchar(bytes[i + k])) k++;
                            //这里注意如果k等于3 并且转换后的值超过255 则应将k-1重新计算一次
                            for (int j = 0; j < k; j++) s += (char)bytes[i + j];

                            byte hexvalue;
                            try
                            {
                                hexvalue = Convert.ToByte(s, 8);
                            }
                            catch
                            {
                                s = "";
                                k--;
                                for (int j = 0; j < k; j++) s += (char)bytes[i + j];
                                if (s == "") hexvalue = 0;
                                else hexvalue = Convert.ToByte(s, 8);
                            }
                            result.Add(hexvalue);
                            i += k;
                        }
                        else if (char.IsDigit((char)bytes[i]))
                        {
                            s = "";
                            int k = 0;
                            while (k < 3 && i + k < bytes.Length && IsHexchar(bytes[i + k])) k++;
                            //这里注意如果k等于3 并且转换后的值超过255 则应将k-1重新计算一次
                            for (int j = 0; j < k; j++) s += (char)bytes[i + j];
                            byte hexvalue;
                            try
                            {
                                hexvalue = Convert.ToByte(s, 8);
                            }
                            catch
                            {
                                s = "";
                                k--;
                                for (int j = 0; j < k; j++) s += (char)bytes[i + j];
                                hexvalue = Convert.ToByte(s, 8);
                            }
                            result.Add(hexvalue);
                            i += k;
                        }
                        else
                        {

                            switch ((char)bytes[i])
                            {
                                case 'n':
                                    result.Add((byte)'\n');
                                    break;
                                case 't':
                                    result.Add((byte)'\t');
                                    break;
                                case 'r':
                                    result.Add((byte)'\r');
                                    break;
                                case '\\':
                                    result.Add((byte)'\\');
                                    break;
                                case '\'':
                                    result.Add((byte)'\'');
                                    break;
                                case '\"':
                                    result.Add((byte)'\"');
                                    break;
                                case 'b':
                                    result.Add((byte)'\b');
                                    break;
                                case 'f':
                                    result.Add((byte)'\f');
                                    break;
                                case 'v':
                                    result.Add((byte)'\v');
                                    break;
                                default:
                                    // 如果是未知的转义符，保留原样
                                    result.Add((byte)'\\');
                                    result.Add(bytes[i]);
                                    break;
                            }

                            i++;
                        }
                    }
                }
            }

            //if (bFirst) return Escape(Encoding.UTF8.GetString(result.ToArray()), false);
            return result.ToArray();
        }
    }
}

////使用Convert.ToString 转换为十六进制字符串，但不保证长度
//string hexString1 = Convert.ToString(number, 16);
//Console.WriteLine(hexString1); // 输出 "f"

////使用 ToString 和 PadLeft 来确保最小长度
//string hexString2 = number.ToString("X").PadLeft(desiredLength, '0');
//Console.WriteLine(hexString2); // 输出 "000f"
//stringBuilder.Append(@"\x" + ((int)c).ToString("x2"));

////转义序列和进制表示
//八进制识别3位，\0101和\101是同一个（前导0无关紧要），但\128 在ASCII码表中是超出范围的因为ASCII码只定义了0到127的字符；
//十六进制最多识别2位（不含前导），在大多数情况下，\x000f 和 \x0f是等价的，因为它们都表示相同的十六进制值 0F，对应ASCII码表中的“换行”（Line Feed, LF）字符（尽管在现代系统中，换行通常表示为 \n，即ASCII码10，而 \x0F 实际上是一个不太常用的控制字符，称为“换页”或“新页”（Form Feed, FF）），实际意义取决于它们被用在什么上下文中。
////算法混淆时的进制数值表示
//0x0f：0x前缀通常用于表示一个十六进制数值，而不是一个字符的转义序列，所以它表示十六进制数值0F，等于十进制的15；
//return "0X" + Convert.ToString(number, 16);//静态方法
//return "0X" + number.ToString("x");//实例方法不限长度，过亿数字设置x8则不足自动用0填充
//用number.ToString("x2")来尝试将一个很大的整数（如21亿）转换为十六进制字符串，并且指定了"x2"格式说明符，‌C#不会报错，‌但也不会给完整的十六进制表示只会返回该整数十六进制表示的前两位。
//星际2Galaxy代码里十六进制只允许\x11x12x13或\x11\x12\x13（不含前导0的话x后面最大跟俩位），八进制不含前导0的话后面最大三位
