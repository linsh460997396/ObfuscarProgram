using System.Runtime.Serialization;//包含了序列化和反序列化相关的类和接口

namespace GalaxyObfuscator
{
    /// <summary>
    /// 文件未正常结束异常表达
    /// </summary>
    internal class UnexpectedEndOfFileException : SyntaxErrorException
    {
        /// <summary>
        /// [构造函数]文件未正常结束异常表达。调用基类构造函数base("Unexpected end of file")进行处理。
        /// </summary>
        public UnexpectedEndOfFileException() : base("Unexpected end of file")
        {
        }
        /// <summary>
        /// [构造函数]文件未正常结束异常表达。调用基类构造函数base(info, context)进行处理。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UnexpectedEndOfFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
