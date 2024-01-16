namespace Daberna.Test.Windows
{
    partial class GamePage
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
            Games_listview = new ListView();
            label1 = new Label();
            label2 = new Label();
            GameName_Txt = new TextBox();
            Create_Btn = new Button();
            SuspendLayout();
            // 
            // Games_listview
            // 
            Games_listview.Location = new Point(12, 32);
            Games_listview.Name = "Games_listview";
            Games_listview.Size = new Size(201, 370);
            Games_listview.TabIndex = 0;
            Games_listview.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 14);
            label1.Name = "label1";
            label1.Size = new Size(64, 15);
            label1.TabIndex = 1;
            label1.Text = "Games List";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(241, 34);
            label2.Name = "label2";
            label2.Size = new Size(73, 15);
            label2.TabIndex = 10;
            label2.Text = "Game Name";
            // 
            // GameName_Txt
            // 
            GameName_Txt.Location = new Point(241, 52);
            GameName_Txt.Name = "GameName_Txt";
            GameName_Txt.Size = new Size(100, 23);
            GameName_Txt.TabIndex = 9;
            // 
            // Create_Btn
            // 
            Create_Btn.Location = new Point(241, 90);
            Create_Btn.Name = "Create_Btn";
            Create_Btn.Size = new Size(100, 23);
            Create_Btn.TabIndex = 8;
            Create_Btn.Text = "Create";
            Create_Btn.UseVisualStyleBackColor = true;
            Create_Btn.Click += Create_Btn_Click;
            // 
            // GamePage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 423);
            Controls.Add(label2);
            Controls.Add(GameName_Txt);
            Controls.Add(Create_Btn);
            Controls.Add(label1);
            Controls.Add(Games_listview);
            Name = "GamePage";
            Text = "GamePage";
            Load += GamePage_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView Games_listview;
        private Label label1;
        private Label label2;
        private TextBox GameName_Txt;
        private Button Create_Btn;
    }
}