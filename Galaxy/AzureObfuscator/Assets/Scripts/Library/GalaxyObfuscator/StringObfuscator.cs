using MetalMaxSystem;
using System;
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
        /// <param name="str">可以是夹在冒号间的原文</param>
        /// <param name="torf">true处理连续的两个\字符为1个\（即处理双\的转义），如需保留两个\或变成1个/或其他字符串时则应设置torf=false（默认）</param>
        /// <param name="torfString">torf=false时生效，当torfString不为null则连续2个\字符将被处理为自定义torfString（默认值"/"），只改torf=false而torfString=null时将保留双\</param>
        /// <returns></returns>
        public string Obfuscate(string str, bool torf = false, string torfString = "/")
        {
            if (!str.Contains("bnet:"))
            {
                string result;
                byte[] bytes = MMCore.Escape(str);//默认会将双\处理为"/"
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
