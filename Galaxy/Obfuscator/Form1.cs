using System.Text.RegularExpressions;
using System.Diagnostics;
using MetalMaxSystem;
using System.ComponentModel;

namespace Obfuscator
{
    public partial class Form1 : Form
    {
        private static bool _userOpEnable = true;
        /// <summary>
        /// �û��������
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        /// <summary>
        /// �û��������
        /// </summary>
        public static bool UserOpEnable { get => _userOpEnable; set => _userOpEnable = value; }

        private static bool _workStatus = false;
        /// <summary>
        /// ����״̬
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        /// <summary>
        /// ����״̬
        /// </summary>
        public static bool WorkStatus { get => _workStatus; set => _workStatus = value; }

        private static bool _workStop = false;
        /// <summary>
        /// ��Ϲ����õ�״̬����
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        /// <summary>
        /// ��Ϲ����õ�״̬����
        /// </summary>
        public static bool WorkStop { get => _workStop; set => _workStop = value; }

        private static Thread _workThread;
        /// <summary>
        /// ����ר�ú�̨���̣߳���ֹ����ʱUI���߳̽��濨ס�޷����������
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        /// <summary>
        /// ����ר�ú�̨���̣߳���ֹ����ʱUI���߳̽��濨ס�޷����������
        /// </summary>
        public static Thread WorkThread { get => _workThread; set => _workThread = value; }

        public Form1()
        {
            InitializeComponent();
            //label_Tips.ForeColor = Color.Red;
            label_Statistics.ForeColor = Color.Red;

            //Obfuscator obfuscator = new Obfuscator();
            //obfuscator.ObfuscateFile("C:/Users/linsh/Desktop/���ܿ����ͼ.SC2Map");
            //MessageBox.Show("Completed!");
        }

        private string GetCodeFromMainThread()
        {
            if (richTextBox_Code.InvokeRequired)
            {
                return richTextBox_Code.Invoke(GetCodeFromMainThread);
            }
            else
            {
                return richTextBox_Code.Text;
            }
        }

        private void SetCodeToMainThread(string code)
        {
            // ���� Invoke �������������͵����߳�
            Invoke((MethodInvoker)delegate ()
            {
                richTextBox_Code.Text = code;
            });
        }

        private int GetSelectedIndexFromMainThread()
        {
            if (comboBox_SelectFunc.InvokeRequired)
            {
                return comboBox_SelectFunc.Invoke(GetSelectedIndexFromMainThread);
            }
            else
            {
                return comboBox_SelectFunc.SelectedIndex;
            }
        }

        private string GetTipsFromMainThread()
        {
            if (label_Tips.InvokeRequired)
            {
                return label_Tips.Invoke(GetTipsFromMainThread);
            }
            else
            {
                return label_Tips.Text;
            }
        }

        private void SetTipsToMainThread(string tips)
        {
            // ���� Invoke �������������͵����߳�
            Invoke((MethodInvoker)delegate ()
            {
                label_Tips.Text = tips;
            });
        }

        private string GetStatisticsFromMainThread()
        {
            if (label_Statistics.InvokeRequired)
            {
                return label_Statistics.Invoke(GetStatisticsFromMainThread);
            }
            else
            {
                return label_Statistics.Text;
            }
        }

        private string GetExclusionRulesPathFromMainThread()
        {
            if (textBox_ExclusionRulesPath.InvokeRequired)
            {
                return textBox_ExclusionRulesPath.Invoke(GetExclusionRulesPathFromMainThread);
            }
            else
            {
                return textBox_ExclusionRulesPath.Text;
            }
        }

        private void SetExclusionRulesPathToMainThread(string path)
        {
            // ���� Invoke �������������͵����߳�
            Invoke((MethodInvoker)delegate ()
            {
                textBox_ExclusionRulesPath.Text = path;
            });
        }

        private void SetStatisticsToMainThread(string text)
        {
            // ���� Invoke �������������͵����߳�
            Invoke((MethodInvoker)delegate ()
            {
                label_Statistics.Text = text;
            });
        }

