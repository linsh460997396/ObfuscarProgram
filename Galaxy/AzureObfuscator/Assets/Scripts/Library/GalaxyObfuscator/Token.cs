using System;
using System.Globalization;
using System.Text;

namespace GalaxyObfuscator
{
    /// <summary>
    /// 标记
    /// </summary>
    internal struct Token
    {
        /// <summary>
        /// 重写ToString方法，返回Sequence的字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Sequence.ToString();
        }
        /// <summary>
        /// 判断当前Token是否为字面量，如果Type是字符串字面量、字符字面量、整数字面量、实数字面量或十六进制字面量，则返回true
        /// </summary>
        public bool IsLiteral
        {
            get
            {
                return this.Type == TokenType.StringLiteral || this.Type == TokenType.CharLiteral || this.Type == TokenType.IntegerLiteral || this.Type == TokenType.RealLiteral || this.Type == TokenType.HexLiteral;
            }
        }
        /// <summary>
        /// 解析字面量并返回相应的对象
        /// </summary>
        /// <returns></returns>
        public object ParseLiteral()
        {
            switch (this.Type)
            {
                case TokenType.StringLiteral:
                    return this.ParseStringLiteral();
                case TokenType.CharLiteral:
                    return this.ParseCharLiteral();
                case TokenType.IntegerLiteral:
                case TokenType.HexLiteral:
                    return this.ParseIntegerLiteral();
                case TokenType.RealLiteral:
                    return this.ParseRealLiteral();
            }
            return null;
        }
        /// <summary>
        /// 解析字符字面量
        /// </summary>
        /// <returns></returns>
        public char ParseCharLiteral()
        {
            // 获取字符字面量中的第一个字符
            char c = this.Sequence[1];
            char c2 = c;
            if (c2 == '\'')
            {
                return '\0';// 如果字符是单引号，返回空字符 
            }
            if (c2 != '\\')
            {
                return c;// 如果字符不是转义字符，直接返回
            }
            return this.SpecialCharacter(this.Sequence[2]);// 处理转义字符
        }
        /// <summary>
        /// 解析字符串字面量
        /// </summary>
        /// <returns></returns>
        public string ParseStringLiteral()
        {
            // 创建StringBuilder对象，长度为Sequence长度减2 
            StringBuilder stringBuilder = new StringBuilder(this.Sequence.Length - 2);
            for (int i = 1; i < this.Sequence.Length - 1; i++)
            {
                if (this.Sequence[i] != '\\')
                {
                    stringBuilder.Append(this.Sequence[i]);// 如果当前字符不是转义字符，直接添加到StringBuilder中 
                }
                else
                {
                    stringBuilder.Append(this.SpecialCharacter(this.Sequence[++i]));// 处理转义字符
                }
            }
            return stringBuilder.ToString();// 返回解析后的字符串
        }
        /// <summary>
        /// 解析整数字面量 
        /// </summary>
        /// <returns></returns>
        public int ParseIntegerLiteral()
        {
            if (this.Type == TokenType.IntegerLiteral)
            {
                return int.Parse(this.Sequence.ToString());// 如果是整数字面量，直接解析为int
            }
            return Convert.ToInt32(this.Sequence.ToString(), 16);// 如果是十六进制字面量，解析为int 
        }
        /// <summary>
        /// 解析实数字面量
        /// </summary>
        /// <returns></returns>
        public double ParseRealLiteral()
        {
            return double.Parse(this.Sequence.ToString(), Token.Culture);// 解析实数字面量为double
        }
        /// <summary>
        /// 处理特殊字符（转义字符）
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public char SpecialCharacter(char ch)
        {
            if (ch == 'n')
            {
                return '\n';// 换行符 
            }
            if (ch != 't')
            {
                return ch;// 如果不是制表符，返回原字符
            }
            return '\t';// 制表符 
        }
        /// <summary>
        /// 定义静态的CultureInfo对象，用于解析实数字面量 
        /// </summary>
        private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-US");
        /// <summary>
        /// 定义Sequence属性，表示Token的序列
        /// </summary>
        public Sequence Sequence;
        /// <summary>
        /// 定义Type属性，表示Token的类型
        /// </summary>
        public TokenType Type;
    }
}
