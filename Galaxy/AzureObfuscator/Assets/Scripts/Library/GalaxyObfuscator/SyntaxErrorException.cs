using MetalMaxSystem;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace GalaxyObfuscator
{
    /// <summary>
    /// 语法错误表达（自定义异常类）
    /// </summary>
    internal class SyntaxErrorException : Exception
    {
        static FileWriter tempFileWriter = new FileWriter();
        public string errPath = "C:/Users/linsh/Desktop/SyntaxErrorException.txt";

        /// <summary>
        /// [构造函数]语法错误表达（自定义异常类）
        /// </summary>
        public SyntaxErrorException()
        {
        }

        /// <summary>
        /// [构造函数]语法错误表达（自定义异常类），创建一个带有自定义错误消息的SyntaxErrorException实例。
        /// 调用基类（Exception）的构造函数base("Unexpected " + scanner.Current.Sequence.ToString())
        /// 传入一个表示意外字符序列（从scanner.Current.Sequence获取并转换为字符串）的消息，
        /// 同时在函数内部获取scanner.Current.Sequence并将其赋值给sequence
        /// </summary>
        /// <param name="scanner"></param>
        public SyntaxErrorException(Scanner scanner) : base("Unexpected " + scanner.Current.ToString())
        {
            //↓强力调试模式
            //tempFileWriter.Buffer.Append(MMCore.fileWriter.Buffer);
            //tempFileWriter.WriteLine("█意外终止 "+ "SyntaxErrorException Unexpected " + scanner.Current.ToString());
            //tempFileWriter.Close(errPath, true, Encoding.UTF8);//出现错误立即写入（性能差），防止在无限循环退出时不打印报告
        }
        /// <summary>
        /// [构造函数]语法错误表达（自定义异常类），调用基类（Exception）的构造函数base(message)进行处理。
        /// </summary>
        /// <param name="message">字符串消息</param>
        public SyntaxErrorException(string message) : base(message)
        {
        }
        /// <summary>
        /// [构造函数]语法错误表达（自定义异常类），受保护的构造函数，调用基类（Exception）的构造函数base(info, context)进行反序列化相关处理。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SyntaxErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        /// <summary>
        /// [构造函数]语法错误表达（自定义异常类），调用基类（Exception）的构造函数base(message, innerException)进行处理。
        /// </summary>
        /// <param name="message">字符串消息</param>
        /// <param name="innerException">内部异常</param>
        public SyntaxErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
