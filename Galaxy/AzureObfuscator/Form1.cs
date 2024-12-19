using GalaxyObfuscator;
using MetalMaxSystem;
using StormLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Xml;

namespace GalaxyObfuscator
{
    public partial class Form1 : Form
    {
        static bool _userOpEnable = true;
        /// <summary>
        /// 用户操作许可
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        /// <summary>
        /// 用户操作许可
        /// </summary>
        public static bool UserOpEnable { get => _userOpEnable; set => _userOpEnable = value; }

        static bool _workStatus = false;
        /// <summary>
        /// 工作状态
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        /// <summary>
        /// 工作状态
        /// </summary>
        public static bool WorkStatus { get => _workStatus; set => _workStatus = value; }

        static bool _workStop = false;
        /// <summary>
        /// 打断工作用的状态变量
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        /// <summary>
        /// 打断工作用的状态变量
        /// </summary>
        public static bool WorkStop { get => _workStop; set => _workStop = value; }

        static Thread _workThread;
        /// <summary>
        /// 工作专用后台子线程，防止工作时UI主线程界面卡住无法点击等问题
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        /// <summary>
        /// 工作专用后台子线程，防止工作时UI主线程界面卡住无法点击等问题
        /// </summary>
        public static Thread WorkThread { get => _workThread; set => _workThread = value; }

        public Form1()
        {
            InitializeComponent();
            label_Statistics.ForeColor = Color.Red;
        }

        bool GetCheckTestStateFromMainThread()
        {
            if (checkBox_Test.InvokeRequired)
            {
                return (bool)checkBox_Test.Invoke(new Func<bool>(GetCheckTestStateFromMainThread));
            }
            else
            {
                return checkBox_Test.Checked;
            }
        }

        void SetCheckTestStateToMainThread(bool state)
        {
            // 调用 Invoke 方法将操作发送到主线程
            Invoke((MethodInvoker)delegate ()
            {
                checkBox_Test.Checked = state;
            });
        }

        bool GetCheckLC4StateFromMainThread()
        {
            if (checkBox_LC4.InvokeRequired)
            {
                return (bool)checkBox_LC4.Invoke(new Func<bool>(GetCheckLC4StateFromMainThread));
            }
            else
            {
                return checkBox_LC4.Checked;
            }
        }

        void SetCheckLC4StateToMainThread(bool state)
        {
            // 调用 Invoke 方法将操作发送到主线程
            Invoke((MethodInvoker)delegate ()
            {
                checkBox_LC4.Checked = state;
            });
        }

        string GetCodeFromMainThread()
        {
            if (richTextBox_Code.InvokeRequired)
            {
                return (string)richTextBox_Code.Invoke(new Func<string>(GetCodeFromMainThread));
            }
            else
            {
                return richTextBox_Code.Text;
            }
        }

        void SetCodeToMainThread(string code)
        {
            // 调用 Invoke 方法将操作发送到主线程
            Invoke((MethodInvoker)delegate ()
            {
                richTextBox_Code.Text = code;
            });
        }

        int GetSelectedIndexFromMainThread()
        {
            if (comboBox_SelectFunc.InvokeRequired)
            {
                return (int)comboBox_SelectFunc.Invoke(new Func<int>(GetSelectedIndexFromMainThread));
            }
            else
            {
                return comboBox_SelectFunc.SelectedIndex;
            }
        }

        string GetTipsFromMainThread()
        {
            if (label_Tips.InvokeRequired)
            {
                return (string)label_Tips.Invoke(new Func<string>(GetTipsFromMainThread));
            }
            else
            {
                return label_Tips.Text;
            }
        }

        void SetTipsToMainThread(string tips)
        {
            // 调用 Invoke 方法将操作发送到主线程
            Invoke((MethodInvoker)delegate ()
            {
                label_Tips.Text = tips;
            });
        }

        string GetStatisticsFromMainThread()
        {
            if (label_Statistics.InvokeRequired)
            {
                return (string)label_Statistics.Invoke(new Func<string>(GetStatisticsFromMainThread));
            }
            else
            {
                return label_Statistics.Text;
            }
        }