        private void SetBtnRunTextToMainThread(string text)
        {
            // ���� Invoke �������������͵����߳�
            Invoke((MethodInvoker)delegate ()
            {
                button_Run.Text = text;
            });
        }

        private void SetPanelBackColorToMainThread(Panel p, Color c)
        {
            // ���� Invoke �������������͵����߳�
            Invoke((MethodInvoker)delegate ()
            {
                p.BackColor = c;
            });
        }

        private void SetControlEnableToMainThread(Control c, bool torf)
        {
            // ���� Invoke �������������͵����߳�
            Invoke((MethodInvoker)delegate ()
            {
                c.Enabled = torf;
            });
        }

        private Type GetControlTypeFromMainThread(Control control)
        {
            Type type = null;
            if (control.InvokeRequired) // �жϵ�ǰ�߳��Ƿ�ΪUI���߳�
            {
                control.BeginInvoke((MethodInvoker)(() => GetControlTypeFromMainThread(control))); // ����������UI���̵߳���Ϣ������
            }
            else
            {
                type = control.GetType(); // ��ȡ�ؼ���������Ϣ
            }
            return type;
        }

        /// <summary>
        /// ��ť���������Ҫ���ݣ����������ɺ�̨�߳�����
        /// </summary>
        private void ButtonRun()
        {
            for (int i = 0; i < 1; i++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                switch (GetSelectedIndexFromMainThread())
                {
                    case -1:
                        SetTipsToMainThread("����δѡ��");
                        break;
                    case 0:
                        //SetTipsToMainThread("��Galaxy������л���");
                        SelectedFunc_0(GetExclusionRulesPathFromMainThread());
                        break;
                    default:
                        SetTipsToMainThread("������Ч��");
                        break;
                }
                stopwatch.Stop();
                //Debug.WriteLine(stopwatch.Elapsed.ToString());
                SetStatisticsToMainThread(" ʱ�ģ�" + stopwatch.Elapsed.ToString());
            }
            //�������߳�ע������������ʼ���������ˣ�����֪�����û��жϻ����������н���
            WorkStatus = false;//���ù���״̬
            if (WorkStop) { SetStatisticsToMainThread("�û�ȡ����"); }
            WorkStop = false;//����_workStop״̬��������û�ȡ���ģ���ӡ��֪
            UserOpEnableChange(true);//�����û�����״̬
            SetBtnRunTextToMainThread("ִ��");
            //Debug.WriteLine("���߳��Ѿ���ɣ�");
            //�߳��������������ģ�Abort��������Ŀ���߳����׳�һ��ThreadAbortException�쳣�Ӷ�����Ŀ���̵߳���ֹ��
            //WorkThread.Abort();
        }

        /// <summary>
        /// The Button of Run.
        /// ���ִ�а�ť��Ӧ����һ����̨�߳̽��и�����������ֹ��UI���ڵ����߳���ɿ��١�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Run_Click(object sender, EventArgs e)
        {
            //����ƥ��ץȡȫ���������������ظ���
            if (richTextBox_Code.Text != "")
            {
                for (int i = 0; i < 1; i++)
                {
                    //��ʼ�������󲿷ֽ����ûң��û����ɲ�����
                    UserOpEnableChange(false);

                    if (button_Run.Text == "ִ��" && WorkStatus == false)
                    {
                        WorkStatus = true;
                        button_Run.Text = "ȡ��";
                        //������̨�߳�ʵ�������и�������
                        WorkThread = new Thread(ButtonRun) { IsBackground = true };
                        WorkThread.Start();
                        // �ȴ����߳���ɡ�
                        //WorkThread.Join();
                    }
                    else if (button_Run.Text == "ȡ��" && WorkStatus == true)
                    {
                        WorkStop = true;
                    }
                }
            }

        }

