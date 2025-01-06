using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MetalMaxSystem;
using StormLib;

namespace GalaxyObfuscator
{
    /// <summary>
    /// 混淆器
    /// </summary>
    internal class Obfuscator
    {
        public static int mutiCount = 0;
        public static Form1 form1;
        public string errFileName;

        public Obfuscator()
        {
            //string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;//exe路径
            //string exeDirectory = System.IO.Path.GetDirectoryName(exePath);

            //返回调试目录（如bin\Debug或bin\Release）或打包后的exe所在目录
            this.errFileName = AppDomain.CurrentDomain.BaseDirectory + @"/SyntaxErrorException.txt";
        }

        public Obfuscator(string fileName)
        {
            //初始化混淆器实例时记录errFileName
            this.errFileName = fileName + @"_SyntaxErrorException.txt";
        }

        /// <summary>
        /// 启动混淆过程。如果文件名不包含"Obfuscated"则创建混淆器实例并对目标文件进行混淆。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="torf">是否跳过含关键字文件</param>
        /// <param name="key">关键字，默认值"Obfuscated"</param>
        public static void Obfuscate(string fileName, bool torf = true, string key = "Obfuscated")
        {
            if (!torf || !fileName.Contains(key))
            {//没有启用跳过含关键字文件或文件名不包含关键字时启动
                mutiCount++;//记录执行次数，若为1则为单文件混淆，否则为多文件混淆
                //创建混淆器实例
                Obfuscator obfuscator = new Obfuscator(fileName);
                //对目标文件进行混淆
                obfuscator.ObfuscateFile(fileName);
                //↓如果勾选了打印报告（目前批量时只对第一个文件进行混淆时有调试报告，多文件暂不支持）
                //打印每个名称的混淆报告（会清空StringBuilder的缓存区，批量混淆时下个报告应能独立）
                MMCore.WriteLine(fileName + @"_混淆报告.txt", "████████████████████████████████████████████" + "\r\n" + "", true, true, false);//尾行留空
            }
        }
        /// <summary>
        /// 对目标文件进行混淆
        /// </summary>
        /// <param name="fileName">目标文件</param>
        public void ObfuscateFile(string fileName)
        {
            //确立混淆后的文件名
            string text = fileName.Substring(0, fileName.LastIndexOf('.')) + " Obfuscated.SC2Map";
            //文件已存在则删除
            if (File.Exists(text))
            {
                File.Delete(text);
            }
            //复制原始文件到目标文件位置
            File.Copy(fileName, text);
            //引用StormLib.dll创建MPQ档案管理器
            using (MpqArchive mpqArchive = new MpqArchive(text))
            {
                //打开MPQ格式文件中的MapScript.galaxy
                using (StreamReader streamReader = new StreamReader(new BufferedStream(mpqArchive.OpenFile("MapScript.galaxy"))))
                {
                    //字节转字符串
                    this.script = streamReader.ReadToEnd();
                }
                //混淆脚本内容
                this.script = this.obfuscateScript();

                //include部分转移至construct()处理
                ////正则表达式模式：匹配 include 后跟着的双引号内的字符串
                //string pattern = @"include\s*""([""]*)""";
                ////创建正则表达式对象
                //Regex regex = new Regex(pattern);
                ////查找匹配项
                ////MatchCollection matches = regex.Matches(script);
                ////将代码中经过pattern正则匹配到的替换为StringObfuscator.Obfuscate(m.Value) //m.Value 获取捕获组（即双引号内的字符串）
                //string obfuscatedCode = Regex.Replace(script, pattern,(Match m) => this.stringObfuscator.Obfuscate(m.Value));

                //创建临时文件
                string tempFileName = Path.GetTempFileName();
                //将混淆后的脚本写入临时文件
                File.WriteAllText(tempFileName, this.script);
                //将临时文件添加到MPQ存档中，替换原有的文件或添加新文件
                mpqArchive.AddFile(tempFileName, "MapScript.galaxy", true);
                //删除临时文件（清理资源）
                File.Delete(tempFileName);

                ////创建Triggers临时文件，准备修改
                //string tempTriggersFileName = Path.GetTempFileName();
                ////创建XML文件包含混淆后的脚本（用于替换Triggers文件）
                //using (FileStream fileStream = new FileStream(tempTriggersFileName, FileMode.OpenOrCreate))
                //{
                //    //创建XML写入器，用于向文件写入XML数据
                //    XmlWriter xmlWriter = XmlWriter.Create(fileStream);
                //    //写入XML声明
                //    xmlWriter.WriteStartDocument();
                //    //写入根元素<TriggerData>
                //    xmlWriter.WriteStartElement("TriggerData");
                //    //写入<Root>元素
                //    xmlWriter.WriteStartElement("Root");
                //    //写入一个<Item>元素，并为其添加Type和Id属性
                //    xmlWriter.WriteStartElement("Item");
                //    xmlWriter.WriteAttributeString("Type", "CustomScript");
                //    xmlWriter.WriteAttributeString("Id", "CFE7E55E");
                //    xmlWriter.WriteEndElement();
                //    //结束<Root>元素
                //    xmlWriter.WriteEndElement();
                //    //写入<Element>元素并为其添加Type和Id属性
                //    xmlWriter.WriteStartElement("Element");
                //    xmlWriter.WriteAttributeString("Type", "CustomScript");
                //    xmlWriter.WriteAttributeString("Id", "CFE7E55E");
                //    //写入<ScriptCode>元素
                //    xmlWriter.WriteStartElement("ScriptCode");
                //    //将混淆后的脚本内容作为其子元素的内容
                //    xmlWriter.WriteString(this.script);
                //    xmlWriter.WriteEndElement();
                //    //结束<Element>元素
                //    xmlWriter.WriteEndElement();
                //    //结束根元素<TriggerData>
                //    xmlWriter.WriteEndElement();
                //    //关闭XML写入器，完成XML文件的写入
                //    xmlWriter.Close();
                //}
                //Triggers必须删掉而不是替换，否则可能导致地图无法打开及发布失败（因为发布时会重新根据Triggers生成脚本覆盖，混淆就白做了）
                //mpqArchive.AddFile(tempTriggersFileName, "Triggers", true);
                //删除临时文件（清理资源）
                //File.Delete(tempTriggersFileName);
                //↓改为删除Triggers文件
                mpqArchive.RemoveFile("Triggers");
            }
        }