        string GetWorkPathFromMainThread()
        {
            if (textBox_workPath.InvokeRequired)
            {
                return (string)textBox_workPath.Invoke(new Func<string>(GetWorkPathFromMainThread));
            }
            else
            {
                return textBox_workPath.Text;
            }
        }

        void SetWorkPathToMainThread(string path)
        {
            // 调用 Invoke 方法将操作发送到主线程
            Invoke((MethodInvoker)delegate ()
            {
                textBox_workPath.Text = path;
            });
        }

        string GetRulePathFromMainThread()
        {
            if (textBox_rulePath.InvokeRequired)
            {
                return (string)textBox_rulePath.Invoke(new Func<string>(GetRulePathFromMainThread));
            }
            else
            {
                return textBox_rulePath.Text;
            }
        }

        void SetRulePathToMainThread(string path)
        {
            // 调用 Invoke 方法将操作发送到主线程
            Invoke((MethodInvoker)delegate ()
            {
                textBox_rulePath.Text = path;
            });
        }

        void SetStatisticsToMainThread(string text)
        {
            // 调用 Invoke 方法将操作发送到主线程
            Invoke((MethodInvoker)delegate ()
            {
                label_Statistics.Text = text;
            });
        }

        void SetBtnRunTextToMainThread(string text)
        {
            // 调用 Invoke 方法将操作发送到主线程
            Invoke((MethodInvoker)delegate ()
            {
                button_Run.Text = text;
            });
        }

        void SetPanelBackColorToMainThread(Panel p, Color c)
        {
            // 调用 Invoke 方法将操作发送到主线程
            Invoke((MethodInvoker)delegate ()
            {
                p.BackColor = c;
            });
        }

        void SetControlEnableToMainThread(Control c, bool torf)
        {
            // 调用 Invoke 方法将操作发送到主线程
            Invoke((MethodInvoker)delegate ()
            {
                c.Enabled = torf;
            });
        }

        Type GetControlTypeFromMainThread(Control control)
        {
            Type type = null;
            if (control.InvokeRequired) // 判断当前线程是否为UI主线程
            {
                control.BeginInvoke((MethodInvoker)(() => GetControlTypeFromMainThread(control))); // 将操作放入UI主线程的消息队列中
            }
            else
            {
                type = control.GetType(); // 获取控件的类型信息
            }
            return type;
        }

        string GetRichTextBoxListFromMainThread()
        {
            if (richTextBox_List.InvokeRequired)
            {
                return (string)richTextBox_List.Invoke(new Func<string>(GetRichTextBoxListFromMainThread));
            }
            else
            {
                return richTextBox_List.Text;
            }
        }

        void SetRichTextBoxListToMainThread(string code)
        {
            // 调用 Invoke 方法将操作发送到主线程
            Invoke((MethodInvoker)delegate ()
            {
                richTextBox_List.Text = code;
            });
        }

        /// <summary>
        /// 按钮点击后处理主要内容，本函数交由后台线程运行
        /// </summary>
        void ButtonRun()
        {
            string directoryPath = textBox_workPath.Text;
            for (int i = 0; i < 1; i++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                switch (GetSelectedIndexFromMainThread())
                {
                    case -1:
                        SetTipsToMainThread("功能未选择！");
                        break;
                    case 0:
                        SelectedFunc_0();
                        break;
                    case 1:
                        SelectedFunc_1();
                        break;
                    case 2:
                        SelectedFunc_2();
                        break;
                    default:
                        SetTipsToMainThread("功能无效！");
                        break;
                }
                stopwatch.Stop();
                //Debug.WriteLine(stopwatch.Elapsed.ToString());
                SetStatisticsToMainThread(" 时耗：" + stopwatch.Elapsed.ToString());
                //MessageBox.Show("Completed!");
            }
            //放弃了线程注销做法，程序将始终运行至此，可以知道是用户中断还是正常运行结束
            WorkStatus = false;//重置工作状态
            if (WorkStop) { SetStatisticsToMainThread("用户取消！"); }
            WorkStop = false;//重置_workStop状态，如果是用户取消的，打印告知
            UserOpEnableChange(true);//重置用户操作状态
            SetBtnRunTextToMainThread("执行");
            //Debug.WriteLine("子线程已经完成！");
            //线程清除（如果不放心，Abort方法能在目标线程中抛出一个ThreadAbortException异常从而导致目标线程的终止）
            //WorkThread.Abort();
        }