        /// <summary>
        /// ��1������������ִ�з�������Galaxy������л���
        /// </summary>
        /// <param name="exclusionRulesPath"></param>
        private void SelectedFunc_0(string exclusionRulesPath)
        {
            //�ų������ļ�ʹ��ϵͳĬ�ϵ������1���ļ�·��Ϊ�գ�2���ļ�·���䲻Ϊ�յ���׺��.txt��3���ļ�·���Ƿ���
            if (
                exclusionRulesPath == ""
                || !(Regex.IsMatch(exclusionRulesPath, @"^(.*)(\.txt)$"))
                || !MMCore.IsDFPath(exclusionRulesPath)
            )
            {
                //�ı�·����������ΪϵͳĬ��
                exclusionRulesPath = AppDomain.CurrentDomain.BaseDirectory + @"exclusion_rules.txt";
                SetExclusionRulesPathToMainThread(exclusionRulesPath);
                //SetTipsToMainThread("�ų������ļ�·����������ΪϵͳĬ�ϣ�");
            }
            //ȥ���������ע��
            string mainCode = MMCore.RemoveEmptyLines(MMCore.RemoveComments(GetCodeFromMainThread()));
            // ����������ʽģʽ��ƥ�亯����
            string pattern = @"(?<=^|[^a-zA-Z_])[a-zA-Z_][\w]*(?=\s*\([^\)]*\)(\s+|\n|$))";
            MatchCollection matches = Regex.Matches(mainCode, pattern);
            CodeObfuscator obfuscator = new CodeObfuscator();
            //��ȡ�ų������ı�����ӻ��ȹ���ʱ���ֹ���ǲ�����ȣ���ʽ��ÿ��һ����������
            obfuscator.LoadExclusionRules(exclusionRulesPath);
            //����ȫ��������
            foreach (Match match in matches)
            {
                //Debug.WriteLine("Function Name: " + match.Value);
                //����������ʽ������"�ų������ı�"ָ���ĺ���������Lib��GAx3��ͷ�ĺ�����Ҳ�ܿ�����
                if (!Regex.IsMatch(match.Value, "^(Lib|lib|GAx3).*"))
                {
                    //��Ӻ����������Ⱥ����Ƶ����ȹ�������̻��Զ�ȥ�أ�Ҳ����������ͬ�������ƣ�
                    obfuscator.AddReplacement(match.Value);
                }

            }
            //Ҫ�����Lib_gf_A��gf_A������߼ӵ����ȹ���ǰ��Ҳ�ᱻ�滻һ���֣����Ի���ǰ�ü�麯�������Ի��ȹ����޸ģ����һ����ȹ�����ļ����ǰ�����ƥ��ĺ�������һ�������޳��ü�
            foreach (Match match in matches)
            {
                if (Regex.IsMatch(match.Value, "^(Lib|lib|GAx3).*"))
                {
                    //���ȹ����ֵ���ļ�����match.Value��һ��������ֵ����޳��ü�
                    //���match.Value�Ƿ�����ֵ��е��κμ�
                    foreach (var key in obfuscator.Replacements.Keys)
                    {
                        if (match.Value.Contains(key))
                        {
                            //���match.Value�����ֵ��еļ�������ֵ���ɾ���ü�
                            obfuscator.Replacements.Remove(key);
                        }
                    }
                }
            }

            //��һ����Ⱥ�Ĵ���
            string obfuscatedCode = obfuscator.ObfuscateCode(mainCode);

            string temp = "";

            //�ڶ������
            //�Դ����ı��е��ַ������л���
            //MatchCollection matches2 = Regex.Matches(obfuscatedCode, @"""(.*?)""");
            MatchCollection matches2 = Regex.Matches(obfuscatedCode, @"""[^\\""]*""");
            foreach (Match match in matches2)
            {
                //ƥ�䵽���ַ���������(.*?)����match.Groups[1].Value�����ݷǿ��������ӹ���
                if (match.Groups[1].Value != "")
                {
                    if (Regex.IsMatch(match.Groups[1].Value, @".*\\.*")) { Debug.WriteLine(match.Groups[1].Value); }
                    //����������ʽ����������ָ���ַ����ַ�����ܿ�����
                    if (!Regex.IsMatch(match.Groups[1].Value, "(\\|bnet:)"))
                    {
                        //��ӵ����ȹ���2��Ҫ�滻���ַ���Ϊ�������ȳ�8���ƻ�18���ƺ���ַ���Ϊֵ��
                        temp = MMCore.ConvertStringToHOMixed(match.Groups[1].Value, 0.7); //���������ݵĻ���
                        //Debug.WriteLine($"Found string: {match.Groups[1].Value}, Value: {temp}");
                        temp = "\"" + temp + "\""; //������������
                        //ע��˴��ڶ������ļ�Ҫ��""���ܹ������ַ���Ϊ��
                        obfuscator.AddReplacement2(match.Value, temp);
                    }
                }
            }
            obfuscatedCode = obfuscator.ObfuscateCode2(obfuscatedCode);

            //������
            //������ʽƥ��void InitMap () { �� }������
            //pattern = @"void\s*InitMap\s*\(\)\s*\{(.*\S)*\}";
            ////ʹ�� RegexOptions.Multiline ѡ����ָ��ģʽӦ�ڶ�����Ͻ���ƥ�䣬��ʹ�� RegexOptions.Singleline ѡ����ָ��ģʽӦ�ڵ��������ַ����Ͻ���ƥ��
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

            string sCHeadPath = AppDomain.CurrentDomain.BaseDirectory + @"SCHead";
            string sCEndPath = AppDomain.CurrentDomain.BaseDirectory + @"SCEnd";
            string sCHead = File.ReadAllText(sCHeadPath);
            string sCEnd = File.ReadAllText(sCEndPath);
            obfuscatedCode = sCHead + obfuscatedCode + sCEnd;

            SetCodeToMainThread(obfuscatedCode);
        }

