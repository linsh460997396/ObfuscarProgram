using System;

namespace GalaxyObfuscator
{
    /// <summary>
    /// 序列
    /// </summary>
    internal struct Sequence : IEquatable<Sequence>, IEquatable<string>
    {
        /// <summary>
        /// [构造函数]序列。Start = 0，End = str.Length。
        /// </summary>
        /// <param name="str">代码字符串</param>
        public Sequence(string str)
        {
            this.String = str;
            this.Start = 0;
            this.End = str.Length;
        }
        /// <summary>
        /// [构造函数]序列
        /// </summary>
        /// <param name="str">代码字符串</param>
        /// <param name="start">序列的起始位置</param>
        /// <param name="end">序列的结束位置</param>
        public Sequence(string str, int start, int end)
        {
            this.String = str;
            this.End = Math.Min(Math.Max(end, 0), str.Length);// 确保 end 在有效范围内
            this.Start = Math.Min(Math.Max(start, 0), this.End);// 确保 start 在有效范围内
        }
        /// <summary>
        /// [构造函数]使用序列的指定范围创建一个新的序列
        /// </summary>
        /// <param name="seq">Sequence对象</param>
        /// <param name="start">序列的起始位置</param>
        /// <param name="end">序列的结束位置</param>
        public Sequence(Sequence seq, int start, int end)
        {
            this = new Sequence(seq.String, start, end);
        }
        /// <summary>
        /// 重写ToString方法，返回当前Sequence对象的子字符串(this.Start, this.End - this.Start)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.String.Substring(this.Start, this.End - this.Start);
        }
        /// <summary>
        /// 获取当前 Sequence 对象的长度（this.End - this.Start）
        /// </summary>
        public int Length
        {
            get
            {
                return this.End - this.Start;
            }
        }
        /// <summary>
        /// [索引器]获取当前 Sequence 对象中指定位置字符（String[this.Start + index]）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public char this[int index]
        {
            get
            {
                return this.String[this.Start + index];
            }
        }
        /// <summary>
        /// 比较两个 Sequence 对象是否相等
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public bool Equals(Sequence seq)
        {
            if (this.Length != seq.Length)
            {
                return false;
            }
            for (int i = 0; i < this.Length; i++)
            {
                if (this[i] != seq[i])
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 比较当前 Sequence 对象与一个字符串是否相等 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool Equals(string str)
        {
            if (this.Length != str.Length)
            {
                return false;
            }
            for (int i = 0; i < this.Length; i++)
            {
                if (this[i] != str[i])
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 重写 Equals 方法，支持与任意对象的比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is string)
            {
                return this.Equals((string)obj);
            }
            return obj is Sequence && this.Equals((Sequence)obj);
        }
        /// <summary>
        /// 重写 GetHashCode 方法，生成当前 Sequence 对象的哈希码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int num = 0;
            for (int i = this.Start; i < this.End; i++)
            {
                num ^= this.String[i].GetHashCode();
            }
            return num;
        }
        /// <summary>
        /// 重载 == 运算符，比较两个 Sequence 对象是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Sequence a, Sequence b)
        {
            return a.Equals(b);
        }
        /// <summary>
        /// 重载 == 运算符，比较一个 Sequence 对象和一个字符串是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Sequence a, string b)
        {
            return a.Equals(b);
        }
        /// <summary>
        /// 重载 == 运算符，比较一个字符串和一个 Sequence 对象是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(string a, Sequence b)
        {
            return b.Equals(a);
        }
        /// <summary>
        /// 重载 != 运算符，比较两个 Sequence 对象是否不相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Sequence a, Sequence b)
        {
            return !a.Equals(b);
        }
        /// <summary>
        /// 重载 != 运算符，比较一个 Sequence 对象和一个字符串是否不相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Sequence a, string b)
        {
            return !a.Equals(b);
        }
        /// <summary>
        /// 重载 != 运算符，比较一个字符串和一个 Sequence 对象是否不相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(string a, Sequence b)
        {
            return !b.Equals(a);
        }
        /// <summary>
        /// 存储的原始字符串
        /// </summary>
        public string String;
        /// <summary>
        /// 序列的起始位置
        /// </summary>
        public int Start;
        /// <summary>
        /// 序列的结束位置
        /// </summary>
        public int End;
    }
}
