
namespace WFA_Yacht_Dice
{
    partial class Form_roomcreate
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_roomname = new System.Windows.Forms.TextBox();
            this.button_roomcreate = new System.Windows.Forms.Button();
            this.textBox_rog = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_nickname = new System.Windows.Forms.TextBox();
            this.timer_receiveTime = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "생성할 방의 이름 : ";
            // 
            // textBox_roomname
            // 
            this.textBox_roomname.Location = new System.Drawing.Point(127, 6);
            this.textBox_roomname.Name = "textBox_roomname";
            this.textBox_roomname.Size = new System.Drawing.Size(251, 21);
            this.textBox_roomname.TabIndex = 1;
            // 
            // button_roomcreate
            // 
            this.button_roomcreate.Location = new System.Drawing.Point(14, 60);
            this.button_roomcreate.Name = "button_roomcreate";
            this.button_roomcreate.Size = new System.Drawing.Size(366, 23);
            this.button_roomcreate.TabIndex = 2;
            this.button_roomcreate.Text = "생성";
            this.button_roomcreate.UseVisualStyleBackColor = true;
            this.button_roomcreate.Click += new System.EventHandler(this.button_roomcreate_Click);
            // 
            // textBox_rog
            // 
            this.textBox_rog.Location = new System.Drawing.Point(14, 93);
            this.textBox_rog.Multiline = true;
            this.textBox_rog.Name = "textBox_rog";
            this.textBox_rog.Size = new System.Drawing.Size(364, 140);
            this.textBox_rog.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "닉네임 : ";
            // 
            // textBox_nickname
            // 
            this.textBox_nickname.Location = new System.Drawing.Point(127, 33);
            this.textBox_nickname.Name = "textBox_nickname";
            this.textBox_nickname.Size = new System.Drawing.Size(251, 21);
            this.textBox_nickname.TabIndex = 5;
            // 
            // timer_receiveTime
            // 
            this.timer_receiveTime.Interval = 10;
            this.timer_receiveTime.Tick += new System.EventHandler(this.timer_receiveTime_Tick);
            // 
            // Form_roomcreate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 245);
            this.Controls.Add(this.textBox_nickname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_rog);
            this.Controls.Add(this.button_roomcreate);
            this.Controls.Add(this.textBox_roomname);
            this.Controls.Add(this.label1);
            this.Name = "Form_roomcreate";
            this.Text = "Form_roomcreate";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_roomname;
        private System.Windows.Forms.Button button_roomcreate;
        private System.Windows.Forms.TextBox textBox_rog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_nickname;
        private System.Windows.Forms.Timer timer_receiveTime;
    }
}