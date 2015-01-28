namespace BLETest.Ui
{
    partial class LandingPage
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
            this.btnStartPID = new System.Windows.Forms.Button();
            this.btnStartML = new System.Windows.Forms.Button();
            this.btnLoadML = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartPID
            // 
            this.btnStartPID.Location = new System.Drawing.Point(6, 6);
            this.btnStartPID.Margin = new System.Windows.Forms.Padding(6);
            this.btnStartPID.Name = "btnStartPID";
            this.btnStartPID.Size = new System.Drawing.Size(164, 62);
            this.btnStartPID.TabIndex = 0;
            this.btnStartPID.Text = "Start PID Controller";
            this.btnStartPID.UseVisualStyleBackColor = true;
            this.btnStartPID.Click += new System.EventHandler(this.btnStartPID_Click);
            // 
            // btnStartML
            // 
            this.btnStartML.Location = new System.Drawing.Point(6, 80);
            this.btnStartML.Margin = new System.Windows.Forms.Padding(6);
            this.btnStartML.Name = "btnStartML";
            this.btnStartML.Size = new System.Drawing.Size(164, 62);
            this.btnStartML.TabIndex = 1;
            this.btnStartML.Text = "Start new Machine Learning Controller";
            this.btnStartML.UseVisualStyleBackColor = true;
            this.btnStartML.Click += new System.EventHandler(this.btnStartML_Click);
            // 
            // btnLoadML
            // 
            this.btnLoadML.Location = new System.Drawing.Point(6, 154);
            this.btnLoadML.Margin = new System.Windows.Forms.Padding(6);
            this.btnLoadML.Name = "btnLoadML";
            this.btnLoadML.Size = new System.Drawing.Size(164, 62);
            this.btnLoadML.TabIndex = 2;
            this.btnLoadML.Text = "Load Machine Learning Controller";
            this.btnLoadML.UseVisualStyleBackColor = true;
            this.btnLoadML.Click += new System.EventHandler(this.btnLoadML_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnStartPID);
            this.flowLayoutPanel1.Controls.Add(this.btnStartML);
            this.flowLayoutPanel1.Controls.Add(this.btnLoadML);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(193, 79);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(176, 222);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // LandingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 384);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "LandingPage";
            this.Text = "WhiteBot Controller";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartPID;
        private System.Windows.Forms.Button btnStartML;
        private System.Windows.Forms.Button btnLoadML;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}