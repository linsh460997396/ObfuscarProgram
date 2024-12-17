using System;
using System.Collections.Generic;
using System.Text;

namespace GalaxyObfuscator
{
    /// <summary>
    /// 标识符生成器
    /// </summary>
    internal class IdentifierGenerator
    {
        /// <summary>
        /// [构造函数]标识符生成器
        /// </summary>
        /// <param name="alphabet">包含所有可能用于生成标识符的字符</param>
        /// <param name="maxLength">生成标识符的最大长度</param>
        public IdentifierGenerator(string alphabet, int maxLength)
        {
            this.alphabet = alphabet;//设置可用字符集
            this.maxLength = maxLength;//设置最大长度
            this.builder = new StringBuilder(maxLength);//初始化StringBuilder，用于构建标识符字符数组
        }

        /// <summary>
        /// 生成唯一标识符
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            string text;
            do
            {
                //随机选择标识符的长度（1到maxLength之间）
                int num = this.random.Next(1, this.maxLength + 1);
                //清空StringBuilder
                this.builder.Clear();
                //随机选择字符集一个字符
                char c;
                do
                {
                    //如果字符集是3个字符，c得到字符alphabet[0~2]之一
                    c = this.alphabet[this.random.Next(this.alphabet.Length)];
                }
                while (char.IsDigit(c));//字符是十进制（数字）则继续随机，直到确保它不是数字
                //将这个字符添加到StringBuilder中（第一位数组元素必须是非数字开头）
                this.builder.Append(c);
                //生成剩余的字符
                for (int i = 1; i < num; i++)
                {
                    //获取字符集中任意随机字符
                    this.builder.Append(this.alphabet[this.random.Next(this.alphabet.Length)]);
                }
                //将生成的字符串转换为字符串文本
                text = this.builder.ToString();
            }
            //确保生成的标识符是唯一的
            while (this.names.Contains(text));
            //将生成的标识符添加到已使用的集合中
            this.names.Add(text);
            //返回生成的唯一标识符
            return text;
        }
        /// <summary>
        /// 已生成的标识符集合，用于确保唯一性
        /// </summary>
        private ISet<string> names = new HashSet<string>();
        /// <summary>
        /// 随机数生成器，用于生成随机字符和长度
        /// </summary>
        private Random random = new Random();
        /// <summary>
        /// 可用字符集，用于生成标识符
        /// </summary>
        private string alphabet;
        /// <summary>
        /// 标识符的最大长度
        /// </summary>
        private int maxLength;
        /// <summary>
        /// 用于构建标识符的StringBuilder
        /// </summary>
        private StringBuilder builder;
    }
}
