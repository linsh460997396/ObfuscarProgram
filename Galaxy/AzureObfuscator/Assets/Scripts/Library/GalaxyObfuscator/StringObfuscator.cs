using MetalMaxSystem;
using System;
using System.Text;

namespace GalaxyObfuscator
{
    /// <summary>
    /// 字符串混淆器
    /// </summary>
    internal class StringObfuscator
    {
        /// <summary>
        /// 混淆处理（将字符串转义序列）。字符串中每个字符将被转换为八进制或十六进制（X2）表示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Obfuscate(string str)
        {
            if (!str.Contains("bnet:"))
            {
                string temp; ushort languageIndex = 0;
                //创建一个StringBuilder对象，初始容量为输入字符串长度的5倍加2，这是因为每个字符可能会被转换成多个字符，需要控制最多五个字符加上开头和结尾的双引号
                StringBuilder stringBuilder = new StringBuilder(str.Length * 5 + 2);
                stringBuilder.Append('"');//添加第一个引号（Char）
                                          //字符遍历和混淆处理
                foreach (char c in str)
                {//遍历输入字符串的每一个字符

                    //这里有个问题，字符串字面量“准备”，混淆后是"\x51c6\x5907"，实际应是"\x51\xc6\x59\x07"或"\x51xc6\x59x07"的格式
                    //"\x51\xc6\x59\x07"或"\x51xc6\x59x07"是相同字节序列，转义出来的字符也是相同的，但八进制需要每个都加斜杠
                    //这里的foreach需要再检测判断字符是否是中文字符
                    if (MMCore.IsChineseCharacter(c))
                    {
                        //Console.WriteLine($"{c} 是中文字符或标点符号");
                        languageIndex = 1;
                    }
                    // 判断字符是否是基本的ASCII英文字符
                    else if (MMCore.IsEnglishCharacter(c))
                    {
                        //Console.WriteLine($"{c} 是英文字符");
                        //languageIndex不变
                    }
                    else
                    {
                        //Console.WriteLine($"{c} 是其他字符");
                        if (MMCore.IsChinesePunctuation(c))
                        {
                            //Console.WriteLine($"{c} 是中文标点符号");
                            languageIndex = 1;
                        }
                        else if (MMCore.IsEnglishPunctuation(c))
                        {
                            //Console.WriteLine($"{c} 是英文标点符号");
                            //languageIndex不变
                        }
                        else
                        {
                            //Console.WriteLine($"{c} 其他情况同中文，按字节转义");
                            languageIndex = 1;
                        }
                    }

                    //生成一个随机数num范围是0到4
                    int num = this.random.Next(5);
                    //stringBuilder.Append('\\');//在StringBuilder中添加一个反斜杠，用于转义后续的八、十六进制字符
                    //根据随机数num的值选择不同的混淆方式
                    switch (num)
                    {
                        case 0:
                        case 1:
                        case 2:
                            //case 0, 1, 2:将字符c转为八进制表示并添加到StringBuilder中
                            if (languageIndex == 1)
                            {
                                //将字符转换为字节数组（UTF-16编码，两个字节）
                                byte[] bytes = BitConverter.GetBytes(c);
                                //遍历字节数组，将每个字节单独转换为八进制
                                foreach (byte b in bytes)
                                {
                                    //模拟"\0101\0102"格式
                                    if ((temp = Convert.ToString(b, 8)) != "0")
                                    {
                                        stringBuilder.Append("\\0" + temp);//Galaxy中加不加0均可，但这里还是加一下
                                    }
                                    //else if (bytes.Length == 2)
                                    //{//如果第一个字节是空字节且bytes长度为2，直接转义字符并退出
                                    //    stringBuilder.Append("\\0" + Convert.ToString(c, 8));
                                    //    break;
                                    //}
                                }
                            }
                            else
                            {//其他情况
                                temp = Convert.ToString(c, 8);
                                if (temp.Length > 3)
                                {//进行拆解
                                    //将字符转换为字节数组（UTF-16编码，两个字节）
                                    byte[] bytes = BitConverter.GetBytes(c);
                                    //遍历字节数组，将每个字节单独转换为八进制
                                    foreach (byte b in bytes)
                                    {
                                        //模拟"\0101\0102"格式
                                        if ((temp = Convert.ToString(b, 8)) != "0")
                                        {
                                            stringBuilder.Append("\\0" + temp);
                                        }
                                    }
                                    break;
                                }
                                stringBuilder.Append("\\0" + temp);
                            }
                            break;
                        case 3:
                        case 4:
                            //将字符c转换为三位的十六进制表示并添加到StringBuilder中
                            //stringBuilder.Append("x" + ((int)c).ToString("x3")); //不允许3位，且直接转换中文也有问题，改为下面的方式
                            if (languageIndex == 1)
                            {

                                //将字符转换为字节数组（UTF-16编码，两个字节）
                                byte[] bytes = BitConverter.GetBytes(c);

                                stringBuilder.Append('\\');//在StringBuilder中添加一个反斜杠
                                                           //遍历字节数组，将每个字节单独转换为十六进制
                                foreach (byte b in bytes)
                                {
                                    //模拟"\x51xc6\x59x07"格式
                                    if ((temp = Convert.ToString(b, 16)) != "0")
                                    {
                                        stringBuilder.Append('x').Append(b.ToString("x2"));//十六进制最多识别2位
                                    }
                                    //else if (bytes.Length == 2)
                                    //{//如果第一个字节是空字节且bytes长度为2，直接转义字符并退出
                                    //    stringBuilder.Append('x' + ((int)c).ToString("x2"));
                                    //    break;
                                    //}

                                    //模拟"\x51\xc6\x59\x07"格式
                                    //stringBuilder.Append('\\').Append('x').Append(b.ToString("x2"));
                                }
                            }
                            else
                            {//其他情况
                                temp = ((int)c).ToString("x2");
                                if (temp.Length > 2)
                                {//进行拆解
                                    //将字符转换为字节数组（UTF-16编码，两个字节）
                                    byte[] bytes = BitConverter.GetBytes(c);
                                    stringBuilder.Append('\\');//在StringBuilder中添加一个反斜杠
                                    //遍历字节数组，将每个字节单独转换为八进制
                                    foreach (byte b in bytes)
                                    {
                                        //模拟"\0101\0102"格式
                                        if ((temp = Convert.ToString(b, 16)) != "0")
                                        {
                                            stringBuilder.Append('x').Append(b.ToString("x2"));//十六进制最多识别2位
                                        }
                                    }
                                    break;
                                }
                                stringBuilder.Append("\\x" + ((int)c).ToString("x2"));
                            }
                            break;
                    }
                }
                //在StringBuilder中添加最后一个引号作为字符串的结尾
                stringBuilder.Append('"');
                //将StringBuilder中的内容转换为字符串并返回
                temp = stringBuilder.ToString();
                //MMCore.WriteLine(str + " 转义序列 => " + temp);
                return temp;
            }
            else
            {//不转义含bnet:的字符串，直接返回
                return str;
            }
        }
        /// <summary>
        /// 用于生成随机数
        /// </summary>
        private Random random = new Random();
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
