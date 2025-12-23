using MetalMaxSystem;
using StormLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GalaxyObfuscator
{
    public partial class Form1 : Form
    {
        StringBuilder globalSB = new StringBuilder();
        StringBuilder globalSB2 = new StringBuilder();

        static bool _userOpEnable = true;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        /// <summary>
        /// 用户操作许可
        /// </summary>
        public static bool UserOpEnable { get => _userOpEnable; set => _userOpEnable = value; }

        static bool _workStatus = false;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        /// <summary>
        /// 工作状态
        /// </summary>
        public static bool WorkStatus { get => _workStatus; set => _workStatus = value; }

        static bool _workStop = false;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        /// <summary>
        /// 打断工作用的状态变量
        /// </summary>
        public static bool WorkStop { get => _workStop; set => _workStop = value; }

        static Thread _workThread;
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

        public bool GetCheckEventStateFromMainThread()
        {
            if (checkBox_Event.InvokeRequired)
            {
                return (bool)checkBox_Event.Invoke(new Func<bool>(GetCheckEventStateFromMainThread));
            }
            else
            {
                return checkBox_Event.Checked;
            }
        }

        public bool GetCheckLC4StateFromMainThread()
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

        string GetListFromMainThread()
        {
            if (richTextBox_Code.InvokeRequired)
            {
                return (string)richTextBox_List.Invoke(new Func<string>(GetListFromMainThread));
            }
            else
            {
                return richTextBox_List.Text;
            }
        }
        /// <summary>
        /// 调用Invoke方法将code发送到主线程代码文本框
        /// </summary>
        /// <param name="code"></param>
        public void SetCodeToMainThread(string code)
        {
            Invoke((MethodInvoker)delegate ()
            {
                richTextBox_Code.Text = code;
            });
        }
        /// <summary>
        /// 调用Invoke方法将list发送到主线程列表文本框
        /// </summary>
        /// <param name="list"></param>
        public void SetListToMainThread(string list)
        {
            Invoke((MethodInvoker)delegate ()
            {
                richTextBox_List.Text = list;
            });
        }

        public int GetSelectedIndexFromMainThread()
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
            Obfuscator.mutiCount = 0;//运行前重置计数
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
                    case 3:
                        SelectedFunc_3();
                        break;
                    case 4:
                        SelectedFunc_4();
                        break;
                    case 5:
                        SelectedFunc_5();
                        SetCodeToMainThread(globalSB.ToString());
                        SetListToMainThread(globalSB2.ToString());
                        break;
                    default:
                        SetTipsToMainThread("功能无效！");
                        break;
                }
                stopwatch.Stop();
                //Debug.WriteLine(stopwatch.Elapsed.ToString());
                SetStatisticsToMainThread(" 时耗：" + stopwatch.Elapsed.ToString());
                //MessageBox.Show("Completed!");//弹窗提示
            }
            //放弃了线程注销做法，程序将始终运行至此，可以知道是用户中断还是正常运行结束
            WorkStatus = false;//重置工作状态
            if (WorkStop) { SetStatisticsToMainThread("用户取消！"); }
            WorkStop = false;//重置_workStop状态，如果是用户取消的，打印告知
            UserOpEnableChange(true);//重置用户操作状态
            OpStateReset();
            SetBtnRunTextToMainThread("执行");
            //Debug.WriteLine("子线程已经完成！");
            //线程清除（如果不放心，Abort方法能在目标线程中抛出一个ThreadAbortException异常从而导致目标线程的终止）
            //WorkThread.Abort();
        }

        /// <summary>
        /// 刷新用户所选功能下的控件状态
        /// </summary>
        void OpStateReset()
        {
            switch (GetSelectedIndexFromMainThread())
            {
                case -1:
                    SetTipsToMainThread("功能未选择！");
                    break;
                case 0:
                    SetTipsToMainThread("（选择处理目录）执行将批处理混淆文件夹内所有.SC2Map地图文件");
                    SetControlEnableToMainThread(checkBox_LC4, true);
                    SetControlEnableToMainThread(checkBox_Test, true);
                    break;
                case 1:
                    SetTipsToMainThread("（左下文本）填入Galaxy代码，执行将进行混淆");
                    SetControlEnableToMainThread(checkBox_LC4, true);
                    SetControlEnableToMainThread(checkBox_Test, true);
                    break;
                case 2:
                    SetTipsToMainThread("（左下文本）填入Objects内容，执行将单位装饰等地形布置信息转Galaxy");
                    SetControlEnableToMainThread(checkBox_LC4, false);
                    SetControlEnableToMainThread(checkBox_Test, false);
                    break;
                case 3:
                    SetTipsToMainThread("（左下文本）填入Galaxy代码，执行将尝试中文转换");
                    SetControlEnableToMainThread(checkBox_LC4, false);
                    SetControlEnableToMainThread(checkBox_Test, false);
                    break;
                case 4:
                    SetTipsToMainThread("左下文本填代码执行将UnitCreate转地形信息格式ObjectUnit，右下文本可填整数干预ID");
                    SetControlEnableToMainThread(checkBox_LC4, false);
                    SetControlEnableToMainThread(checkBox_Test, false);
                    break;
                case 5:
                    SetTipsToMainThread("（选择处理目录）执行将批扫描虫出封锁线N档案并生成恢复用文本");
                    SetControlEnableToMainThread(checkBox_LC4, false);
                    SetControlEnableToMainThread(checkBox_Test, false);
                    break;
                case 6:
                    SetTipsToMainThread("读取星际录像或其他二进制文件");
                    SetControlEnableToMainThread(checkBox_LC4, false);
                    SetControlEnableToMainThread(checkBox_Test, false);
                    break;
                default:
                    SetTipsToMainThread("功能无效！");
                    break;
            }
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
                if (MMCore.IsDFPath(textBox_workPath.Text) || GetSelectedIndexFromMainThread() == 1 || GetSelectedIndexFromMainThread() == 2 || GetSelectedIndexFromMainThread() == 3 || GetSelectedIndexFromMainThread() == 4)
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
        /// 递归方式混淆文件夹内所有地图文件
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
                Obfuscator.Obfuscate(newInfo.FullName);
            }
        }
        void ObDirectoryRecursively(string dirPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            ObDirectoryRecursively(dirInfo);
        }

        /// <summary>
        /// 递归方式处理文件夹内所有虫出封锁线N档
        /// </summary>
        /// <param name="dirInfo"></param>
        void NBankDirectoryRecursively(DirectoryInfo dirInfo)
        {
            foreach (DirectoryInfo newInfo in dirInfo.GetDirectories())
            {
                NBankDirectoryRecursively(newInfo);
            }
            foreach (FileInfo newInfo in dirInfo.GetFiles("*.SC2Bank"))
            {
                HandleUBankFile(newInfo.FullName);
            }
        }
        void NBankDirectoryRecursively(string dirPath)
        {
            globalSB.Clear(); globalSB2.Clear();
            globalSB.AppendLine("//█注：以下信息用于战网-LoadBank指令恢复档案█");
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            NBankDirectoryRecursively(dirInfo);
        }

        void HandleUBankFile(string filePath)
        {
            string sectionName, tempStringValue;
            XElement tempElement;
            try
            {
                //从newInfo.FullName 即 参数filePath 读取XML文件全部文本内容以便交给XDocument.Parse
                string xmlContent = File.ReadAllText(filePath);
                //使用XDocument.Parse来解析XML字符串
                XDocument doc = XDocument.Parse(xmlContent);
                //遍历所有的Section元素
                foreach (var section in doc.Descendants("Section"))
                {
                    //提取name属性值
                    sectionName = section.Attribute("name")?.Value ?? "";
                    //根据Section的name属性名称处理不同的KeyValue
                    switch (sectionName)
                    {
                        case "Check":
                            //处理Check节的Key,提取JuBing键值
                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "JuBing");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("string")?.Value;
                            if (string.IsNullOrEmpty(tempStringValue))
                            {
                                tempStringValue = "5-S2-1-请补充句柄";
                            }
                            else
                            {
                                globalSB2.AppendLine(tempStringValue);
                            }
                            globalSB.AppendLine($"if (lp_id == \"{tempStringValue}\") {{");
                            break;
                        case "Main":
                            //处理Main节的Key
                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "Tz");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("string")?.Value;
                            if (string.IsNullOrEmpty(tempStringValue))
                            {
                                tempStringValue = "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
                            }
                            globalSB.AppendLine($"    if (lp_type == \"TZ\") {{ lv_torf = \"{tempStringValue}\";}}");

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "TzEx");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("string")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"TZEX\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "Bd");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("string")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"BD\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "EX");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("string")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"EX\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "TF");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("string")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"TF\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "AWin");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("int")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"胜场\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "PWin");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("int")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"有效胜场\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "AGameSum");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("int")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"场次\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "PGameSum");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("int")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"有效场次\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "Exploit");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("int")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"功勋\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "WSZGJS");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("int")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"王兽最高击杀\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "WSQHLV");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("int")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"王兽挑战等级\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "JF");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("int")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"积分\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "Money");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("int")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"资金\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "Ws");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("int")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"瓦斯\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "En");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("int")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"能源\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }

                            tempElement = section.Descendants("Key").FirstOrDefault(x => x.Attribute("name")?.Value == "Rare");
                            tempStringValue = tempElement?.Element("Value")?.Attribute("int")?.Value;
                            if (!string.IsNullOrEmpty(tempStringValue))
                            {
                                globalSB.AppendLine($"    else if (lp_type == \"稀金\"){{ lv_torf = \"{tempStringValue}\";}}");
                            }
                            globalSB.AppendLine($"}}");
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (IOException ex)
            {
                //处理文件读取过程中的异常
                Debug.WriteLine("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// 对.SC2Map地图文件进行混淆
        /// </summary>
        void SelectedFunc_0()
        {
            try
            {
                if (GetCheckTestStateFromMainThread() == false)
                {//非实验版混淆
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
        /// 对Galaxy代码进行混淆
        /// </summary>
        void SelectedFunc_1()
        {
            if (GetCheckTestStateFromMainThread() == false)
            {
                string workDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Obfuscator obfuscator = new Obfuscator(workDirectory + @"/SyntaxErrorException.txt");
                obfuscator.script = GetCodeFromMainThread();
                SetCodeToMainThread(obfuscator.obfuscateScript());
                //↓如果勾选了打印报告
                MMCore.WriteLine(workDirectory + @"/代码混淆报告.txt", "████████████████████████████████████████████" + "\r\n" + "", true, true, false);//尾行留空
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
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                //使用XDocument.Parse来解析XML字符串
                XDocument doc = XDocument.Parse(GetCodeFromMainThread());

                stringBuilder.AppendLine("█注：地编布置装饰物时碰撞写死二进制文件，转Galaxy布置可用透明单位移动碰撞来补█");
                stringBuilder.AppendLine("█↓ObjectUnit.UnitCreate.Start↓█单位.创建");
                // 遍历所有的ObjectUnit元素
                foreach (var obj in doc.Descendants("ObjectUnit"))
                {
                    // 提取必要的属性值
                    // int id = int.Parse(obj.Attribute("Id").Value);
                    string position = obj.Attribute("Position").Value;
                    string type = obj.Attribute("UnitType").Value;
                    string rotation = obj.Attribute("Rotation")?.Value;
                    double ro = 0.0;
                    if (rotation != null)
                    {
                        double.TryParse(rotation, out ro);
                        //使用 Math.Round 进行四舍五入，然后转换为 int 类型来保留整数部分
                        rotation = ((int)Math.Round(RadianToDegree(ro)) - 90).ToString();
                    }
                    else
                    {
                        rotation = "270";
                    }

                    // 分割Position属性以获取X和Y坐标
                    string[] positionParts = position.Split(',');
                    string x = positionParts[0];
                    string y = positionParts[1];

                    // 替换Type属性中的"CCData_Blocker"为"CCData_UnitBlocker"
                    // string unitType = type.Replace("CCData_FKWall", "CCData_FK4Wall");

                    // 检查unitType是否包含"CCData_UnitBlocker"
                    // if (unitType.Contains("CCData_FK4Wall"))
                    // {
                    //     // 如果包含，则输出新格式的字符串
                    //     stringBuilder.AppendLine($"UnitCreate(1, \"{unitType}\", 0, gv_playerID, Point({x}, {y}), rotation);");
                    // }

                    //输出新格式的字符串
                    stringBuilder.AppendLine(
                        $"UnitCreate(1, \"{type}\", 0, gv_playerID, Point({x}, {y}), {rotation});"
                    );
                }
                stringBuilder.AppendLine("█↑ObjectUnit.UnitCreate.End↑█" + "\r\n");

                stringBuilder.AppendLine("█↓ObjectDoodad.CreateActorAtPoint.Start↓█装饰物.创建演算体");
                //遍历所有的ObjectDoodad元素
                foreach (var obj in doc.Descendants("ObjectDoodad"))
                {
                    // 提取必要的属性值
                    string position = obj.Attribute("Position").Value;
                    string type = obj.Attribute("Type").Value;
                    string rotation = obj.Attribute("Rotation")?.Value;
                    double ro = 0.0;

                    // 分割Position属性以获取X和Y坐标
                    string[] positionParts = position.Split(',');
                    string x = positionParts[0];
                    string y = positionParts[1];

                    // 输出新格式的字符串
                    stringBuilder.AppendLine(
                        $"libNtve_gf_CreateActorAtPoint (\"{type}\", Point({x}, {y}));"
                    );
                    stringBuilder.AppendLine(
                        $"libBC0D3AAD_gf_HD_RegA_Simple(libNtve_gf_ActorLastCreated(), \"gv_DoodedObjGroup\");\\\\加入演算体组（需MM_功能库）"
                    );
                    if (rotation != null)
                    {
                        double.TryParse(rotation, out ro);
                        //使用Math.Round进行四舍五入，然后转换为int类型来保留整数部分
                        rotation = ((int)Math.Round(RadianToDegree(ro)) - 90).ToString();
                        stringBuilder.AppendLine(
                            $"libNtve_gf_MakeModelFaceAngle(libNtve_gf_ActorLastCreated(), {rotation});"
                        );
                    }
                }
                stringBuilder.AppendLine("█↑ObjectDoodad.CreateActorAtPoint.End↑█" + "\r\n");

                stringBuilder.AppendLine("█↓ObjectDoodad.CreateModelAtPoint.Start↓█装饰物.创建模型");
                // 遍历所有的ObjectDoodad元素
                foreach (var obj in doc.Descendants("ObjectDoodad"))
                {
                    // 提取必要的属性值
                    string position = obj.Attribute("Position").Value;
                    string type = obj.Attribute("Type").Value;
                    string rotation = obj.Attribute("Rotation")?.Value;
                    double ro = 0.0;

                    // 分割Position属性以获取X和Y坐标
                    string[] positionParts = position.Split(',');
                    string x = positionParts[0];
                    string y = positionParts[1];

                    // 输出新格式的字符串
                    Console.WriteLine(
                        $"libNtve_gf_CreateModelAtPoint(\"{type}\", Point({x}, {y}));"
                    );
                    Console.WriteLine(
                        $"libBC0D3AAD_gf_HD_RegA_Simple(libNtve_gf_ActorLastCreated(), \"gv_DoodedObjGroup\");"
                    );
                    if (rotation != null)
                    {
                        double.TryParse(rotation, out ro);
                        //使用 Math.Round 进行四舍五入，然后转换为 int 类型来保留整数部分
                        rotation = ((int)Math.Round(RadianToDegree(ro)) - 90).ToString();
                        Console.WriteLine(
                            $"libNtve_gf_MakeModelFaceAngle(libNtve_gf_ActorLastCreated(), {rotation});"
                        );
                    }
                }
                stringBuilder.AppendLine("█↑ObjectDoodad.CreateModelAtPoint.End↑█" + "\r\n");

                stringBuilder.AppendLine("█↓ObjectPoint.PathAddNoFlyZone.Start↓█点.路径创建禁飞区");
                // 遍历所有的ObjectDoodad元素
                foreach (var obj in doc.Descendants("ObjectPoint"))
                {
                    // 提取必要的属性值
                    // int id = int.Parse(doodad.Attribute("Id").Value);
                    string position = obj.Attribute("Position").Value;
                    string type = obj.Attribute("Type").Value;

                    // 分割Position属性以获取X和Y坐标
                    string[] positionParts = position.Split(',');
                    string x = positionParts[0];
                    string y = positionParts[1];

                    // 输出新格式的字符串
                    if (type == "NoFlyZone")
                    {
                        stringBuilder.AppendLine($"PathAddNoFlyZone(Point({x}, {y}), 2.0, 2.0);\\\\后俩参数是范围大小");
                    }
                }
                stringBuilder.AppendLine("█↑ObjectPoint.PathAddNoFlyZone.End↑█" + "\r\n");

                SetCodeToMainThread(stringBuilder.ToString());
            }
            catch (IOException ex)
            {
                //处理文件读取过程中的异常
                Debug.WriteLine("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// 对代码中的乱码进行中文转换
        /// </summary>
        void SelectedFunc_3()
        {
            string workDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string pattern = @"([A-Za-z][A-Za-z0-9]*[0-9][A-Za-z0-9]*)";
            string code = GetCodeFromMainThread();
            //存储替换记录的列表
            var replacements = new List<Tuple<string, string>>();
            //执行正则替换
            string outputText = Regex.Replace(code, pattern, match =>
            {
                string original = match.Value;
                string transformed = MMCore.HexStringToChineseCharacter(original);
                if (transformed != "")
                {
                    replacements.Add(Tuple.Create(original, transformed));
                }
                else
                {
                    transformed = original;
                }
                return transformed;
            });
            SetCodeToMainThread(outputText);
            //打印报告
            foreach (var replacement in replacements)
            {
                MMCore.WriteLine($"替换前: {replacement.Item1} \t 替换后: {replacement.Item2}");
            }
            MMCore.WriteLine(workDirectory + @"/中文转换报告.txt", "████████████████████████████████████████████" + "\r\n" + "", true, true, false);//尾行留空
        }

        /// <summary>
        /// 将代码中的UnitCreate转回地形信息格式（ObjectUnit）
        /// </summary>
        void SelectedFunc_4()
        {
            string workDirectory = AppDomain.CurrentDomain.BaseDirectory;
            int idCounter = 2001;
            string code = GetCodeFromMainThread();
            string list = GetListFromMainThread();
            if (list != null && int.TryParse(list.ToString(), out int parsedValue))
            {
                idCounter = parsedValue;
            }
            string outputText = UnitConverter.Go(code, idCounter);
            SetCodeToMainThread(outputText);
            MMCore.WriteLine(outputText);
            MMCore.WriteLine(workDirectory + @"/转换报告.txt", "████████████████████████████████████████████" + "\r\n" + "", true, true, false);//尾行留空
        }

        /// <summary>
        /// 执行将批扫描虫出封锁线U、N档案并生成恢复用文本
        /// </summary>
        void SelectedFunc_5()
        {
            try
            {
                NBankDirectoryRecursively(GetWorkPathFromMainThread());
            }
            catch (Exception ex)
            {
                //处理可能出现的异常，例如访问被拒绝等
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        /// <summary>
        /// 弧度转度数
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double RadianToDegree(double radian)
        {
            return radian * (180.0 / Math.PI);
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
                obfuscatedCode = sCHead + "\r\n" + obfuscatedCode + "\r\n" + sCEnd;
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
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    textBox_workPath.Text = fbd.SelectedPath;
                }
                else if (result == DialogResult.Cancel)
                {
                    //MessageBox.Show("用户取消则什么也不做，防止目录刷回空");
                }
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

        // 检查字符串是否可打印
        static bool IsPrintable(string text)
        {
            return text.All(c => c >= 32 && c <= 126);
        }
        /// <summary>
        /// 提取录像信息文本中所有句柄然后存放到列表中返回
        /// </summary>
        /// <param name="replayContent"></param>
        /// <returns></returns>
        public List<string> ExtractAllReplayIds(string replayContent)
        {
            var replayIds = new List<string>();

            // 匹配所有符合格式的录像ID：数字-字母数字-数字-数字
            string pattern = @"\d+-[A-Za-z0-9]+-\d+-\d+";
            MatchCollection matches = Regex.Matches(replayContent, pattern);

            foreach (Match match in matches)
            {
                if (IsValidReplayId(match.Value))
                {
                    replayIds.Add(match.Value);
                }
            }

            return replayIds.Distinct().ToList();
        }
        /// <summary>
        /// 符合格式的玩家ID验证
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsValidReplayId(string id)
        {
            // 验证ID格式：例如 "5-S2-1-3372089"
            string[] parts = id.Split('-');
            return parts.Length == 4 &&
                   int.TryParse(parts[0], out _) &&
                   int.TryParse(parts[2], out _) &&
                   int.TryParse(parts[3], out _);
        }

        /// <summary>
        /// 提取指定ID的录像数据.并生成对应的Galaxy代码段.
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="targetReplayId"></param>
        /// <returns></returns>
        public string GenerateCodeFromReplay(string fileContent, string targetReplayId)
        {
            //提取指定ID的数据
            var replayData = ExtractReplayData(fileContent, targetReplayId);

            //生成代码
            StringBuilder codeBuilder = new StringBuilder();
            codeBuilder.AppendLine($"if (lp_id == \"{targetReplayId}\") {{");

            bool firstCondition = true;
            foreach (var data in replayData)
            {
                if (firstCondition)
                {
                    codeBuilder.AppendLine($"    if (lp_type == \"{data.Key}\") {{ lv_torf = \"{data.Value}\";}}");
                    firstCondition = false;
                }
                else
                {
                    codeBuilder.AppendLine($"    else if (lp_type == \"{data.Key}\"){{ lv_torf = \"{data.Value}\";}}");
                }
            }

            codeBuilder.AppendLine("}");
            return codeBuilder.ToString();
        }

        public void ProcessReplayFile(string inputFilePath, string outputFilePath, string replayId)
        {
            try
            {
                string generatedCode = GenerateCodeFromReplay(inputFilePath, replayId);
                File.WriteAllText(outputFilePath, generatedCode);
                Console.WriteLine("生成成功！");
            }
            catch (Exception ex)
            {
                label_Tips.Text = $"发生错误: {ex.Message}";
            }
        }

        Dictionary<string, string> ExtractReplayData(string replayContent, string replayId)
        {
            var result = new Dictionary<string, string>();

            // 定义要提取的数据类型及其对应的关键词
            var dataTypes = new Dictionary<string, string[]>
        {
            { "TZ", new[] { "TzValue" } },
            { "TZEX", new[] { "TzEx" } },
            { "BD", new[] { "Bd" } },
            { "EX", new[] { "EX" } },
            { "TF", new[] { "TF" } },
            { "胜场", new[] { "Win", "AWin" } },
            { "有效胜场", new[] { "PWin" } },
            { "场次", new[] { "GameSum" } },
            { "有效场次", new[] { "PGameSum" } },
            { "功勋", new[] { "Exploit" } },
            { "王兽最高击杀", new[] { "WSZGJS" } },
            { "王兽挑战等级", new[] { "WSQHLV" } },
            { "积分", new[] { "JF" } },
            { "资金", new[] { "Money" } },
            { "瓦斯", new[] { "Ws" } },
            { "能源", new[] { "En" } },
            { "稀金", new[] { "Rare" } }
        };

            foreach (var dataType in dataTypes)
            {
                string value = ExtractValueByKeywords(replayContent, dataType.Value);
                if (!string.IsNullOrEmpty(value))
                {
                    result[dataType.Key] = value;
                }
            }

            return result;
        }

        /// <summary>
        /// 根据所提供的关键词从指定内容中提取出相应的值.
        /// Extracts a value from the specified content based on the provided keywords.
        /// </summary>
        /// <remarks>The method searches the content for the first occurrence of any keyword followed by a
        /// numeric  value (e.g., "123") or a binary value (e.g., "101"). The search is case-sensitive and stops at the 
        /// first successful match.</remarks>
        /// <param name="content">The input string to search for a value matching the keywords.</param>
        /// <param name="keywords">An array of keywords used to locate the value. Each keyword is matched against the content,  followed by a
        /// numeric or binary value.</param>
        /// <returns>The first value found in the content that matches a keyword followed by a numeric or binary value;  or <see
        /// langword="null"/> if no match is found.</returns>
        string ExtractValueByKeywords(string content, string[] keywords)
        {
            foreach (string keyword in keywords)
            {
                // 匹配模式：关键词 + 数字/二进制值
                string pattern = keyword + @"(\d+|[01]+)";
                Match match = Regex.Match(content, pattern);

                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 读取SC2Replay或其他二进制文件内容到代码编辑区（过滤输出可打印字符）
        /// </summary>
        void LoadContentFromFile_Read()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "SC2Replay Files (*.SC2Replay)|*.SC2Replay|Files (replay.game.events)|replay.game.events|All Files (*.*)|*.*";
                DialogResult result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string str; StringBuilder sb = new StringBuilder();
                    string path = ofd.FileName; //文件完整路径
                    string fileExtension = Path.GetExtension(path).ToLower(); // 获取文件扩展名并转换为小写
                    switch (fileExtension)
                    {
                        case ".sc2replay":
                            // 处理MPQ文件
                            using (MpqArchive mpqArchive = new MpqArchive(path))
                            {
                                //打开MPQ格式文件中的指定名称文件
                                using (StreamReader streamReader = new StreamReader(new BufferedStream(mpqArchive.OpenFile("replay.game.events"))))
                                {
                                    //字节转字符串
                                    str = streamReader.ReadToEnd();
                                    //过滤出可打印字符
                                    str = new string(str.Where(c => c >= 32 && c <= 126).ToArray());
                                    //脚本内容显示到窗口
                                    richTextBox_Code.Text = str;
                                }
                            }
                            break;
                        default:
                            //处理二进制文件
                            byte[] binaryData;
                            try
                            {
                                binaryData = File.ReadAllBytes(path);
                                //过滤出字符
                                string filteredText = new string(binaryData.Where(b => b >= 32 && b <= 126).Select(b => (char)b).ToArray());
                                //内容显示到窗口
                                richTextBox_Code.Text = filteredText;
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
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    //MessageBox.Show("用户取消了文件选择！");
                }
            }
        }

        /// <summary>
        /// 读取SC2Replay或replay.game.events文件内容到代码编辑区并自动执行转换
        /// </summary>
        void LoadContentFromFile_Replay()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "SC2Replay Files (*.SC2Replay)|*.SC2Replay|Files (replay.game.events)|replay.game.events|All Files (*.*)|*.*";
                DialogResult result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string str; StringBuilder sb = new StringBuilder();
                    string path = ofd.FileName; //文件完整路径
                    string fileExtension = Path.GetExtension(path).ToLower(); // 获取文件扩展名并转换为小写
                    switch (fileExtension)
                    {
                        case ".sc2replay":
                            // 处理MPQ文件
                            using (MpqArchive mpqArchive = new MpqArchive(path))
                            {
                                //打开MPQ格式文件中的指定名称文件
                                using (StreamReader streamReader = new StreamReader(new BufferedStream(mpqArchive.OpenFile("replay.game.events"))))
                                {
                                    //字节转字符串
                                    str = streamReader.ReadToEnd();
                                    str = new string(str.Where(c => c >= 32 && c <= 126).ToArray());
                                    //提取录像中的所有玩家ID
                                    List<string> replayIds = ExtractAllReplayIds(str);
                                    //遍历所有玩家ID
                                    foreach (string replayId in replayIds)
                                    {
                                        sb.AppendLine(GenerateCodeFromReplay(str, replayId));
                                    }
                                    //脚本内容显示到窗口
                                    richTextBox_Code.Text = sb.ToString();
                                }
                            }
                            break;

                        case ".events":
                            //处理二进制文件
                            byte[] binaryData;
                            try
                            {
                                binaryData = File.ReadAllBytes(path);
                                //过滤出字符
                                string filteredText = new string(binaryData.Where(b => b >= 32 && b <= 126).Select(b => (char)b).ToArray());
                                //提取录像中的所有玩家ID
                                List<string> replayIds = ExtractAllReplayIds(filteredText);
                                //遍历所有玩家ID
                                foreach (string replayId in replayIds)
                                {
                                    sb.AppendLine(GenerateCodeFromReplay(filteredText, replayId));
                                }
                                //内容显示到窗口
                                richTextBox_Code.Text = sb.ToString();
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
                            MessageBox.Show("必须是*.SC2Replay或名为replay.game.events的文件");
                            break;
                    }
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

                        case "":
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
                    SetTipsToMainThread("（选择处理目录）执行将批处理混淆文件夹内所有.SC2Map地图文件");
                    panel1.Visible = true;
                    panel_Bottom.Visible = false;
                    checkBox_LC4.Enabled = true;
                    checkBox_Test.Enabled = false;
                    checkBox_Event.Enabled = true;
                    break;
                case 1:
                    SetTipsToMainThread("（左下文本）填入Galaxy代码，执行将进行混淆");
                    button_LoadContentFromFile.Text = "读取Map里的代码";
                    panel1.Visible = false;
                    panel_Bottom.Visible = true;
                    checkBox_LC4.Enabled = true;
                    checkBox_Test.Enabled = false;
                    checkBox_Event.Enabled = true;
                    break;
                case 2:
                    SetTipsToMainThread("（左下文本）填入Objects内容，执行将单位装饰等地形布置信息转Galaxy");
                    button_LoadContentFromFile.Text = "读取Map里的Objects";
                    panel1.Visible = false;
                    panel_Bottom.Visible = true;
                    checkBox_LC4.Enabled = false;
                    checkBox_Test.Enabled = false;
                    checkBox_Event.Enabled = false;
                    break;
                case 3:
                    SetTipsToMainThread("（左下文本）填入Galaxy代码，执行将尝试中文转换");
                    button_LoadContentFromFile.Text = "读取Map里的代码";
                    panel1.Visible = false;
                    panel_Bottom.Visible = true;
                    checkBox_LC4.Enabled = false;
                    checkBox_Test.Enabled = false;
                    checkBox_Event.Enabled = false;
                    break;
                case 4:
                    SetTipsToMainThread("左下文本填代码执行将UnitCreate转地形信息格式ObjectUnit，右下文本可填整数干预ID");
                    button_LoadContentFromFile.Text = "读取Map里的代码";
                    panel1.Visible = false;
                    panel_Bottom.Visible = true;
                    checkBox_LC4.Enabled = false;
                    checkBox_Test.Enabled = false;
                    checkBox_Event.Enabled = false;
                    break;
                case 5:
                    SetTipsToMainThread("（选择处理目录）执行将批扫描虫出封锁线N档案并生成恢复用文本");
                    button_LoadContentFromFile.Text = "处理录像信息";
                    panel1.Visible = true;
                    panel_Bottom.Visible = true;
                    checkBox_LC4.Enabled = false;
                    checkBox_Test.Enabled = false;
                    checkBox_Event.Enabled = false;
                    break;
                case 6:
                    SetTipsToMainThread("读取星际录像或其他二进制文件");
                    button_LoadContentFromFile.Text = "读取信息";
                    panel1.Visible = false;
                    panel_Bottom.Visible = true;
                    checkBox_LC4.Enabled = false;
                    checkBox_Test.Enabled = false;
                    checkBox_Event.Enabled = false;
                    break;
                default:
                    SetTipsToMainThread("功能无效！");
                    break;
            }
        }

        void Form1_Load(object sender, EventArgs e)
        {
            comboBox_SelectFunc.Items.Add("对.SC2Map地图文件进行混淆");
            comboBox_SelectFunc.Items.Add("对自定义Galaxy代码进行混淆");
            comboBox_SelectFunc.Items.Add("[正在开发]将Objects等地形布置信息转Galaxy");
            comboBox_SelectFunc.Items.Add("[仅测试]尝试将乱码转回中文");
            comboBox_SelectFunc.Items.Add("将代码中的UnitCreate转回地形信息格式（ObjectUnit）");
            comboBox_SelectFunc.Items.Add("批扫描虫出封锁线N档案或星际录像并生成恢复用文本");
            comboBox_SelectFunc.Items.Add("读取星际录像或其他二进制文件");
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
                case 3:
                    LoadContentFromFile_Galaxy();
                    break;
                case 4:
                    LoadContentFromFile_Galaxy();
                    break;
                case 5:
                    LoadContentFromFile_Replay();
                    break;
                case 6:
                    LoadContentFromFile_Read();
                    break;
                default:
                    break;
            }
        }
    }
}