        private void UserOpEnableChange(bool torf)
        {
            UserOpEnable = torf;
            //����ȫ�ؼ�����ȡ���ͣ��ɿ��̲߳��ûص������ؼ����Զ�д��������Ҫ�ص�
            foreach (Control a in Controls)
            {
                if (a is Panel)
                {
                    Panel p = a as Panel;  //ȡ��Panel
                    if (!torf)
                    {
                        // �ı�Panel����ɫ��ִ��ʱ�ǻ�ɫ
                        SetPanelBackColorToMainThread(p, Color.Gray);
                    }
                    else
                    {
                        //��ִ��ʱ�ǰ�ɫ
                        SetPanelBackColorToMainThread(p, Color.Transparent);
                    }

                    foreach (Control c in p.Controls) //��������е�ÿһ���ؼ�
                    {
                        //Debug.WriteLine(c.GetType().Name);
                        if (c.GetType().Name.Equals("TextBox"))
                        {
                            //�����ı���
                            SetControlEnableToMainThread(c, torf);
                        }
                        if (c.GetType().Name.Equals("CheckBox"))
                        {
                            //���ø�ѡ��
                            SetControlEnableToMainThread(c, torf);
                        }
                        if (c.GetType().Name.Equals("ComboBox"))
                        {
                            //����������
                            SetControlEnableToMainThread(c, torf);
                        }
                        if (c.GetType().Name.Equals("Label"))
                        {
                            //����������
                            SetControlEnableToMainThread(c, torf);
                        }
                        if (c.GetType().Name.Equals("Button"))
                        {
                            if (!c.Name.Equals("button_Run"))
                            {
                                //���ó����а�ť���������ť
                                SetControlEnableToMainThread(c, torf);
                            }
                        }

                    }
                }
            }
            //����Controlsδ��������ʽ�ı��򣬵�������
            SetControlEnableToMainThread(richTextBox_Code, torf);
        }

        private void button_SelectExclusionRulesFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string path = ofd.FileName;
            textBox_ExclusionRulesPath.Text = path;
        }

        private void richTextBox_Code_TextChanged(object sender, EventArgs e)
        {
            //�ı��ı�ʱ����Ҫд�κζ���
        }

        private void comboBox_SelectFunc_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GetSelectedIndexFromMainThread())
            {
                case -1:
                    SetTipsToMainThread("����δѡ��");
                    break;
                case 0:
                    //SetTipsToMainThread("��Galaxy������л���");
                    break;
                default:
                    SetTipsToMainThread("������Ч��");
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox_SelectFunc.Items.Add("��Galaxy������л��ȣ�����Ĭ�Ϲ���");
            comboBox_SelectFunc.SelectedIndex = 0;
        }
    }
}