        /// <summary>
        /// 混淆脚本
        /// </summary>
        /// <returns>返回对Obfuscator.script混淆后的结果</returns>
        public string obfuscateScript()
        {
            if (form1.GetCheckLC4StateFromMainThread() == true)
            {//LC4先添加到脚本头尾
                string sCHeadPath = AppDomain.CurrentDomain.BaseDirectory + @"/Rules/SC2Head";
                string sCEndPath = AppDomain.CurrentDomain.BaseDirectory + @"/Rules/SC2End";
                string sCHead = File.ReadAllText(sCHeadPath);
                string sCEnd = File.ReadAllText(sCEndPath);
                this.script = sCHead + "\r\n" + this.script + "\r\n" + sCEnd;
                if (form1.GetSelectedIndexFromMainThread() == 1)
                {
                    MMCore.WriteLine("█脚本初始化：添加LC4头尾");
                    form1.SetCodeToMainThread(this.script);
                }
            }

            //使用字符集及长度限制创建标识符生成器
            this.identifierGenerator = new IdentifierGenerator("lI1", 16);
            //构建混淆字典（标识符表、字面量表）
            this.identifierTable = new Dictionary<Sequence, string>();
            this.literalTable = new Dictionary<Sequence, string>();
            //扫描脚本（分析并提取出所有需要混淆的标识符和字面量），涉及解析脚本代码识别出变量名、函数名等并记录在相应的表中
            MMCore.WriteLine("█↓扫描开始↓█");
            this.scan();
            MMCore.WriteLine("█Scan End█" + "\r\n" + "");
            MMCore.WriteLine("█↓标识符表↓█");
            MMCore.WriteLine(string.Join(",\r\n", identifierTable.Select(kvp => $"Key: {kvp.Key}, Value: {kvp.Value}")));
            MMCore.WriteLine("█identifierTable End█" + "\r\n" + "");
            MMCore.WriteLine("█↓字面量表↓█");
            MMCore.WriteLine(string.Join(",\r\n", literalTable.Select(kvp => $"Key: {kvp.Key}, Value: {kvp.Value}")));
            MMCore.WriteLine("█literalTable End█" + "\r\n" + "");
            //遍历标识符保留列表（这些标识符在混淆过程中不应被改变）
            //Obfuscator.ReservedIdentifiers包含了不应被混淆的标识符集合
            ReservedIdentifiers = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"Rules/exclusion_rules.txt");
            foreach (string str in ReservedIdentifiers)
            {
                //将保留标识符转换为Sequence类型（为了方便比较或存储）
                Sequence key = new Sequence(str);
                //参与混淆的identifierTable中包含该保留标识符的映射则将其移除，以便保留标识符在混淆后脚本中保持不变
                if (this.identifierTable.ContainsKey(key))
                {
                    //移除保留标识符
                    this.identifierTable.Remove(key);
                    MMCore.WriteLine("移除标识符：" + key);
                }
            }
            //构建混淆后的脚本
            return this.construct();
        }
        /// <summary>
        /// 判断给定的Token对象是否应该被视为一个独立的标记
        /// </summary>
        /// <param name="token">要判断的Token对象</param>
        /// <returns></returns>
        private static bool isSeparateToken(Token token)
        {
            //如果Token的类型是标识符（Identifier）、十六进制字面量（HexLiteral）、整数字面量（IntegerLiteral）或实数字面量（RealLiteral）中的任何一种
            //则认为它是一个独立的标记返回true，否则返回false
            return token.Type == TokenType.Identifier || token.Type == TokenType.HexLiteral || token.Type == TokenType.IntegerLiteral || token.Type == TokenType.RealLiteral;
        }
        /// <summary>
        /// 构建混淆后的脚本 
        /// </summary>
        /// <returns></returns>
        private string construct()
        {
            string tempStr; int tempInt;
            //初始化扫描器，用于扫描原始脚本
            this.scanner = new Scanner(this.script, this.errFileName);
            //初始化StringBuilder，用于构建混淆后的脚本
            //乘以10是为了预留足够的空间，避免频繁扩容
            StringBuilder stringBuilder = new StringBuilder(this.script.Length * 10);
            //处理脚本中的"include"指令
            //如果当前token是标识符且为"include"，则读取下一个token作为"include"的加载文件名
            while (this.scanner.Read().Type == TokenType.Identifier && this.scanner.Current.Sequence == "include")
            {
                //"include"后面紧跟着的是要"include"的加载文件名（作为字符串字面量）
                //这里没有处理可能的错误情况，比如"include"后面不是字符串字面量

                tempStr = scanner.Read().Content();//冒号间内容：Assets\\Textures\\HongMaster1.dds
                MMCore.WriteLine("字符串字面量：" + scanner.Current.ToString());//结果示范："Assets\\Textures\\HongMaster1.dds"
                MMCore.WriteLine("解析字符串字面量：" + scanner.Current.ParseStringLiteral());//结果示范：Assets/Textures/HongMaster1.dds
                tempStr = this.stringObfuscator.Obfuscate(tempStr);
                MMCore.WriteLine("混淆后：" + tempStr);
                stringBuilder.AppendLine("include " + tempStr);
            }
            //初始化一个空的token，用于标记上一个处理的token
            Token token = new Token
            {
                Type = TokenType.None
            };
            //无限循环处理token直到在循环内部遇到break或return
            for (; ; )
            {
                //获取当前扫描器指向的token
                Token token2 = this.scanner.Current;
                //如果当前token不是None（即需要有效的token）
                if (token2.Type != TokenType.None)
                {
                    //处理独立token之间的换行
                    //如果当前token和上一个token都是独立的标记则在StringBuilder中添加一个换行符保持混淆后脚本的可读性
                    //但显然混淆是不需要可读性的，这里直接将前后串起来
                    if (Obfuscator.isSeparateToken(token2) && Obfuscator.isSeparateToken(token))
                    {
                        stringBuilder.AppendLine();
                    }
                    //根据token类型进行处理
                    switch (token2.Type)
                    {
                        //如果identifierTable中包含了当前标识符的映射则使用映射后的标识符
                        case TokenType.Identifier:
                            if (this.identifierTable.ContainsKey(token2.Sequence))
                            {
                                //附加混淆后的标识符
                                stringBuilder.Append(this.identifierTable[token2.Sequence]);
                            }
                            else
                            {
                                //否则直接附加原始的标识符
                                stringBuilder.Append(token2.ToString());
                            }
                            break;
                        case TokenType.StringLiteral:
                            if (this.literalTable.ContainsKey(token2.Sequence))
                            {//如果literalTable中包含了当前字符串字面量的映射
                                //使用映射后的字面量
                                stringBuilder.Append(this.literalTable[token2.Sequence]);
                            }
                            else
                            {
                                //否则，对字符串字面量进行混淆处理并附加混淆后的结果
                                tempStr = token2.Content();//冒号间内容：Assets\\Textures\\HongMaster1.dds
                                MMCore.WriteLine("字符串字面量：" + token2.ToString());//结果示范："Assets\\Textures\\HongMaster1.dds"
                                MMCore.WriteLine("解析字符串字面量：" + token2.ParseStringLiteral());//结果示范：Assets/Textures/HongMaster1.dds
                                //如果标识符表中有变量名=解析后的字符串字面量（如声明了gv_u_Ship=飞船单位后，在事件注册中作为变量参数名字符串填入的情况）
                                Sequence tempSequence = new Sequence(token2.ParseStringLiteral());
                                if (this.identifierTable.ContainsKey(tempSequence))
                                {
                                    //如果存在则取出
                                    tempStr = '\"' + identifierTable[tempSequence] + '\"';
                                }
                                else
                                {
                                    //否则进行新混淆（会补上两侧冒号）
                                    tempStr = this.stringObfuscator.Obfuscate(tempStr);
                                }
                                MMCore.WriteLine("混淆后：" + tempStr);
                                stringBuilder.Append(tempStr);
                            }
                            break;
                        case TokenType.CharLiteral:
                        case TokenType.RealLiteral:
                        case TokenType.Symbol:
                            //对于字符字面量、实数字面量和符号直接跳转到IL_19D标签处理
                            goto IL_19D;
                        case TokenType.IntegerLiteral:
                        case TokenType.HexLiteral:
                            //对于整数字面量和十六进制字面量进行混淆处理并附加混淆后的结果
                            tempInt = token2.ParseIntegerLiteral();
                            MMCore.WriteLine("整数字面量：" + tempInt.ToString());
                            tempStr = this.integerObfuscator.Obfuscate(tempInt);
                            MMCore.WriteLine("混淆后：" + tempStr);
                            stringBuilder.Append(tempStr);
                            break;
                        default:
                            //对于其他类型的token也跳转到IL_19D标签处理
                            goto IL_19D;
                    }
                //IL_1B1标签：更新上一个token为当前token并检查是否有下一个token
                IL_1B1:
                    token = token2;
                    if (!this.scanner.MoveNext())
                    {
                        //如果没有下一个token则退出循环
                        break;
                    }
                    //继续下一次循环迭代
                    continue;
                //IL_19D标签：对不需要特殊处理的token直接附加其原始文本
                IL_19D:
                    stringBuilder.Append(token2.ToString());
                    //跳转到IL_1B1标签进行下一次迭代
                    goto IL_1B1;
                }
                break;//目前多余的，因为'goto IL_1B1;'已经确保了不会执行到这里
            }
            //返回构建好的混淆脚本
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 添加标识符到表中（将当前扫描器指向的标识符添加到标识符表中并为其生成一个新名称）
        /// </summary>
        private void addIdentifier()
        {
            //从扫描器获取当前标识符的文本，利用标识符生成器为当前标识符创造一个新名称
            //将原始标识符及其新名称作为键值对添加到标识符表中
            string temp = this.identifierGenerator.Generate();
            this.identifierTable.Add(this.scanner.Current.Sequence, temp);
            MMCore.WriteLine("存储标识符表" + $"Key: {this.scanner.Current.Sequence}, Value: {temp}");
        }

        /// <summary>
        /// 扫描脚本并构建混淆表
        /// </summary>
        /// <exception cref="SyntaxErrorException"></exception>
        private void scan()
        {
            //创建扫描器并填入要扫描的原始脚本
            this.scanner = new Scanner(this.script);
            //读标记（识别空白字符和注释、标识符、字面量、符号并读其中一种直到各自结尾符，为一次标记，一直循环到代码全部读完）
            while (this.scanner.MoveNext())
            {
                //获取当前分析出来的标记
                Token token = this.scanner.Current;
                MMCore.WriteLine("新的遍历标记！" + $"Type: {token.Type.ToString()}, Value: {token.ToString()}");
                //如果当前标记为None表示没有更多标记可读，因此退出循环
                if (token.Type == TokenType.None)
                {
                    return;
                }
                //如果当前标记不是标识符，则抛出语法错误异常
                if (token.Type != TokenType.Identifier)
                {
                    //gf_ObfAuto_syscalltemplate(){};funcref<gf_ObfAuto_syscalltemplate>[...问题发生时指针位置在{}后的分号
                    //如果退出代码块时，代码块里什么都没有，应归属于正常现象（由于未捕获上个标记头暂不分析代码块，直接按遇}或;当正常继续）
                    //实际上有非空内容应处理而非随意跳过（待处理）
                    //if (token.Type == TokenType.Symbol && (token.ToString() == "}" || token.ToString() == ";"))
                    //{
                    //    continue;
                    //}
                    MMCore.WriteLine("当前标记不是标识符，抛出！" + $"Type: {token.Type.ToString()}, Value: {token.ToString()}");
                    throw new SyntaxErrorException(this.scanner);
                }
                //将当前标识符转换为字符串
                string a;
                if ((a = token.ToString()) != null)
                {
                    MMCore.WriteLine("进入检查！" + $"Type: {token.Type.ToString()}, Value: {token.ToString()}");
                    //根据标识符的值执行不同的解析逻辑
                    if (a == "include")
                    {
                        //读取并期望下一个标记为字符串字面量（表示"include"的加载文件名）
                        this.scanner.ReadExpectedToken(TokenType.StringLiteral);
                        continue;
                    }
                    if (!(a == "typedef"))
                    {
                        if (a == "trigger")
                        {
                            //扫描触发器定义
                            this.scanTriggerDefinition();
                            continue;
                        }
                        if (a == "struct")
                        {
                            //扫描结构体定义
                            this.scanStructDefinition();
                            continue;
                        }
                    }
                    else
                    {
                        //读取并期望下一个标记为标识符（新类型名）
                        this.scanner.ReadExpectedToken(TokenType.Identifier);
                        //解析变量声明（此处假设typedef后紧跟变量声明）
                        this.scanVariableDeclaration(false);
                        //期望下一个标记为分号，表示声明结束
                        if (this.scanner.Current.Sequence != ";")
                        {
                            MMCore.WriteLine("期望下一个标记为分号表示声明结束，但不存在所以抛出！" + $"Type: {this.scanner.Current.Type.ToString()}, Value: {this.scanner.Current.ToString()}");
                            throw new SyntaxErrorException(this.scanner);
                        }
                        continue;
                    }
                }
                //如果标识符不是上述特殊关键字则解析为一个普通的声明
                this.scanDeclaration();
            }
        }

        /// <summary>
        /// 扫描触发器定义 
        /// </summary>
        private void scanTriggerDefinition()
        {
            string temp;
            //读取一个预期的标识符标记
            this.scanner.ReadExpectedToken(TokenType.Identifier);
            //将读取的标识符添加到标识符表
            this.addIdentifier();
            //初始化字符串数组用于存储处理后的标识符
            string[] array = new string[2];
            string[] array2 = array;
            int num = 0;
            //获取当前标记（是之前读取的标识符）
            Token token = this.scanner.Current;
            //将当前标识符加上"_Func"后缀，并存入数组的第一个位置
            array2[num] = token.ToString() + "_Func";
            string[] array3 = array;
            int num2 = 1;
            //再次获取当前标记
            Token token2 = this.scanner.Current;
            //将当前标识符加上"Func"后缀，并存入数组的第二个位置
            array3[num2] = token2.ToString() + "Func";
            //遍历处理后的标识符数组
            foreach (string text in array)
            {
                string text2;
                //检查标识符表中是否已经存在该标识符（使用Sequence作为键）
                if (!this.identifierTable.ContainsKey(new Sequence(text)))
                {
                    //如果不存在，则生成一个新的标识符
                    text2 = this.identifierGenerator.Generate();
                    //将新生成的标识符添加到标识符表中
                    this.identifierTable.Add(new Sequence(text), text2);
                }
                else
                {
                    //如果存在则直接使用已有的标识符（应该是这种情况）
                    text2 = this.identifierTable[new Sequence(text)];
                }

                //将处理后的标识符（新生成的或是已有的）添加到字面量表中
                //注意：这里将标识符用双引号括起来，并进行了混淆处理，字符串中有同名函数的应采用标识符混淆表同一Key对应的混淆值
                temp = this.stringObfuscator.Obfuscate(text2);
                this.literalTable.Add(new Sequence("\"" + text + "\""), temp);
                MMCore.WriteLine("存储至字面量表 " + $"Key: {text}, Value: {temp}");
            }
            //跳过至下一个分号，表示触发器定义的结束
            this.scanner.SkipBlock(";");
        }

        /// <summary>
        /// 跳过表达式直至遭遇特定的终止符号（分号、逗号或右括号）
        /// </summary>
        private void skipExpression()
        {
            //利用无限循环不断读取并处理标记，直至满足退出条件
            for (; ; )
            {
                //读取下一个预期的标记并立即检查其类型是否为符号
                if (this.scanner.ReadExpectedToken().Type == TokenType.Symbol)
                {
                    //获取当前标记的引用
                    Token token = this.scanner.Current;
                    //将标记转换为字符串形式以便进行后续比较
                    //此处的转换通常不会返回null因为Token的ToString方法应当总是返回有效的字符串
                    string a;
                    if ((a = token.ToString()) != null)
                    {
                        //首先检查标记是否为左括号
                        if (!(a == "("))
                        {
                            //如果不是左括号则进一步检查是否为终止符号（分号、逗号或右括号）
                            if (a == ";" || a == "," || a == ")")
                            {
                                //如果是终止符号则退出循环
                                break;
                            }
                        }
                        else
                        {
                            //如果是左括号则调用SkipBlock方法跳过由左右括号包围的内容
                            //通常用于跳过括号内的表达式或语句块
                            this.scanner.SkipBlock("(", ")");
                        }
                    }
                    //如果标记的字符串形式为null（虽然这种情况很少发生但理论上应当处理）
                    //或者不是需要关注的符号则循环将继续，读取下一个标记
                }
                //如果读取的标记不是符号类型则循环也将继续
            }
            //当遇到终止符号并退出循环后方法执行完毕
        }

        /// <summary>
        /// 扫描变量声明语句
        /// </summary>
        /// <param name="allowMultiple">表示是否允许声明多个变量（通过逗号分隔）</param>
        /// <exception cref="SyntaxErrorException"></exception>
        private void scanVariableDeclaration(bool allowMultiple = false)
        {
            if (this.scanner.Current.Sequence == "const")
            {
                //如果当前扫描到的标记是"const"则读取接下来的标识符（变量名）
                this.scanner.ReadExpectedToken(TokenType.Identifier);
            }
            //读取并跳过变量类型（假设类型已经在之前被读取）
            this.scanner.ReadExpectedToken();
            //处理可能存在的数组维度（例如 int[] arr）
            while (this.scanner.Current.Type == TokenType.Symbol && this.scanner.Current.Sequence == "[")
            {
                // 跳过数组维度内的所有内容直到找到闭合的']'
                do
                {
                    this.scanner.ReadExpectedToken();
                }
                while (this.scanner.Current.Type != TokenType.Symbol || this.scanner.Current.Sequence != "]");
                //读取并跳过闭合的']'
                this.scanner.ReadExpectedToken();
            }
            //处理特殊类型声明（例如arrayref<>或funcref<>）
            while (this.scanner.Current.Type == TokenType.Symbol && this.scanner.Current.Sequence == "<")
            {
                //跳过直到找到闭合的'>'
                do
                {
                    this.scanner.ReadExpectedToken();
                }
                while (this.scanner.Current.Type != TokenType.Symbol || this.scanner.Current.Sequence != ">");
                //读取并跳过闭合的'>'
                this.scanner.ReadExpectedToken();
            }
            //gf_ObfAuto_syscalltemplate(){};funcref<gf_ObfAuto_syscalltemplate>[...问题发生时指针位置在>后的[，尖括号不能比[]先处理否则指针跨度太大导致没有处理到关键内容
            //除非符号或空格，否则不应跳过（[]后面一般不会直接连着<>所以跳跃动作虽然连着但不会发生，如果发生就要处理[]和<>代码块里的内容）

            //检查当前标记是否为标识符（变量名）
            if (this.scanner.Current.Type != TokenType.Identifier)
            {
                //如果不是则抛出语法错误异常，函数内if...;}DataTableSetBool(...问题发生时指针位置在(符号处，标记是DataTableSetBool但它不是标识符（Native函数未在代码文件声明）
                //DataTableSetBool(false,lll[(l1I1^0x2a397^(~-0X1e1f9d8c))][((1<<(-0x2B+IIl))^((~-1785843762)^l1Il))][((-0X99^llII)+lllI+(~0X57F5F7D0))],lp_bool0);
                //实际上有非空内容应处理而非随意跳过（待处理）
                MMCore.WriteLine("期望是变量但不是所以抛出！" + $"Type: {this.scanner.Current.Type.ToString()}, Value: {this.scanner.Current.ToString()}");
                throw new SyntaxErrorException(this.scanner);
            }
            //如果标识符表中不包含当前标识符则将其添加进去
            if (!this.identifierTable.ContainsKey(this.scanner.Current.Sequence))
            {
                this.addIdentifier();
            }
            //读取并跳过'='符号（如果存在表示有初始化表达式）
            this.scanner.ReadExpectedToken(TokenType.Symbol);
            //如果当前标记是'='则跳过初始化表达式
            if (this.scanner.Current.Sequence == "=")
            {
                this.skipExpression();
            }
            //如果允许声明多个变量则继续处理逗号分隔的变量声明
            if (allowMultiple)
            {
                //循环处理每个逗号分隔的变量声明
                while (this.scanner.Current.Sequence == ",")
                {
                    //读取并跳过逗号
                    this.scanner.ReadExpectedToken(TokenType.Identifier);
                    //添加新的标识符到标识符表中
                    this.addIdentifier();
                    //读取并跳过'='符号（如果存在）
                    this.scanner.ReadExpectedToken(TokenType.Symbol);
                    //如果当前标记是'='则跳过初始化表达式
                    if (this.scanner.Current.Sequence == "=")
                    {
                        this.skipExpression();
                    }
                }
            }
        }

        /// <summary>
        /// 解析并处理结构体定义
        /// </summary>
        /// <exception cref="SyntaxErrorException"></exception>
        private void scanStructDefinition()
        {
            //读取结构体名称
            this.scanner.ReadExpectedToken(TokenType.Identifier);
            this.addIdentifier();
            //读取{符号
            this.scanner.ReadExpectedSymbol("{");
            TokenType type;
            for (; ; )
            {
                type = this.scanner.Read().Type;
                if (type != TokenType.Identifier)
                {
                    break;
                }
                //解析成员变量声明
                this.scanVariableDeclaration(true);
                //检查分号
                if (this.scanner.Current.Sequence != ";")
                {
                    goto Block_3;
                }
            }
            //确保读取到}右大括号
            if (type != TokenType.Symbol)
            {
                MMCore.WriteLine("期望是符号但不是所以抛出！" + $"Type: {type.ToString()}, Value: {this.scanner.Current.ToString()}");
                throw new SyntaxErrorException(this.scanner);
            }
            if (this.scanner.Current.Sequence != "}")
            {
                MMCore.WriteLine("期望是右大括号但不是所以抛出！" + $"Type: {type.ToString()}, Value: {this.scanner.Current.ToString()}");
                throw new SyntaxErrorException(this.scanner);
            }
            //确保读取到;分号
            this.scanner.ReadExpectedSymbol(";");
            return;
        Block_3:
            MMCore.WriteLine("期望是分号但不是所以抛出！" + $"Type: {type.ToString()}, Value: {this.scanner.Current.ToString()}");
            throw new SyntaxErrorException(this.scanner);
        }

        /// <summary>
        /// 扫描和处理声明语句（可能的变量或函数声明）
        /// </summary>
        /// <exception cref="SyntaxErrorException"></exception>
        private void scanDeclaration()
        {
            //首先以允许声明多个变量的模式扫描变量声明
            this.scanVariableDeclaration(true);
            //获取当前扫描器指向的标记（token）并尝试将其转换为字符串
            Token token = this.scanner.Current;
            string a;
            //如果转换后的字符串为空则直接返回，不进行后续处理
            if ((a = token.ToString()) == null)
            {
                return;
            }
            //检查当前标记是否为分号或不是左括号
            //如果是分号表示声明语句结束，不是左括号则可能不是函数声明，直接返回。
            if (a == ";" || !(a == "("))
            {
                return;
            }
            //如果当前标记是左括号则读取并跳过它，准备解析函数参数
            this.scanner.ReadExpectedToken();
            //如果接下来的标记是标识符（可能表示函数参数的类型）则以不允许声明多个变量的模式再次扫描变量声明（处理函数参数）
            if (this.scanner.Current.Type == TokenType.Identifier)
            {
                this.scanVariableDeclaration(false);
                //如果后续还有逗号表示有多个参数，继续扫描每个参数
                while (this.scanner.Current.Sequence == ",")
                {
                    //读取并跳过逗号
                    this.scanner.ReadExpectedToken(TokenType.Identifier);
                    //扫描下一个参数声明
                    this.scanVariableDeclaration(false);
                }
            }
            //此时应期望一个右括号来结束函数参数列表，如果不是右括号则抛出语法错误异常
            if (this.scanner.Current.Type != TokenType.Symbol && this.scanner.Current.Sequence != ")")
            {
                MMCore.WriteLine("期望是右括号来结束函数参数列表但不是所以抛出！" + $"Type: {this.scanner.Current.Type.ToString()}, Value: {this.scanner.Current.Sequence.ToString()}");
                throw new SyntaxErrorException(this.scanner);
            }
            //读取并跳过右括号
            this.scanner.ReadExpectedToken(TokenType.Symbol);
            //接下来的标记应该是分号表示声明语句结束或者是左大括号表示函数体的开始
            if (this.scanner.Current.Sequence == ";")
            {
                //如果是分号则直接返回，声明语句结束
                return;
            }
            if (this.scanner.Current.Sequence == "{")
            {
                //如果是左大括号则调用scanBlock方法扫描函数体
                this.scanBlock();
                return;
            }
            // 如果既不是分号也不是左大括号则抛出语法错误异常
            MMCore.WriteLine("期望是右括号来结束函数参数列表但不是所以抛出！" + $"Type: {scanner.Current.Type.ToString()}, Value: {this.scanner.Current.Sequence.ToString()}");
            throw new SyntaxErrorException(this.scanner);
        }

        /// <summary>
        /// 跳过特定的指令（如for循环的初始化语句）
        /// </summary>
        private void skipInstruction()
        {
            //获取当前扫描器所指的标记（token）
            Token token = this.scanner.Current;
            //如果当前标记是"for"关键字，设置num为2
            //因为for循环通常包含初始化语句、条件判断语句和迭代语句，此处只想跳过初始化语句所以需要额外处理跳过两个分号（防止后面期望函数无法正确）
            int num = (token.ToString() == "for") ? 2 : 0;
            //使用无限循环来持续读取标记直至满足退出条件
            for (; ; )
            {
                //检查当前标记是否为符号
                if (this.scanner.Current.Type == TokenType.Symbol)
                {
                    //获取当前符号标记
                    Token token2 = this.scanner.Current;
                    string a;
                    //如果符号不是null（即确实读取到了符号）
                    if ((a = token2.ToString()) != null)
                    {
                        //如果符号是左大括号{表示代码块开始，应退出循环
                        if (a == "{")
                        {
                            break;
                        }
                        //如果符号是分号;且num减为0表示已经跳过了for循环的初始化语句，应返回
                        if (a == ";")
                        {
                            if (num-- == 0)
                            {
                                return;
                            }
                        }
                    }
                }
                //读取并跳过当前标记，继续检查下一个标记
                this.scanner.ReadExpectedToken();
            }
            //当退出循环时说明遇到了左大括号{表示代码块开始，此时调用scanBlock方法来扫描并处理该代码块
            this.scanBlock();
        }

        /// <summary>
        /// 扫描并处理代码块（例如函数体、循环体等）直至遇到右大括号}表示代码块结束
        /// </summary>
        /// <exception cref="SyntaxErrorException"></exception>
        private void scanBlock()
        {
            string result; bool torf = false;
            int end = -1;
            int start = scanner.position;//当前扫描指针在代码块头部{位置
            //MMCore.WriteLine("StartIndex：" + start);
            //使用无限循环来持续读取标记直至满足退出条件
            for (; ; )
            {
                //读取并跳过当前标记然后检查下一个标记
                this.scanner.ReadExpectedToken();
                //如果当前标记是符号则进一步判断
                if (this.scanner.Current.Type == TokenType.Symbol)
                {
                    //如果符号是右大括号}表示代码块结束，应退出循环
                    if (this.scanner.Current.Sequence == "}")
                    {
                        //退出代码块（已经退出了就无需记录位置，改为在嵌套外层记录）
                        //MMCore.WriteLine("退出代码块！");
                        break;
                    }
                    //如果符号是左大括号{表示嵌套的代码块开始，递归调用scanBlock方法处理
                    if (this.scanner.Current.Sequence == "{")
                    {
                        //MMCore.WriteLine("准备递归进入代码块内部扫描！" + $"Type: {scanner.Current.Type.ToString()}, Value: {scanner.Current.ToString()}");
                        start = scanner.position - 1;//进嵌套块前开始位置要刷新，因ReadExpectedToken使当前标记和指针错位（指针又走了一步，所以要-1才是{符所在）
                        this.scanBlock();
                        end = scanner.position - 1;//记录刚退出嵌套块时的扫描指针位置（当前标记和指针错位依然是错位状态）
                        //MMCore.WriteLine("递归结束！EndIndex：" + end + $" Type: {scanner.Current.Type.ToString()}, Value: {scanner.Current.ToString()}");
                    }
                    else
                    {
                        //如果符号是分号表示一条语句结束，继续检查下一条语句
                        if (this.scanner.Current.Sequence == ";")
                        {
                            end = scanner.position;
                            //MMCore.WriteLine("指针停在分号的后面1位：" + end);
                            continue;
                        }
                        //如果符号不是上述三种情况之一则跳过以分号为结尾的代码块
                        //↓处理如单行注释或特定语法结构的跳过
                        //this.scanner.SkipBlock(";");//一直跳直到指针停在分号的后面1位但如果是空块会报错（无限循环）

                        //↓修复如下
                        //MMCore.WriteLine("指针位置：" + scanner.position + " 准备一直跳，直到指针停在指定符号之一的后面1位");
                        this.scanner.SkipBlockPro(";", "{", "}");//一直跳直到指针停在指定符号之一的后面1位
                        end = scanner.position - 1;//指针停在指定符号之一的后面1位
                    }
                }
                //如果当前标记不是标识符则跳转到Block_4标签抛出语法错误异常，或在此处处理异常
                if (this.scanner.Current.Type != TokenType.Identifier)
                {
                    //MMCore.WriteLine("标记类型非标识符！当前指针位置：" + scanner.position);
                    //如果退出代码块时，代码块里什么都没有，应归属于正常现象
                    if (end == -1)
                    {//本层是递归方法最内部或无嵌套检查代码块情况，说明有未识别语法或语法错误，处理或记录它
                        if (scanner.position - 1 - start == 1)
                        {//如果{}前后相靠
                            result = scanner.Current.Sequence.String.Substring(start, 2);
                            if (result == "{}")
                            {//检查是否为{}
                                //通过
                                torf = true;
                                MMCore.WriteLine("代码块为{}相靠！");
                            }
                        }
                        else
                        {//{}非前后相靠，说明夹着其他字符
                            //记录夹着的字符
                            result = scanner.Current.Sequence.String.Substring(start + 1, scanner.position - start - 2);
                            //检查字符串是否为null或者全是空格
                            if (string.IsNullOrEmpty(result) || result.All(char.IsWhiteSpace))
                            {
                                //通过
                                torf = true;
                                MMCore.WriteLine("代码块为空或全是空格！");
                            }
                        }
                    }
                    else
                    {//本层有嵌套检查代码块情况，检查是否是嵌套块的异常（有未识别语法或语法错误），处理或记录它
                        if (end == scanner.position - 1)
                        {//是递归块跑到这里来
                            if (scanner.position - 1 - start == 1)
                            {//如果递归块{}前后相靠
                                result = scanner.Current.Sequence.String.Substring(start, 2);
                                if (result == "{}")
                                {//检查递归块是否为{}
                                 //通过
                                    torf = true;
                                    MMCore.WriteLine("嵌套代码块为{}相靠！");
                                }
                            }
                            else
                            {//递归块{}非前后相靠，说明夹着其他字符
                             //记录夹着的字符
                                result = scanner.Current.Sequence.String.Substring(start + 1, scanner.position - start - 2);
                                //检查字符串是否为null或者全是空格
                                if (string.IsNullOrEmpty(result) || result.All(char.IsWhiteSpace))
                                {
                                    //通过
                                    torf = true;
                                    MMCore.WriteLine("嵌套代码块为空或全是空格！");
                                }
                            }
                        }
                    }
                    //MMCore.WriteLine("↓扫描结果↓");
                    //MMCore.WriteLine(scanner.Current.Sequence.String.Substring(start, scanner.position - start));
                    //MMCore.WriteLine("↑扫描结果↑");
                    // 检查字符串是否为空或者全是空格
                    if (torf == false)
                    {
                        result = scanner.Current.Sequence.String.Substring(start, scanner.position - start);
                        MMCore.WriteLine("严重意外：代码块非null或空格，亦非有效标识符！请反馈！");
                        MMCore.WriteLine("↓异常扫描结果↓");
                        MMCore.WriteLine(result);
                        MMCore.WriteLine("↑异常扫描结果↑");
                        goto Block_4;
                    }
                }
                //获取关键字列表用于后续判断当前标识符是否为关键字
                IEnumerable<string> keywords = Obfuscator.Keywords;
                //获取当前标识符标记
                Token token = this.scanner.Current;
                //如果当前标识符是关键字则跳过该指令
                if (keywords.Contains(token.ToString()))
                {
                    this.skipInstruction();
                }
                else
                {
                    //如果不是关键字则尝试扫描变量声明
                    try
                    {
                        this.scanVariableDeclaration(true);
                    }
                    //如果遇到UnexpectedEndOfFileException异常则直接抛出
                    catch (UnexpectedEndOfFileException ex)
                    {
                        throw ex;
                    }
                    //如果遇到SyntaxErrorException异常则跳过当前指令并继续检查下一个标记
                    catch (SyntaxErrorException)
                    {
                        this.skipInstruction();
                        continue;
                    }
                    //如果扫描变量声明后当前标记不是分号则跳转到Block_7标签，抛出语法错误异常
                    if (this.scanner.Current.Sequence != ";")
                    {
                        goto Block_7;
                    }
                }
            }
            return;
        Block_4:
            MMCore.WriteLine("期望是标识符（如关键字、结构体名、变量名、函数名等）但不是所以抛出！" + $"Type: {scanner.Current.Type.ToString()}, Value: {this.scanner.Current.Sequence.ToString()}");
            throw new SyntaxErrorException(this.scanner);
        Block_7:
            MMCore.WriteLine("期望序列字符串是分号但不是所以抛出！" + $"Type: {scanner.Current.Type.ToString()}, Value: {this.scanner.Current.Sequence.ToString()}");
            throw new SyntaxErrorException(this.scanner);
        }

        /// <summary>
        /// 定义脚本文件的名称常量
        /// </summary>
        private const string ScriptFileName = "MapScript.galaxy";
        /// <summary>
        /// 定义关键字数组（扫描时直接跳过，扫描指针将停留在它们后面）。这些关键字在脚本解析中具有特殊含义（如include、void、int、for），除不应被混淆（逐字混淆方式凡没有声明的本就不会被混淆）还决定后面可预期的扫描方法。
        /// For循环初始化语句中因有分号特殊（为防止预期处理函数无法正确识别）所以检测关键字后立即让扫描指针2次跳过分号
        /// </summary>
        private static readonly string[] Keywords = new string[]
{
            "for",
            "while",
            "if",
            "else",
            "elseif", //C#中为else if
            //"switch", //Galaxy里没有switch，会被转成if..elseif..
            "foreach",
            "arrayref",//不添加导致扫描中断
            "funcref",//不添加导致扫描中断
            "return"
};
        private static string[] _reservedIdentifiers;
        /// <summary>
        /// 标识符保留数组，这些标识符在脚本中具有特殊用途，不应被混淆。
        /// 从扫描到声明的结构体、变量、函数名表（参与混淆）中排除保留标识符，官方函数因在代码文件中没声明所以不在该表。
        /// </summary>
        public static string[] ReservedIdentifiers
        {
            get
            {
                return _reservedIdentifiers;
            }
            set
            {
                _reservedIdentifiers = value;
            }
        }
        //        private static readonly string[] ReservedIdentifiers = new string[]
        //{
        //            "InitMap"
        //};

        /// <summary>
        /// 标识符生成器，用于生成唯一的标识符或进行某种形式的标识符混淆
        /// </summary>
        private IdentifierGenerator identifierGenerator;
        /// <summary>
        /// 标识符表，存储已声明的标识符及其相关信息（混淆前后的名称）
        /// </summary>
        private IDictionary<Sequence, string> identifierTable;
        /// <summary>
        /// 字面量表，存储已出现的字面量（如字符串、数字）及其相关信息（混淆前后的值）
        /// </summary>
        private IDictionary<Sequence, string> literalTable;
        /// <summary>
        /// 扫描器对象，用于逐个读取脚本文件的标记（token）
        /// </summary>
        private Scanner scanner;
        /// <summary>
        /// 存储脚本内容的字符串
        /// </summary>
        public string script;
        /// <summary>
        /// 随机数生成器，可能用于混淆过程中的随机化操作
        /// </summary>
        private Random random = new Random();
        /// <summary>
        /// 字符串混淆器，用于对字符串字面量进行混淆处理
        /// </summary>
        private StringObfuscator stringObfuscator = new StringObfuscator();
        /// <summary>
        /// 整数混淆器，用于对整数字面量进行混淆处理
        /// </summary>
        private IntegerObfuscator integerObfuscator = new IntegerObfuscator();
    }
}
