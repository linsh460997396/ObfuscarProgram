using MetalMaxSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GalaxyObfuscator
{
    /// <summary>
    /// 扫描器。用于解析输入字符串并生成标记
    /// </summary>
    internal class Scanner : IEnumerator<Token>, IDisposable, IEnumerator
    {
        /// <summary>
        /// [构造函数]扫描器。用于解析输入字符串并生成标记，初始token.Type = TokenType.None，Start = 0，End = 0
        /// </summary>
        /// <param name="str"></param>
        public Scanner(string str)
        {
            this.token.Sequence = new Sequence(str, 0, 0);
            this.token.Type = TokenType.None;
        }
        /// <summary>
        /// 实现释放资源接口（暂无动作）
        /// </summary>
        public void Dispose()
        {
        }
        /// <summary>
        /// 重置扫描器状态。Sequence.Start与Sequence.End均置0，token.Type = TokenType.None。
        /// </summary>
        public void Reset()
        {
            this.token.Sequence.Start = (this.token.Sequence.End = 0);
            this.token.Type = TokenType.None;
        }
        /// <summary>
        /// 判断是否到达当前标记的序列（字符串）末尾
        /// </summary>
        public bool End
        {
            get
            {
                //当前扫描位置的索引达到序列字符串长度则返回真，反之返回假
                return this.position >= this.length;
            }
        }
        /// <summary>
        /// 获取当前标记
        /// </summary>
        public Token Current
        {
            get
            {
                return this.token;
            }
        }
        /// <summary>
        /// 实现IEnumerator接口的Current属性，获取当前标记
        /// </summary>
        object IEnumerator.Current
        {
            get
            {
                return this.token;
            }
        }
        /// <summary>
        /// 指针非末尾时执行读标记。具体流程：
        /// 读空白字符和注释（是空格则前进，当字符不为斜杠或下一扫描位置 >= 长度或下一扫描字符不为斜杠和 * 时函数退出，否则说明是注释，开始读注释，
        /// 最后设置当前标记类型为无并将Sequence.Start = Sequence.End），如果当前扫描位置是结尾字符则直接返回，反之下一步，当前位置的字符是：1）字母或下划线时执行读标识符；
        /// 2）十进制数或.且下一位置不是字符串末尾且下一位置也是十进制数则执行读数字字面量；3）单引号或双引号则执行读文本字面量；4）其他情况则读符号。
        /// </summary>
        /// <returns>读标记成功返回真，否则返回假</returns>
        public bool MoveNext()
        {
            if (this.End)
            {//指针在结尾返回假
                return false;
            }
            //读标记
            this.readToken();
            //读标记成功返回真
            return true;
        }
        /// <summary>
        /// 读当前标记并返回（扫描指针将前进）
        /// </summary>
        /// <returns></returns>
        public Token Read()
        {
            this.readToken();
            return this.token;
        }
        /// <summary>
        /// 如果不是当前标记的序列（字符串）结尾则读取标记并移动到下一个，如果是结尾则抛出异常
        /// </summary>
        /// <returns>返回当前标记</returns>
        /// <exception cref="UnexpectedEndOfFileException"></exception>
        public Token ReadExpectedToken()
        {
            if (!this.MoveNext())
            {
                MMCore.WriteLine("MoveNext失败！" + $"Type: {token.Type.ToString()}, Value: {token.Sequence.ToString()}");
                throw new UnexpectedEndOfFileException();
            }
            return this.token;
        }
        /// <summary>
        /// 读当前标记，如果标记类型不是符号或预期标记的序列（字符串）不等于参数symbol时抛出异常
        /// </summary>
        /// <param name="symbol">预期标记的序列字符串</param>
        /// <exception cref="SyntaxErrorException">如果标记类型不是符号或当前标记的序列（字符串）不等于参数symbol时抛出异常</exception>
        public void ReadExpectedSymbol(string symbol)
        {
            this.Read();
            if (this.token.Type != TokenType.Symbol || this.token.Sequence != symbol)
            {
                MMCore.WriteLine("标记类型不是符号或当前标记的序列（字符串）不等于参数" + symbol + "时抛出异常！" + $"Type: {token.Type.ToString()}, Value: {token.Sequence.ToString()}");
                throw new SyntaxErrorException("Expected " + symbol);
            }
        }
        /// <summary>
        /// 读当前标记并期望特定的标记类型为参数type，否则抛出异常
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="SyntaxErrorException"></exception>
        public void ReadExpectedToken(TokenType type)
        {
            this.Read();
            if (this.token.Type != type)
            {
                MMCore.WriteLine("读当前标记并期望特定的标记类型为参数" + token + "，否则抛出异常！" + $"Type: {token.Type.ToString()}, Value: {token.Sequence.ToString()}");
                throw new SyntaxErrorException("Unexpected " + this.token);
            }
            else
            {
                MMCore.WriteLine("得到预期标记类型！" + $"Type: {type.ToString()}, Value: {token.ToString()}");
            }
        }
        /// <summary>
        /// 跳过直到遇到指定的终止符，如果到当前标记的序列（字符串）末尾还没有遇到终止符则抛出异常
        /// </summary>
        /// <param name="terminate"></param>
        /// <exception cref="SyntaxErrorException"></exception>
        public void SkipBlock(string terminate)
        {
            while (!this.End)
            {
                if (this.Read().Type == TokenType.Symbol && this.token.Sequence == terminate)
                {
                    return;
                }
            }
            MMCore.WriteLine("跳过直到遇到指定的终止符，如果到当前标记的序列（字符串）末尾还没有遇到终止符" + terminate + "，则抛出异常！" + $"Type: {this.Current.Type.ToString()}, Value: {this.Current.Sequence.ToString()}");
            throw new SyntaxErrorException("Missing " + terminate);
        }
        /// <summary>
        /// 跳过直到遇到指定的任意终止符，如果到当前标记的序列（字符串）末尾还没有遇到终止符则抛出异常
        /// </summary>
        /// <param name="terminate1">终止符1</param>
        /// <param name="terminate2">终止符2</param>
        /// <exception cref="SyntaxErrorException"></exception>
        public void SkipBlockPro(string terminate1, string terminate2)
        {
            while (!this.End)
            {
                if (this.Read().Type == TokenType.Symbol && (this.token.Sequence == terminate1 || this.token.Sequence == terminate2))
                {
                    return;
                }
            }
            MMCore.WriteLine("跳过直到遇到指定的任意终止符，如到当前标记序列末尾还没遇到终止符：" + terminate1 + "，或：" + terminate2 + "，则抛出异常！" + $"Type: {this.Current.Type.ToString()}, Value: {this.Current.Sequence.ToString()}");
            throw new SyntaxErrorException("Missing " + terminate1 + "或" + terminate2);
        }
        /// <summary>
        /// 跳过直到遇到指定的任意终止符，如果到当前标记的序列（字符串）末尾还没有遇到终止符则抛出异常
        /// </summary>
        /// <param name="terminate1"></param>
        /// <param name="terminate2"></param>
        /// <param name="terminate3"></param>
        /// <exception cref="SyntaxErrorException"></exception>
        public void SkipBlockPro(string terminate1, string terminate2, string terminate3)
        {
            while (!this.End)
            {
                if (this.Read().Type == TokenType.Symbol && (this.token.Sequence == terminate1 || this.token.Sequence == terminate2 || this.token.Sequence == terminate3))
                {
                    return;
                }
            }
            MMCore.WriteLine("跳过直到遇到指定的任意终止符，如到当前标记序列末尾还没遇到终止符：" + terminate1 + "，或：" + terminate2 + "，或：" + terminate3 + "，则抛出异常！" + $"Type: {this.Current.Type.ToString()}, Value: {this.Current.Sequence.ToString()}");
            throw new SyntaxErrorException("Missing " + terminate1 + "或" + terminate2 + "或" + terminate3);
        }
        /// <summary>
        /// 跳过嵌套的代码块直到遇到指定的结束符，如果到当前标记的序列（字符串）末尾还没遇到有效的结束符则抛出异常
        /// </summary>
        /// <param name="o">字符串中期望的代码块标记类型符号头，可以不存在</param>
        /// <param name="e">结束符，如果不存在会抛出异常</param>
        /// <exception cref="SyntaxErrorException"></exception>
        public void SkipBlock(string o, string e)
        {
            int num = 1;
            while (!this.End)
            {
                if (this.Read().Type == TokenType.Symbol)
                {
                    if (this.token.Sequence == o)
                    {
                        num++;
                    }
                    else if (this.token.Sequence == e && --num == 0)
                    {
                        return;
                    }
                }
            }
            MMCore.WriteLine("跳过嵌套的代码块直到遇到指定的结束符，如果到当前标记的序列（字符串）末尾还没遇到有效的结束符" + e + "，则抛出异常！" + $"Type: {this.Current.Type.ToString()}, Value: {this.Current.Sequence.ToString()}");
            throw new SyntaxErrorException("Missing " + e);
        }
        /// <summary>
        /// 当前扫描位置（1=第一个元素，0=未开始扫描）。当前标记的序列（字符串）的尾部指针索引（this.token.Sequence.End）
        /// </summary>
        public int position
        {
            get
            {
                return this.token.Sequence.End;
            }
            set
            {
                this.token.Sequence.End = value;
            }
        }
        /// <summary>
        /// 当前标记的序列字符串长度。this.token.Sequence.String.Length
        /// </summary>
        private int length
        {
            get
            {
                return this.token.Sequence.String.Length;
            }
        }
        /// <summary>
        ///  获取当前标记的序列字符串索引器[当前扫描位置]的Char字符
        /// </summary>
        /// <returns></returns>
        private char get()
        {
            return this.token.Sequence.String[this.position];
        }
        /// <summary>
        /// 读当前标记的序列字符串索引器[当前扫描位置]的Char字符并将尾部指针移动到下一个字符
        /// </summary>
        /// <returns></returns>
        private char read()
        {
            string @string = this.token.Sequence.String;
            int end;
            this.token.Sequence.End = (end = this.token.Sequence.End) + 1;
            return @string[end];
        }
        /// <summary>
        /// 前进，将尾部指针移动到下一个字符
        /// </summary>
        /// <returns>下一字符非字符串末尾则返回真，反之为假</returns>
        private bool forward()
        {
            return (this.token.Sequence.End = this.token.Sequence.End + 1) < this.length;
        }
        /// <summary>
        /// 设置当前标记类型为无并将Sequence.Start = Sequence.End
        /// </summary>
        private void setNil()
        {
            this.token.Sequence.Start = this.token.Sequence.End;
            this.token.Type = TokenType.None;
        }
        /// <summary>
        /// [索引器]当前标记的序列字符串索引器[当前扫描位置]的Char字符
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private char this[int index]
        {
            get
            {
                return this.token.Sequence.String[index];
            }
        }
        /// <summary>
        /// 读标记。读空白字符和注释（是空格则前进，当字符不为斜杠或下一扫描位置 >= 长度或下一扫描字符不为斜杠和 * 时函数退出，否则说明是注释，开始读注释，
        /// 最后设置当前标记类型为无并将Sequence.Start = Sequence.End），如果当前扫描位置是结尾字符则直接返回，反之下一步，当前位置的字符是：1）字母或下划线时执行读标识符；
        /// 2）十进制数或.且下一位置不是字符串末尾且下一位置也是十进制数则执行读数字字面量；3）单引号或双引号则执行读文本字面量；4）其他情况则读符号。
        /// 注：每次读标记只能在代码全文里读一种直到遇到结束符，作为一次标记。
        /// </summary>
        private void readToken()
        {
            //读空白字符和注释
            this.readWhite();
            if (this.End)
            {
                return;
            }
            char c = this.get();
            if (char.IsLetter(c) || c == '_')
            {//如果当前位置的字符是字母或下划线
                this.readIdentifier();//读取标识符
                return;
            }
            if (char.IsDigit(c) || (c == '.' && this.position + 1 < this.length && char.IsDigit(this[this.position + 1])))
            {//如果当前位置的字符是十进制数或（.且下一位置不是字符串末尾且下一位置也是十进制数说明是浮点数）
                this.readNumberLiteral();//读取数字字面量
                return;
            }
            if (c == '\'' || c == '"')
            {
                //单引号或双引号则执行读取文本字面量
                this.readTextLiteral();
                return;
            }
            //读取符号
            this.readSymbol();
        }
        /// <summary>
        /// 读空白字符和注释。
        /// 是空格则前进，当字符不为斜杠或下一扫描位置 >= 长度或下一扫描字符不为斜杠和 * 时函数退出，否则说明是注释，开始读注释，
        /// 最后设置当前标记类型为无并将Sequence.Start = Sequence.End
        /// </summary>
        private void readWhite()
        {
            while (!this.End)
            {//如果不是序列末尾
                if (char.IsWhiteSpace(this.get()))
                {
                    //是空格则前进
                    this.forward();
                }
                else
                {
                    if (this.get() != '/' || this.position + 1 >= this.length || (this[this.position + 1] != '/' && this[this.position + 1] != '*'))
                    {//当前标记的序列字符串索引器[当前扫描位置]的Char字符不为斜杠或下一扫描位置>=长度或（下一扫描字符不为斜杠和*）
                        return;
                    }
                    //条件通过说明是注释，开始读注释
                    this.readComment();
                }
            }
            //设置当前标记类型为无并将Sequence.Start = Sequence.End
            this.setNil();
        }
        /// <summary>
        /// 读注释
        /// </summary>
        /// <exception cref="SyntaxErrorException"></exception>
        private void readComment()
        {
            //前进
            this.forward();
            if (this.read() == '*')
            {//当前字符是*说明是多行注释，这里指针继续前进
                while (this.position < this.length - 1)
                {//一直读到*/，即多行注释结束
                    if (this.read() == '*' && this.get() == '/')
                    {
                        //指针移动到/字符后面一位
                        this.forward();
                        return;
                    }
                }
                //全部读完还没遇到*/结尾，则报错
                MMCore.WriteLine("全部读完还没遇到*/结尾则抛出异常！" + $"Type: {this.Current.Type.ToString()}, Value: {this.Current.ToString()}");
                throw new SyntaxErrorException("End of file in comment");
            }
            while (!this.End)
            {//指针不在字符串末尾情况
                char c = this.read();
                if (c == '\n')
                {//当前字符是换行符则直接返回
                    return;
                }
                if (c == '\\')
                {//当前字符是反斜杠，说明是单行注释的头，继续前进直到遇到换行符
                    if (!this.End)
                    {//非末尾字符
                        //前进
                        this.forward();
                    }
                }
            }
        }
        /// <summary>
        /// 读标识符
        /// </summary>
        private void readIdentifier()
        {
            //标记类型是标识符
            this.token.Type = TokenType.Identifier;
            //将当前指针位置作为序列开始
            this.token.Sequence.Start = this.position;
            while (this.forward())
            {//前进并判断当前字符是否非十进制数且不为_，直到下一字符是结尾
                if (!char.IsLetterOrDigit(this.get()) && this.get() != '_')
                {
                    return;
                }
            }
        }
        /// <summary>
        /// 检查是否为十六进制字符
        /// </summary>
        /// <param name="ch"></param>
        /// <returns>如果是十进制数或"abcdefABCDEF"组成则返回真</returns>
        private static bool IsHexDigit(char ch)
        {
            return char.IsDigit(ch) || "abcdefABCDEF".Contains(ch);
        }
        /// <summary>
        /// 读数字字面量
        /// </summary>
        private void readNumberLiteral()
        {
            //序列Start从当前指针位置
            this.token.Sequence.Start = this.position;
            Func<char, bool> func;//创建临时函数引用（委托）类型变量
            if (this.get() == '0' && this.position + 2 < this.length && this[this.position + 1] == 'x')
            {//当前字符是0且后移2位也没到字符串结尾且0后面紧跟x字符（十六位进制使用了X2X3没有X1）
                this.position += 2;//指针后移2位
                this.token.Type = TokenType.HexLiteral;//标记类型设置为十六进制字面量
                func = new Func<char, bool>(Scanner.IsHexDigit);//给这个临时委托类型变量挂上函数引用，用以检查是否为十六进制字符
            }
            else
            {
                //否则是正常整数字面量
                this.token.Type = TokenType.IntegerLiteral;
                //给这个临时委托类型变量挂上函数引用，用以检查是否为十进制字符
                func = new Func<char, bool>(char.IsDigit);
            }
            for (; ; )
            {//无限循环
                if (this.get() == '.' && this.token.Type == TokenType.IntegerLiteral)
                {//当前字符是.切标记类型是整数字面量，将标记类型改为实数字面量
                    this.token.Type = TokenType.RealLiteral;
                }
                else if (!func(this.get()))
                {//委托检查当前字符不符合上述十六进制或十进制字符的，打断循环并结束函数
                    break;
                }
                if (!this.forward())
                {//下一个字符不是末尾则下一个，是末尾则打断循环并结束函数
                    return;
                }
            }
        }
        /// <summary>
        /// 读文本字面量
        /// </summary>
        /// <exception cref="SyntaxErrorException"></exception>
        /// <exception cref="UnexpectedEndOfFileException"></exception>
        private void readTextLiteral()
        {
            this.token.Sequence.Start = this.position;
            char c = this.read();//读取当前字符并前进指针
            //当前字符是冒号开头则将标记类型设为字符串字面量，否则设为字符字面量
            this.token.Type = ((c == '"') ? TokenType.StringLiteral : TokenType.CharLiteral);
            while (!this.End)
            {
                //读取当前字符并前进指针
                char c2 = this.read();
                if (c2 == c)
                {//当前字符与序列字符串的第一个字符相同则返回
                    return;
                }
                if (c2 == '\\' && !this.End)
                {//当前字符是反斜杠且为末尾字符
                    //继续前进
                    this.forward();
                }
                else if (c2 == '\n')
                {//否则当前字符是换行符，弹出错误提示“常量中存在新行”
                    MMCore.WriteLine("当前字符期望是反斜杠且为末尾字符但是换行符，抛出“常量中存在新行”" + $"Type: {this.Current.Type.ToString()}, Value: {this.Current.Sequence.ToString()}");
                    throw new SyntaxErrorException("New line in constant");
                }
            }
            //没有正常返回时执行错误提示
            MMCore.WriteLine("读文本字面量没有正常返回，抛出！" + $"Type: {this.Current.Type.ToString()}, Value: {this.Current.Sequence.ToString()}");
            throw new UnexpectedEndOfFileException();
        }
        /// <summary>
        /// 读符号
        /// </summary>
        private void readSymbol()
        {
            this.token.Sequence.Start = this.position;
            this.token.Type = TokenType.Symbol;
            //读取当前字符并将扫描指针前进1字符
            char c = this.read();
            if (!this.End)
            {//如果第二字符非末尾
                //获取当前字符
                char c2 = this.get();
                if (c2 <= '-')
                {//'-' 的ASCII值是45，检查当前字符的ASCII值是否在0~45之间
                    if (c2 != '&')
                    {//当前字符不是&
                        switch (c2)
                        {//当前字符是=或-都打断，英文逗号或其他情况直接退出函数
                            case '+':
                            case '-':
                                break;
                            case ',':
                                return;
                            default:
                                return;
                        }
                    }
                }
                else
                {//当前字符的ASCII值>45
                    switch (c2)
                    {
                        case '<':
                            break;
                        case '=':
                            //当前字符是<或=的情况
                            if ("+-*/%&^|=!<>".Contains(c))
                            {//检查第一个字符是否在"+-*/%&^|=!<>"中
                                //前进
                                this.forward();
                                return;
                            }
                            return;
                        case '>':
                            //当前字符是>的情况
                            if (c == '-')
                            {
                                //前进
                                this.forward();
                            }
                            break;
                        default:
                            //其他情况
                            if (c2 != '|')
                            {//当前字符是|，直接函数返回
                                return;
                            }
                            //否则跳至标签IL_8B
                            goto IL_8B;
                    }
                    //如果当前字符与第一个字符相同且下一字符是=且非末尾字符时
                    if (this.get() == c && this.forward() && this.get() == '=')
                    {
                        //前进
                        this.forward();
                        return;
                    }
                    return;
                }
            IL_8B:
                //当前字符与第一个字符相同
                if (this.get() == c)
                {
                    //前进
                    this.forward();
                    return;
                }
            }
        }
        /// <summary>
        /// 标记
        /// </summary>
        private Token token;
    }
}
