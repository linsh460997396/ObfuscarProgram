namespace GalaxyObfuscator
{
    /// <summary>
    /// 标记类型
    /// </summary>
    internal enum TokenType
    {
        /// <summary>
        /// 表示没有任何类型的标记
        /// </summary>
        None,
        /// <summary>
        /// 表示标识符（如关键字、结构体名、变量名、函数名等）
        /// </summary>
        Identifier,
        /// <summary>
        /// 表示字符串字面量（如 "Hello, World!"）
        /// </summary>
        StringLiteral,
        /// <summary>
        /// 表示字符字面量（如 'A'）
        /// </summary>
        CharLiteral,
        /// <summary>
        /// 表示整数字面量（如 123）
        /// </summary>
        IntegerLiteral,
        /// <summary>
        /// 表示实数字面量（如 123.45）
        /// </summary>
        RealLiteral,
        /// <summary>
        /// 表示符号（如 +, -, *, / 等）
        /// </summary>
        Symbol,
        /// <summary>
        /// 表示十六进制字面量（如 0x1A）
        /// </summary>
        HexLiteral
    }
}