        /// <summary>
        /// The Button of Run.
        /// 点击执行按钮后应创建一个后台线程进行复杂任务处理，防止对UI所在的主线程造成卡顿。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void button_Run_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                if (MMCore.IsDFPath(textBox_workPath.Text) || GetSelectedIndexFromMainThread()==1 || GetSelectedIndexFromMainThread()==2)
                {
                    //开始工作，大部分界面置灰（用户不可操作）
                    UserOpEnableChange(false);

                    if (button_Run.Text == "执行" && WorkStatus == false)
                    {
                        WorkStatus = true;
                        button_Run.Text = "取消";
                        //创建后台线程实例来运行复杂任务
                        WorkThread = new Thread(ButtonRun) { IsBackground = true };
                        WorkThread.Start();
                        // 等待子线程完成↓
                        //WorkThread.Join();
                    }
                    else if (button_Run.Text == "取消" && WorkStatus == true)
                    {
                        WorkStop = true;
                    }
                }
                else
                {
                    label_Tips.Text = "工作目录错误！";
                    break;
                }
            }
        }

        /// <summary>
        /// 递归方式混肴文件夹内所有地图文件
        /// </summary>
        /// <param name="dirInfo"></param>
        void ObDirectoryRecursively(DirectoryInfo dirInfo)
        {
            foreach (DirectoryInfo newInfo in dirInfo.GetDirectories())
            {
                ObDirectoryRecursively(newInfo);
            }
            foreach (FileInfo newInfo in dirInfo.GetFiles("*.SC2Map"))
            {
                //ProcessingFile(newInfo.FullName);
                Obfuscator.Obfuscate(newInfo.FullName);
            }
        }
        void ObDirectoryRecursively(string dirPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            ObDirectoryRecursively(dirInfo);
        }

        /// <summary>
        /// 对.SC2Map地图文件进行混肴
        /// </summary>
        void SelectedFunc_0()
        {
            try
            {
                if (GetCheckTestStateFromMainThread() == false)
                {
                    ObDirectoryRecursively(GetWorkPathFromMainThread());
                }
                else 
                {
                    //实验版混淆
                    SelectedFunc_Test();
                }
            }
            catch (Exception ex)
            {
                //处理可能出现的异常，例如访问被拒绝等
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        /// <summary>
        /// 对Galaxy代码进行混肴
        /// </summary>
        void SelectedFunc_1()
        {
            if (GetCheckTestStateFromMainThread() == false)
            {
                Obfuscator obfuscator = new Obfuscator();
                obfuscator.script = GetCodeFromMainThread();
                SetCodeToMainThread(obfuscator.obfuscateScript());
            }
            else
            {
                //实验版混淆
                SelectedFunc_Test();
            }
        }

        /// <summary>
        /// 将Objects布置信息转Galaxy
        /// </summary>
        void SelectedFunc_2()
        {

        }

        /// <summary>
        /// 实验版混淆
        /// </summary>
        void SelectedFunc_Test()
        {
            string exclusionRulesPath = GetRulePathFromMainThread();
            //排除规则文件使用系统默认的情况：1）文件路径为空；2）文件路径虽不为空但后缀非.txt；3）文件路径非法。
            if (
                exclusionRulesPath == ""
                || !(Regex.IsMatch(exclusionRulesPath, @"^(.*)(\.txt)$"))
                || !MMCore.IsDFPath(exclusionRulesPath)
            )
            {
                //文本路径错误，重置为系统默认
                exclusionRulesPath = AppDomain.CurrentDomain.BaseDirectory + @"Rules/exclusion_rules.txt";
                SetRulePathToMainThread(exclusionRulesPath);
                //SetTipsToMainThread("排除规则文件路径错误，重置为系统默认！");
            }
            //去除代码里的注释和空行
            string mainCode = MMCore.RemoveEmptyLines(MMCore.RemoveComments(GetCodeFromMainThread()));
            // 定义正则表达式模式，匹配函数名
            string pattern = @"(?<=^|[^a-zA-Z_])[a-zA-Z_][\w]*(?=\s*\([^\)]*\)(\s+|\n|$))";
            MatchCollection matches = Regex.Matches(mainCode, pattern);
            CodeObfuscator obfuscator = new CodeObfuscator();
            //读取排除规则文本，添加混淆规则时会防止它们参与混淆（格式：每行一个函数名）
            obfuscator.LoadExclusionRules(exclusionRulesPath);
            //遍历全部函数名
            foreach (Match match in matches)
            {
                //Debug.WriteLine("Function Name: " + match.Value);
                //构建正则表达式，除了"排除规则文本"指定的函数名，让Lib、GAx3开头的函数名也避开混淆
                if (!Regex.IsMatch(match.Value, "^(Lib|lib|GAx3).*"))
                {
                    //添加函数名及混淆后名称到混淆规则（这过程会自动去重，也不会生成相同混淆名称）
                    obfuscator.AddReplacement(match.Value);
                }

            }
            //要解决：Lib_gf_A和gf_A，如后者加到混淆规则，前者也会被替换一部分，所以混淆前得检查函数名并对混淆规则修改：如第一遍混淆规则里的键名是按库名匹配的函数名的一部分则剔除该键
            foreach (Match match in matches)
            {
                if (Regex.IsMatch(match.Value, "^(Lib|lib|GAx3).*"))
                {
                    //混淆规则字典里的键名是match.Value的一部分则从字典里剔除该键
                    //检查match.Value是否包含字典中的任何键
                    foreach (var key in obfuscator.Replacements.Keys)
                    {
                        if (match.Value.Contains(key))
                        {
                            //如果match.Value包含字典中的键，则从字典中删除该键
                            obfuscator.Replacements.Remove(key);
                        }
                    }
                }
            }

            //第一遍混淆后的代码
            string obfuscatedCode = obfuscator.ObfuscateCode(mainCode);

            string temp = "";

            //第二遍混淆
            //对代码文本中的字符串进行混淆
            //MatchCollection matches2 = Regex.Matches(obfuscatedCode, @"""(.*?)""");
            MatchCollection matches2 = Regex.Matches(obfuscatedCode, @"""[^\\""]*""");
            foreach (Match match in matches2)
            {
                //匹配到的字符串的内容(.*?)放在match.Groups[1].Value，内容非空则进行添加规则
                if (match.Groups[1].Value != "")
                {
                    if (Regex.IsMatch(match.Groups[1].Value, @".*\\.*")) { Debug.WriteLine(match.Groups[1].Value); }
                    //构建正则表达式，带有以下指定字符的字符串会避开混淆
                    if (!Regex.IsMatch(match.Groups[1].Value, "(\\|bnet:)"))
                    {
                        //添加到混淆规则2（要替换的字符串为键，混淆成8进制或18进制后的字符串为值）
                        temp = MMCore.ConvertStringToHOMixed(match.Groups[1].Value, 0.7); //这里是内容的混淆
                        //Debug.WriteLine($"Found string: {match.Groups[1].Value}, Value: {temp}");
                        temp = "\"" + temp + "\""; //重新套上引号
                        //注意此处第二项规则的键要带""不能光内容字符作为键
                        obfuscator.AddReplacement2(match.Value, temp);
                    }
                }
            }
            obfuscatedCode = obfuscator.ObfuscateCode2(obfuscatedCode);

            //第三遍
            //正则表达式匹配void InitMap () { 到 }的内容
            //pattern = @"void\s*InitMap\s*\(\)\s*\{(.*\S)*\}";
            ////使用 RegexOptions.Multiline 选项来指定模式应在多个行上进行匹配，并使用 RegexOptions.Singleline 选项来指定模式应在单个连续字符串上进行匹配
            //RegexOptions options = RegexOptions.Multiline | RegexOptions.Singleline;
            //MatchCollection matches3 = Regex.Matches(obfuscatedCode, pattern, options);
            //foreach (Match match in matches3)
            //{
            //    if (match.Groups[1].Value != "")
            //    {
            //        temp = match.Groups[1].Value;
            //        Debug.WriteLine(temp);
            //    }
            //}
            //obfuscatedCode = Regex.Replace(obfuscatedCode, pattern, string.Empty, options);

            //植入LC4等其他功能（注：LC4卡顿检测功能来自岛风）
            if (GetCheckLC4StateFromMainThread() == true)
            {
                string sCHeadPath = AppDomain.CurrentDomain.BaseDirectory + @"Rules/SCHead";
                string sCEndPath = AppDomain.CurrentDomain.BaseDirectory + @"Rules/SCEnd";
                string sCHead = File.ReadAllText(sCHeadPath);
                string sCEnd = File.ReadAllText(sCEndPath);
                obfuscatedCode = sCHead + obfuscatedCode + sCEnd;
            }
            SetCodeToMainThread(obfuscatedCode);
        }

        void UserOpEnableChange(bool torf)
        {
            UserOpEnable = torf;
            //遍历全控件并获取类型（可跨线程不用回调），控件属性读写操作仍需要回调
            foreach (Control a in Controls)
            {
                if (a is Panel)
                {
                    Panel p = a as Panel;  //取出Panel
                    if (!torf)
                    {
                        // 改变Panel的颜色，执行时是灰色
                        SetPanelBackColorToMainThread(p, Color.Gray);
                    }
                    else
                    {
                        //不执行时是白色
                        SetPanelBackColorToMainThread(p, Color.Transparent);
                    }

                    foreach (Control c in p.Controls) //遍历面板中的每一个控件
                    {
                        //Debug.WriteLine(c.GetType().Name);
                        if (c.GetType().Name.Equals("TextBox"))
                        {
                            //禁用文本框
                            SetControlEnableToMainThread(c, torf);
                        }
                        if (c.GetType().Name.Equals("CheckBox"))
                        {
                            //禁用复选框
                            SetControlEnableToMainThread(c, torf);
                        }
                        if (c.GetType().Name.Equals("ComboBox"))
                        {
                            //禁用下拉框
                            SetControlEnableToMainThread(c, torf);
                        }
                        if (c.GetType().Name.Equals("Label"))
                        {
                            //禁用下拉框
                            SetControlEnableToMainThread(c, torf);
                        }
                        if (c.GetType().Name.Equals("Button"))
                        {
                            if (!c.Name.Equals("button_Run"))
                            {
                                //禁用除运行按钮外的其他按钮
                                SetControlEnableToMainThread(c, torf);
                            }
                        }

                    }
                }
            }
            //上面Controls未读到富格式文本框，单独设置
            SetControlEnableToMainThread(richTextBox_Code, torf);
        }

        /// <summary>
        /// 选择工作文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void button_selectWorkPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.ShowDialog();
                string path = fbd.SelectedPath;
                textBox_workPath.Text = path;
            }
        }

        void button_selectRulePath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text files (*.txt)|*.txt";
                DialogResult result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string path = ofd.FileName;
                    textBox_rulePath.Text = path;
                }
                else if (result == DialogResult.Cancel)
                {
                    //MessageBox.Show("用户取消了文件选择！");
                }
            }
        }

        void LoadContentFromFile_Galaxy()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "SC2Map Files (*.SC2Map)|*.SC2Map|Galaxy Files (*.galaxy)|*.galaxy|All Files (*.*)|*.*";
                DialogResult result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string script;
                    string path = ofd.FileName;
                    string fileExtension = Path.GetExtension(path).ToLower(); // 获取文件扩展名并转换为小写
                    switch (fileExtension)
                    {
                        case ".sc2map":
                            // 处理.SC2Map文件
                            using (MpqArchive mpqArchive = new MpqArchive(path))
                            {
                                //打开MPQ格式文件中的MapScript.galaxy
                                using (StreamReader streamReader = new StreamReader(new BufferedStream(mpqArchive.OpenFile("MapScript.galaxy"))))
                                {
                                    //字节转字符串
                                    script = streamReader.ReadToEnd();
                                    //脚本内容显示到窗口
                                    richTextBox_Code.Text = script;
                                }
                            }
                            break;

                        case ".galaxy":
                            // 处理.galaxy文件
                            byte[] binaryData;
                            try
                            {
                                binaryData = File.ReadAllBytes(path);
                                script = Encoding.UTF8.GetString(binaryData);
                                //内容显示到窗口
                                richTextBox_Code.Text = script;
                            }
                            catch (FileNotFoundException)
                            {
                                label_Tips.Text = "文件未找到";
                            }
                            catch (Exception ex)
                            {
                                label_Tips.Text = $"发生错误: {ex.Message}";
                            }
                            break;

                        default:
                            // 未知文件类型
                            MessageBox.Show("必须是*.SC2Map或*.galaxy文件");
                            break;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    //MessageBox.Show("用户取消了文件选择！");
                }
            }
        }

        void LoadContentFromFile_Objects()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "SC2Map Files (*.SC2Map)|*.SC2Map|Object Files (Objects)|Objects|All Files (*.*)|*.*";
                DialogResult result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string str;
                    string path = ofd.FileName;
                    string fileExtension = Path.GetExtension(path).ToLower(); // 获取文件扩展名并转换为小写
                    switch (fileExtension)
                    {
                        case ".sc2map":
                            // 处理.SC2Map文件
                            using (MpqArchive mpqArchive = new MpqArchive(path))
                            {
                                //打开MPQ格式文件中的MapScript.galaxy
                                using (StreamReader streamReader = new StreamReader(new BufferedStream(mpqArchive.OpenFile("Objects"))))
                                {
                                    //字节转字符串
                                    str = streamReader.ReadToEnd();
                                    //脚本内容显示到窗口
                                    richTextBox_Code.Text = str;
                                }
                            }
                            break;

                        case "Objects":
                            // 处理.galaxy文件
                            byte[] binaryData;
                            try
                            {
                                binaryData = File.ReadAllBytes(path);
                                str = Encoding.UTF8.GetString(binaryData);
                                //内容显示到窗口
                                richTextBox_Code.Text = str;
                            }
                            catch (FileNotFoundException)
                            {
                                label_Tips.Text = "文件未找到";
                            }
                            catch (Exception ex)
                            {
                                label_Tips.Text = $"发生错误: {ex.Message}";
                            }
                            break;
                        default:
                            // 未知文件类型
                            MessageBox.Show("必须是*.SC2Map或名为Objects的文件");
                            break;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    //MessageBox.Show("用户取消了文件选择！");
                }
            }
        }

        void richTextBox_Code_TextChanged(object sender, EventArgs e)
        {
            //文本改变时不需要写任何动作，但也可用来检查关键字或执行命令
        }

        void comboBox_SelectFunc_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GetSelectedIndexFromMainThread())
            {
                case -1:
                    SetTipsToMainThread("功能未选择！");
                    break;
                case 0:
                    SetTipsToMainThread("（设置处理目录）执行将批处理混肴文件夹内所有.SC2Map地图文件");
                    panel1.Visible = true;
                    panel_Bottom.Visible = false;
                    checkBox_LC4.Enabled = true;
                    checkBox_Test.Enabled = true;
                    break;
                case 1:
                    SetTipsToMainThread("（左下文本）填入Galaxy代码，执行将进行混肴");
                    button_LoadContentFromFile.Text = "读取Map里的代码";
                    panel1.Visible = false;
                    panel_Bottom.Visible = true;
                    checkBox_LC4.Enabled = true;
                    checkBox_Test.Enabled = true;
                    break;
                case 2:
                    SetTipsToMainThread("（左下文本）填入Objects内容，执行将单位、装饰物布置信息转Galaxy动作");
                    button_LoadContentFromFile.Text = "读取Map里的Objects";
                    panel1.Visible = false;
                    panel_Bottom.Visible = true;
                    checkBox_LC4.Enabled = false;
                    checkBox_Test.Enabled = false;
                    break;
                default:
                    SetTipsToMainThread("功能无效！");
                    break;
            }
        }

        void Form1_Load(object sender, EventArgs e)
        {
            comboBox_SelectFunc.Items.Add("对.SC2Map地图文件进行混肴");
            comboBox_SelectFunc.Items.Add("对Galaxy代码进行混肴");
            comboBox_SelectFunc.Items.Add("将Objects布置信息转Galaxy（演算体的碰撞将失效）");
            comboBox_SelectFunc.SelectedIndex = 0;
        }

        void button_LoadContentFromFile_Click(object sender, EventArgs e)
        {
            switch (GetSelectedIndexFromMainThread())
            {
                case 1:
                    LoadContentFromFile_Galaxy();
                    break;
                case 2:
                    LoadContentFromFile_Objects();
                    break;
                default:
                    break;
            }
        }
    }
}
