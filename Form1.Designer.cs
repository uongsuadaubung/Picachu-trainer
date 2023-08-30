namespace Picachu
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            btnResetTime = new Button();
            checkBoxFreezeTime = new CheckBox();
            btnSuggestAPair = new Button();
            btnAuto = new Button();
            btnMatchOnePair = new Button();
            SuspendLayout();
            // 
            // btnResetTime
            // 
            btnResetTime.Location = new Point(410, 32);
            btnResetTime.Name = "btnResetTime";
            btnResetTime.Size = new Size(193, 69);
            btnResetTime.TabIndex = 0;
            btnResetTime.Text = "Reset time";
            btnResetTime.UseVisualStyleBackColor = true;
            btnResetTime.Click += button1_Click;
            // 
            // checkBoxFreezeTime
            // 
            checkBoxFreezeTime.AutoSize = true;
            checkBoxFreezeTime.Location = new Point(183, 49);
            checkBoxFreezeTime.Name = "checkBoxFreezeTime";
            checkBoxFreezeTime.Size = new Size(171, 36);
            checkBoxFreezeTime.TabIndex = 1;
            checkBoxFreezeTime.Text = "Freeze time";
            checkBoxFreezeTime.UseVisualStyleBackColor = true;
            checkBoxFreezeTime.CheckedChanged += checkBoxFreezeTime_CheckedChanged;
            // 
            // btnSuggestAPair
            // 
            btnSuggestAPair.Location = new Point(183, 142);
            btnSuggestAPair.Name = "btnSuggestAPair";
            btnSuggestAPair.Size = new Size(195, 66);
            btnSuggestAPair.TabIndex = 2;
            btnSuggestAPair.Text = "Suggest a pair";
            btnSuggestAPair.UseVisualStyleBackColor = true;
            btnSuggestAPair.Click += button2_Click;
            // 
            // btnAuto
            // 
            btnAuto.Location = new Point(410, 256);
            btnAuto.Name = "btnAuto";
            btnAuto.Size = new Size(193, 66);
            btnAuto.TabIndex = 3;
            btnAuto.Text = "Auto";
            btnAuto.UseVisualStyleBackColor = true;
            btnAuto.Click += btnAuto_Click;
            // 
            // btnMatchOnePair
            // 
            btnMatchOnePair.Location = new Point(410, 142);
            btnMatchOnePair.Name = "btnMatchOnePair";
            btnMatchOnePair.Size = new Size(195, 66);
            btnMatchOnePair.TabIndex = 4;
            btnMatchOnePair.Text = "Match a pair";
            btnMatchOnePair.UseVisualStyleBackColor = true;
            btnMatchOnePair.Click += btnMatchOnePair_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnMatchOnePair);
            Controls.Add(btnAuto);
            Controls.Add(btnSuggestAPair);
            Controls.Add(checkBoxFreezeTime);
            Controls.Add(btnResetTime);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pikachu trainer";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnResetTime;
        private CheckBox checkBoxFreezeTime;
        private Button btnSuggestAPair;
        private Button btnAuto;
        private Button btnMatchOnePair;
    }
}