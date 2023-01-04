
namespace WFA_Yacht_Dice
{
    partial class Form_connect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_connectLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_contextip = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_connectLog
            // 
            this.textBox_connectLog.Location = new System.Drawing.Point(12, 71);
            this.textBox_connectLog.Multiline = true;
            this.textBox_connectLog.Name = "textBox_connectLog";
            this.textBox_connectLog.Size = new System.Drawing.Size(325, 92);
            this.textBox_connectLog.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "연결할 서버 ip :";
            // 
            // textBox_contextip
            // 
            this.textBox_contextip.Location = new System.Drawing.Point(107, 12);
            this.textBox_contextip.Name = "textBox_contextip";
            this.textBox_contextip.Size = new System.Drawing.Size(230, 21);
            this.textBox_contextip.TabIndex = 2;
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(12, 42);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(325, 23);
            this.button_connect.TabIndex = 3;
            this.button_connect.Text = "서버에 연결..";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // Form_connect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 174);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.textBox_contextip);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_connectLog);
            this.Name = "Form_connect";
            this.Text = "Form_connect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_connectLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_contextip;
        private System.Windows.Forms.Button button_connect;
    }
}