using System.Drawing;
using System.Windows.Forms;

namespace GalaxyObfuscator
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel_Top = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_selectRulePath = new System.Windows.Forms.Button();
            this.textBox_rulePath = new System.Windows.Forms.TextBox();
            this.label_rulePath = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_selectWorkPath = new System.Windows.Forms.Button();
            this.textBox_workPath = new System.Windows.Forms.TextBox();
            this.label_workPath = new System.Windows.Forms.Label();
            this.label_Statistics = new System.Windows.Forms.Label();
            this.label_Tips = new System.Windows.Forms.Label();
            this.button_Run = new System.Windows.Forms.Button();
            this.checkBox_LC4 = new System.Windows.Forms.CheckBox();
            this.comboBox_SelectFunc = new System.Windows.Forms.ComboBox();
            this.label_SelectFunc = new System.Windows.Forms.Label();
            this.richTextBox_Code = new System.Windows.Forms.RichTextBox();
            this.panel_Bottom = new System.Windows.Forms.Panel();
            this.richTextBox_List = new System.Windows.Forms.RichTextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel_Top.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel_Bottom.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Top
            // 
            this.panel_Top.Controls.Add(this.panel2);
            this.panel_Top.Controls.Add(this.panel1);
            this.panel_Top.Controls.Add(this.label_Statistics);
            this.panel_Top.Controls.Add(this.label_Tips);
            this.panel_Top.Controls.Add(this.button_Run);
            this.panel_Top.Location = new System.Drawing.Point(0, 0);
            this.panel_Top.Margin = new System.Windows.Forms.Padding(0);
            this.panel_Top.Name = "panel_Top";
            this.panel_Top.Size = new System.Drawing.Size(806, 85);
            this.panel_Top.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button_selectRulePath);
            this.panel2.Controls.Add(this.textBox_rulePath);
            this.panel2.Controls.Add(this.label_rulePath);
            this.panel2.Location = new System.Drawing.Point(0, 42);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(737, 21);
            this.panel2.TabIndex = 13;
            // 
            // button_selectRulePath
            // 
            this.button_selectRulePath.Location = new System.Drawing.Point(677, 0);
            this.button_selectRulePath.Margin = new System.Windows.Forms.Padding(0);
            this.button_selectRulePath.Name = "button_selectRulePath";
            this.button_selectRulePath.Size = new System.Drawing.Size(60, 21);
            this.button_selectRulePath.TabIndex = 4;
            this.button_selectRulePath.Text = "Sclect";
            this.button_selectRulePath.UseVisualStyleBackColor = true;
            this.button_selectRulePath.Click += new System.EventHandler(this.button_selectRulePath_Click);
            // 
            // textBox_rulePath
            // 
            this.textBox_rulePath.Location = new System.Drawing.Point(86, 0);
            this.textBox_rulePath.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_rulePath.MaximumSize = new System.Drawing.Size(640, 30);
            this.textBox_rulePath.MinimumSize = new System.Drawing.Size(640, 28);
            this.textBox_rulePath.Name = "textBox_rulePath";
            this.textBox_rulePath.Size = new System.Drawing.Size(640, 21);
            this.textBox_rulePath.TabIndex = 3;
            // 
            // label_rulePath
            // 
            this.label_rulePath.Location = new System.Drawing.Point(0, 0);
            this.label_rulePath.Margin = new System.Windows.Forms.Padding(0);
            this.label_rulePath.Name = "label_rulePath";
            this.label_rulePath.Size = new System.Drawing.Size(86, 21);
            this.label_rulePath.TabIndex = 5;
            this.label_rulePath.Text = "排除规则";
            this.label_rulePath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_selectWorkPath);
            this.panel1.Controls.Add(this.textBox_workPath);
            this.panel1.Controls.Add(this.label_workPath);
            this.panel1.Location = new System.Drawing.Point(0, 21);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(737, 21);
            this.panel1.TabIndex = 12;
            // 
            // button_selectWorkPath
            // 
            this.button_selectWorkPath.Location = new System.Drawing.Point(677, 0);
            this.button_selectWorkPath.Margin = new System.Windows.Forms.Padding(0);
            this.button_selectWorkPath.Name = "button_selectWorkPath";
            this.button_selectWorkPath.Size = new System.Drawing.Size(60, 21);
            this.button_selectWorkPath.TabIndex = 4;
            this.button_selectWorkPath.Text = "Sclect";
            this.button_selectWorkPath.UseVisualStyleBackColor = true;
            this.button_selectWorkPath.Click += new System.EventHandler(this.button_selectWorkPath_Click);
            // 
            // textBox_workPath
            // 
            this.textBox_workPath.Location = new System.Drawing.Point(86, 0);
            this.textBox_workPath.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_workPath.MaximumSize = new System.Drawing.Size(640, 30);
            this.textBox_workPath.MinimumSize = new System.Drawing.Size(640, 28);
            this.textBox_workPath.Name = "textBox_workPath";
            this.textBox_workPath.Size = new System.Drawing.Size(640, 21);
            this.textBox_workPath.TabIndex = 3;
            // 
            // label_workPath
            // 
            this.label_workPath.Location = new System.Drawing.Point(0, 0);
            this.label_workPath.Margin = new System.Windows.Forms.Padding(0);
            this.label_workPath.Name = "label_workPath";
            this.label_workPath.Size = new System.Drawing.Size(86, 21);
            this.label_workPath.TabIndex = 5;
            this.label_workPath.Text = "处理目录";
            this.label_workPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Statistics
            // 
            this.label_Statistics.Location = new System.Drawing.Point(557, 0);
            this.label_Statistics.Margin = new System.Windows.Forms.Padding(0);
            this.label_Statistics.Name = "label_Statistics";
            this.label_Statistics.Size = new System.Drawing.Size(249, 21);
            this.label_Statistics.TabIndex = 11;
            this.label_Statistics.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Tips
            // 
            this.label_Tips.Location = new System.Drawing.Point(0, 0);
            this.label_Tips.Margin = new System.Windows.Forms.Padding(0);
            this.label_Tips.Name = "label_Tips";
            this.label_Tips.Size = new System.Drawing.Size(557, 21);
            this.label_Tips.TabIndex = 10;
            this.label_Tips.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Run
            // 
            this.button_Run.Location = new System.Drawing.Point(737, 21);
            this.button_Run.Margin = new System.Windows.Forms.Padding(0);
            this.button_Run.Name = "button_Run";
            this.button_Run.Size = new System.Drawing.Size(69, 42);
            this.button_Run.TabIndex = 2;
            this.button_Run.Text = "执行";
            this.button_Run.UseVisualStyleBackColor = true;
            this.button_Run.Click += new System.EventHandler(this.button_Run_Click);
            // 
            // checkBox_LC4
            // 
            this.checkBox_LC4.Enabled = false;
            this.checkBox_LC4.Location = new System.Drawing.Point(678, 0);
            this.checkBox_LC4.Margin = new System.Windows.Forms.Padding(0);
            this.checkBox_LC4.Name = "checkBox_LC4";
            this.checkBox_LC4.Size = new System.Drawing.Size(60, 21);
            this.checkBox_LC4.TabIndex = 7;
            this.checkBox_LC4.Text = "LC4";
            this.checkBox_LC4.UseVisualStyleBackColor = true;
            // 
            // comboBox_SelectFunc
            // 
            this.comboBox_SelectFunc.AccessibleRole = System.Windows.Forms.AccessibleRole.IpAddress;
            this.comboBox_SelectFunc.DropDownHeight = 120;
            this.comboBox_SelectFunc.FormattingEnabled = true;
            this.comboBox_SelectFunc.IntegralHeight = false;
            this.comboBox_SelectFunc.Location = new System.Drawing.Point(86, 0);
            this.comboBox_SelectFunc.Margin = new System.Windows.Forms.Padding(0);
            this.comboBox_SelectFunc.Name = "comboBox_SelectFunc";
            this.comboBox_SelectFunc.Size = new System.Drawing.Size(592, 20);
            this.comboBox_SelectFunc.TabIndex = 7;
            // 
            // label_SelectFunc
            // 
            this.label_SelectFunc.Location = new System.Drawing.Point(0, 0);
            this.label_SelectFunc.Margin = new System.Windows.Forms.Padding(0);
            this.label_SelectFunc.Name = "label_SelectFunc";
            this.label_SelectFunc.Size = new System.Drawing.Size(86, 21);
            this.label_SelectFunc.TabIndex = 7;
            this.label_SelectFunc.Text = "选择功能";
            this.label_SelectFunc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richTextBox_Code
            // 
            this.richTextBox_Code.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_Code.Margin = new System.Windows.Forms.Padding(0);
            this.richTextBox_Code.Name = "richTextBox_Code";
            this.richTextBox_Code.Size = new System.Drawing.Size(652, 453);
            this.richTextBox_Code.TabIndex = 6;
            this.richTextBox_Code.Text = "";
            // 
            // panel_Bottom
            // 
            this.panel_Bottom.Controls.Add(this.richTextBox_List);
            this.panel_Bottom.Controls.Add(this.richTextBox_Code);
            this.panel_Bottom.Location = new System.Drawing.Point(0, 85);
            this.panel_Bottom.Margin = new System.Windows.Forms.Padding(0);
            this.panel_Bottom.Name = "panel_Bottom";
            this.panel_Bottom.Size = new System.Drawing.Size(806, 452);
            this.panel_Bottom.TabIndex = 7;
            this.panel_Bottom.Visible = false;
            // 
            // richTextBox_List
            // 
            this.richTextBox_List.Location = new System.Drawing.Point(651, 0);
            this.richTextBox_List.Margin = new System.Windows.Forms.Padding(0);
            this.richTextBox_List.Name = "richTextBox_List";
            this.richTextBox_List.Size = new System.Drawing.Size(155, 453);
            this.richTextBox_List.TabIndex = 7;
            this.richTextBox_List.Text = "";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label_SelectFunc);
            this.panel3.Controls.Add(this.comboBox_SelectFunc);
            this.panel3.Controls.Add(this.checkBox_LC4);
            this.panel3.Location = new System.Drawing.Point(0, 64);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(806, 21);
            this.panel3.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 537);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel_Bottom);
            this.Controls.Add(this.panel_Top);
            this.Name = "Form1";
            this.Text = "代码混淆器V0.3（For Galaxy） By 蔚蓝星海";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel_Top.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel_Bottom.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        System.Windows.Forms.Panel panel_Top;
        System.Windows.Forms.CheckBox checkBox_LC4;
        System.Windows.Forms.ComboBox comboBox_SelectFunc;
        System.Windows.Forms.Label label_SelectFunc;
        System.Windows.Forms.Label label_workPath;
        System.Windows.Forms.Button button_selectWorkPath;
        System.Windows.Forms.TextBox textBox_workPath;
        System.Windows.Forms.Button button_Run;
        System.Windows.Forms.RichTextBox richTextBox_Code;
        System.Windows.Forms.Label label_Tips;
        System.Windows.Forms.Label label_Statistics;
        Panel panel_Bottom;
        RichTextBox richTextBox_List;
        Panel panel1;
        Panel panel2;
        Button button_selectRulePath;
        TextBox textBox_rulePath;
        Label label_rulePath;
        Panel panel3;
    }
}

