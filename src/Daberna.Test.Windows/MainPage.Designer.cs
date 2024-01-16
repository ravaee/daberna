namespace Daberna.Test.Windows
{
    partial class MainPage
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Name_txt = new TextBox();
            Connect_btn = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // Name_txt
            // 
            Name_txt.Location = new Point(38, 42);
            Name_txt.Name = "Name_txt";
            Name_txt.Size = new Size(100, 23);
            Name_txt.TabIndex = 0;
            // 
            // Connect_btn
            // 
            Connect_btn.Location = new Point(40, 77);
            Connect_btn.Name = "Connect_btn";
            Connect_btn.Size = new Size(75, 23);
            Connect_btn.TabIndex = 1;
            Connect_btn.Text = "Connect";
            Connect_btn.UseVisualStyleBackColor = true;
            Connect_btn.Click += Connect_btn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(39, 16);
            label1.Name = "label1";
            label1.Size = new Size(93, 15);
            label1.TabIndex = 2;
            label1.Text = "Enter you name:";
            // 
            // MainPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(Connect_btn);
            Controls.Add(Name_txt);
            Name = "MainPage";
            Text = "Form1";
            Load += MainPage_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox Name_txt;
        private Button Connect_btn;
        private Label label1;
    }
}