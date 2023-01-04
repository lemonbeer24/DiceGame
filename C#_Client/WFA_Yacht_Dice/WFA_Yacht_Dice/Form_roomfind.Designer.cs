
namespace WFA_Yacht_Dice
{
    partial class Form_roomfind
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
            this.listBox_roomlist = new System.Windows.Forms.ListBox();
            this.button_Entry = new System.Windows.Forms.Button();
            this.button_refresh = new System.Windows.Forms.Button();
            this.textBox_rog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_nickname = new System.Windows.Forms.TextBox();
            this.timer_receive = new System.Windows.Forms.Timer(this.components);
            this.timer_join = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBox_roomlist
            // 
            this.listBox_roomlist.FormattingEnabled = true;
            this.listBox_roomlist.ItemHeight = 12;
            this.listBox_roomlist.Location = new System.Drawing.Point(12, 60);
            this.listBox_roomlist.Name = "listBox_roomlist";
            this.listBox_roomlist.Size = new System.Drawing.Size(345, 184);
            this.listBox_roomlist.TabIndex = 0;
            // 
            // button_Entry
            // 
            this.button_Entry.Location = new System.Drawing.Point(12, 250);
            this.button_Entry.Name = "button_Entry";
            this.button_Entry.Size = new System.Drawing.Size(345, 23);
            this.button_Entry.TabIndex = 1;
            this.button_Entry.Text = "입장";
            this.button_Entry.UseVisualStyleBackColor = true;
            this.button_Entry.Click += new System.EventHandler(this.button_Entry_Click);
            // 
            // button_refresh
            // 
            this.button_refresh.Location = new System.Drawing.Point(12, 279);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(345, 23);
            this.button_refresh.TabIndex = 2;
            this.button_refresh.Text = "새로고침";
            this.button_refresh.UseVisualStyleBackColor = true;
            this.button_refresh.Click += new System.EventHandler(this.button_refresh_Click);
            // 
            // textBox_rog
            // 
            this.textBox_rog.Location = new System.Drawing.Point(12, 308);
            this.textBox_rog.Multiline = true;
            this.textBox_rog.Name = "textBox_rog";
            this.textBox_rog.Size = new System.Drawing.Size(345, 89);
            this.textBox_rog.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "닉네임 : ";
            // 
            // textBox_nickname
            // 
            this.textBox_nickname.Location = new System.Drawing.Point(71, 9);
            this.textBox_nickname.Name = "textBox_nickname";
            this.textBox_nickname.Size = new System.Drawing.Size(286, 21);
            this.textBox_nickname.TabIndex = 5;
            // 
            // timer_receive
            // 
            this.timer_receive.Tick += new System.EventHandler(this.timer_receive_Tick);
            // 
            // timer_join
            // 
            this.timer_join.Tick += new System.EventHandler(this.timer_join_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "방 목록 들 :";
            // 
            // Form_roomfind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 409);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_nickname);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_rog);
            this.Controls.Add(this.button_refresh);
            this.Controls.Add(this.button_Entry);
            this.Controls.Add(this.listBox_roomlist);
            this.Name = "Form_roomfind";
            this.Text = "Form_roomfind";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_roomlist;
        private System.Windows.Forms.Button button_Entry;
        private System.Windows.Forms.Button button_refresh;
        private System.Windows.Forms.TextBox textBox_rog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_nickname;
        private System.Windows.Forms.Timer timer_receive;
        private System.Windows.Forms.Timer timer_join;
        private System.Windows.Forms.Label label2;
    }
}