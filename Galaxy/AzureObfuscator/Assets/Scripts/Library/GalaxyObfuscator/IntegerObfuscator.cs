using System;
using System.Text;

namespace GalaxyObfuscator
{
    /// <summary>
    /// 内部混肴器
    /// </summary>
    internal class IntegerObfuscator
    {
        /// <summary>
        /// 简单混淆（用于加减法计算）。函数将数字转成八进制或十六进制表示。
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string obfuscateSimple(int number)
        {
            //根据随机数生成器的结果，选择不同的方式来表示整数
            switch (this.random.Next(7))
            {//随机出0~6，其中0~3均执行return "0" + Convert.ToString(number, 8);
                case 0:
                case 1:
                case 2:
                case 3:
                    //以八进制形式表示整数，并在前面加上'0'
                    return '0' + Convert.ToString(number, 8);//Galaxy中加不加0均可，但这里还是加一下
                case 4:
                    //以十六进制形式表示整数，并在前面加上'0x'
                    //return "0x" + Convert.ToString(number, 16);
                    return "0x" + number.ToString("x");
                case 5:
                    //以十六进制形式表示整数，并在前面加上'0X'
                    //return "0X" + Convert.ToString(number, 16);
                    return "0X" + number.ToString("x");
                default:
                    //直接返回整数的字符串表示 
                    return number.ToString();
            }
        }

        /// <summary>
        /// 生成混淆后的加减法表达式。函数将number的混淆结果以字符串的形式存储在StringBuilder对象参数中
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="number"></param>
        /// <param name="canBeFixed">设置三组范围值（num3, num4, num5）。如果canBeFixed为true则范围较小，反之范围较大。这些范围值用于后续生成随机数时限制其大小</param>
        private void obfuscateAddition(StringBuilder stringBuilder, int number, bool canBeFixed)
        {
            int num = 0;
            int num2 = this.random.Next(1, 8);//随机生成一个1到7之间的整数
            if (num2 >= 2)
            {
                num2 /= 2;//如果 num2大于等于2，则将其除以2 
            }
            //定义范围值，根据canBeFixed的值决定范围
            int num3;
            int num4;
            int num5;
            if (canBeFixed)
            {
                num3 = -524287;
                num4 = 524287;
                num5 = 262143;
            }
            else
            {
                num3 = -2147483548;
                num4 = 2147483547;
                num5 = 1073676288;
            }
            //循环num2次，生成加减法表达式
            for (int i = 0; i < num2; i++)
            {
                int num7;
                if (i < num2 - 1)
                {
                    //如不是最后一次循环，生成一个随机long类型数值num6，该值基于number和一个在-num5到num5范围内的随机数相加得到
                    long num6 = (long)number + (long)this.random.Next(-num5, num5);
                    //然后确保num6在num3和num4指定的范围内
                    num6 = Math.Max(num6, (long)num3);
                    num6 = Math.Min(num6, (long)num4);
                    num7 = (int)num6;
                }
                else
                {
                    //如果是最后一次循环直接使用原始的number值
                    num7 = number;
                }
                //计算当前数字num7与之前累计的结果num的差值并根据差值的正负决定是否在StringBuilder中添加'+'或'-'符号
                num = num7 - num;
                if (num < 0)
                {
                    stringBuilder.Append('-');
                    num = -num;
                }
                else if (i > 0)
                {
                    stringBuilder.Append('+');//如果不是第一次循环，添加加号
                }
                //处理差值num，结果添加到StringBuilder中
                stringBuilder.Append(this.obfuscateSimple(num));
                num = num7;//更新当前数字
            }
        }

        /// <summary>
        /// 混肴一个整数为（^）形式。
        /// </summary>
        /// <param name="number">用于混肴的整数</param>
        /// <returns>返回混淆后的字符串</returns>
        public string Obfuscate(int number)
        {
            if (number == 2147483647)
            {
                //如果输入的number是Int32的最大值（2147483647）则直接返回其十六进制表示形式"0x7fffffff"
                return "0x7fffffff";
            }
            //初始化StringBuilder对象用于构建混淆后的字符串
            StringBuilder stringBuilder = new StringBuilder();
            //判断number是否在一个较小的范围内（-524287 到 524287）并设置flag
            bool flag = -524287 <= number && number <= 524287;
            //在StringBuilder对象中追加一个左括号'('表示混淆表达式的开始
            stringBuilder.Append('(');
            //初始化num为0，它将用于存储异或操作的累积结果
            int num = 0;
            //随机生成一个1到6之间的整数num2表示将要进行的异或和加法操作的次数
            int num2 = this.random.Next(1, 7);
            //如果num2大于等于3则将其调整为(num2+1)/2确保操作次数在1到3之间
            if (num2 >= 3)
            {
                num2 = (num2 + 1) / 2;
            }
            //循环num2次，每次执行以下操作：
            for (int i = 0; i < num2; i++)
            {
                //如果不是第一次循环则在StringBuilder对象中追加'^'符号表示异或操作
                if (i > 0)
                {
                    stringBuilder.Append('^');
                }
                int num3;
                if (i < num2 - 1)
                {
                    //如果当前不是最后一次循环则生成一个随机数num3，根据flag的值决定随机数的范围
                    //如果flag为true则范围较小（0 到 524287）否则范围较大（0 到 2147418112）
                    num3 = this.random.Next(0, flag ? 524287 : 2147418112);
                }
                else
                {
                    //如果是最后一次循环则直接使用原始的number值作为num3
                    num3 = number;
                }
                //对num和num3执行异或操作并将结果存回num中
                num ^= num3;
                //调用obfuscateAddition方法将异或操作的结果以加减法表达式的形式追加到StringBuilder对象中
                //传参包括StringBuilder对象、当前的num值及flag标志
                this.obfuscateAddition(stringBuilder, num, flag);
                //更新num值为当前的num3以便下一次循环中使用
                num = num3;
            }
            //在StringBuilder对象中追加一个右括号')'表示混淆表达式的结束
            stringBuilder.Append(')');
            //返回StringBuilder对象中构建的混淆后的字符串
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 固定范围的最小值
        /// </summary>
        private const int MinFixed = -524287;
        /// <summary>
        /// 固定范围的最大值
        /// </summary>
        private const int MaxFixed = 524287;
        /// <summary>
        /// 随机数生成器
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
