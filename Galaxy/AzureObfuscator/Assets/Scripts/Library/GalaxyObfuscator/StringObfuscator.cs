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
        /// 混淆处理
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Obfuscate(string str)
        {
            //创建一个StringBuilder对象，初始容量为输入字符串长度的5倍加2，这是因为每个字符可能会被转换成多个字符，需要控制最多五个字符加上开头和结尾的双引号
            StringBuilder stringBuilder = new StringBuilder(str.Length * 5 + 2);
            //添加第一个引号（Char）
            stringBuilder.Append('"');
            //字符遍历和混淆处理
            foreach (char c in str)
            {//遍历输入字符串的每一个字符
                //生成一个随机数num范围是0到4
                int num = this.random.Next(5);
                //在StringBuilder中添加一个反斜杠，用于转义后续的八、十六进制字符
                stringBuilder.Append("\\");
                //根据随机数num的值选择不同的混淆方式
                switch (num)
                {
                    case 0:
                    case 1:
                    case 2:
                        //case 0, 1, 2:将字符c转为八进制表示并添加到StringBuilder中
                        stringBuilder.Append(Convert.ToString((int)c, 8));
                        break;
                    case 3:
                        {
                            //将字符c转换为两位的十六进制表示并添加到StringBuilder中
                            stringBuilder.Append("x");
                            StringBuilder stringBuilder2 = stringBuilder;
                            int num2 = (int)c;
                            stringBuilder2.Append(num2.ToString("x2"));
                            break;
                        }
                    case 4:
                        {
                            //将字符c转换为三位的十六进制表示并添加到StringBuilder中
                            stringBuilder.Append("x");
                            StringBuilder stringBuilder3 = stringBuilder;
                            int num3 = (int)c;
                            stringBuilder3.Append(num3.ToString("x3"));
                            break;
                        }
                }
            }
            //在StringBuilder中添加最后一个引号，作为字符串的结尾
            stringBuilder.Append('"');
            //将StringBuilder中的内容转换为字符串并返回
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 用于生成随机数
        /// </summary>
        private Random random = new Random();
    }
}
