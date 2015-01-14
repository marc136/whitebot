namespace BLETest
{
    partial class Form1
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
            this.changePositions = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.robot1OrientationVis1 = new BLETest.DirectionVisualizer();
            this.robot1OrientationVis2 = new BLETest.DirectionVisualizer();
            this.robot1OrientationVis3 = new BLETest.DirectionVisualizer();
            this.label6 = new System.Windows.Forms.Label();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.robot1GravityVis1 = new BLETest.DirectionVisualizer();
            this.robot1GravityVis2 = new BLETest.DirectionVisualizer();
            this.robot1GravityVis3 = new BLETest.DirectionVisualizer();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.robot1PositionX = new System.Windows.Forms.Label();
            this.robot1PositionY = new System.Windows.Forms.Label();
            this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
            this.penDown = new System.Windows.Forms.Button();
            this.penUp = new System.Windows.Forms.Button();
            this.eraserDown = new System.Windows.Forms.Button();
            this.eraserUp = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.robot2OrientationVis1 = new BLETest.DirectionVisualizer();
            this.robot2OrientationVis2 = new BLETest.DirectionVisualizer();
            this.robot2OrientationVis3 = new BLETest.DirectionVisualizer();
            this.label5 = new System.Windows.Forms.Label();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.robot2GravityVis1 = new BLETest.DirectionVisualizer();
            this.robot2GravityVis2 = new BLETest.DirectionVisualizer();
            this.robot2GravityVis3 = new BLETest.DirectionVisualizer();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.robot2PositionX = new System.Windows.Forms.Label();
            this.robot2PositionY = new System.Windows.Forms.Label();
            this.flowLayoutPanel8 = new System.Windows.Forms.FlowLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.drawAxes = new System.Windows.Forms.Button();
            this.drawFunction = new System.Windows.Forms.Button();
            this.drawNiceFunction = new System.Windows.Forms.Button();
            this.stop = new System.Windows.Forms.Button();
            this.flowLayoutPanel9 = new System.Windows.Forms.FlowLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.kp = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.ki = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.kd = new System.Windows.Forms.TextBox();
            this.moveForward = new System.Windows.Forms.Button();
            this.btnStraightSamples = new System.Windows.Forms.Button();
            this.btnStartControllers = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.flowLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel8.SuspendLayout();
            this.flowLayoutPanel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // changePositions
            // 
            this.changePositions.Location = new System.Drawing.Point(4, 5);
            this.changePositions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.changePositions.Name = "changePositions";
            this.changePositions.Size = new System.Drawing.Size(180, 35);
            this.changePositions.TabIndex = 7;
            this.changePositions.Text = "change positions";
            this.changePositions.UseVisualStyleBackColor = true;
            this.changePositions.Click += new System.EventHandler(this.changePositions_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.36795F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.63205F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel8, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(18, 18);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.8718F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.1282F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1011, 858);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel3);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel4);
            this.flowLayoutPanel1.Controls.Add(this.label8);
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel2);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel7);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 390);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(480, 463);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Robot 1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Orientation:";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.robot1OrientationVis1);
            this.flowLayoutPanel3.Controls.Add(this.robot1OrientationVis2);
            this.flowLayoutPanel3.Controls.Add(this.robot1OrientationVis3);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(4, 45);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(264, 86);
            this.flowLayoutPanel3.TabIndex = 5;
            // 
            // robot1OrientationVis1
            // 
            this.robot1OrientationVis1.IndicatorColor = System.Drawing.Color.Green;
            this.robot1OrientationVis1.IndicatorScale = 1F;
            this.robot1OrientationVis1.Location = new System.Drawing.Point(6, 8);
            this.robot1OrientationVis1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.robot1OrientationVis1.Name = "robot1OrientationVis1";
            this.robot1OrientationVis1.Size = new System.Drawing.Size(75, 77);
            this.robot1OrientationVis1.TabIndex = 0;
            this.robot1OrientationVis1.X = 1F;
            this.robot1OrientationVis1.Y = 0F;
            // 
            // robot1OrientationVis2
            // 
            this.robot1OrientationVis2.IndicatorColor = System.Drawing.Color.Blue;
            this.robot1OrientationVis2.IndicatorScale = 1F;
            this.robot1OrientationVis2.Location = new System.Drawing.Point(93, 8);
            this.robot1OrientationVis2.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.robot1OrientationVis2.Name = "robot1OrientationVis2";
            this.robot1OrientationVis2.Size = new System.Drawing.Size(75, 77);
            this.robot1OrientationVis2.TabIndex = 1;
            this.robot1OrientationVis2.X = 1F;
            this.robot1OrientationVis2.Y = 0F;
            // 
            // robot1OrientationVis3
            // 
            this.robot1OrientationVis3.IndicatorColor = System.Drawing.Color.Red;
            this.robot1OrientationVis3.IndicatorScale = 1F;
            this.robot1OrientationVis3.Location = new System.Drawing.Point(180, 8);
            this.robot1OrientationVis3.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.robot1OrientationVis3.Name = "robot1OrientationVis3";
            this.robot1OrientationVis3.Size = new System.Drawing.Size(75, 77);
            this.robot1OrientationVis3.TabIndex = 2;
            this.robot1OrientationVis3.X = 1F;
            this.robot1OrientationVis3.Y = 0F;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 136);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 20);
            this.label6.TabIndex = 4;
            this.label6.Text = "Gravity:";
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Controls.Add(this.robot1GravityVis1);
            this.flowLayoutPanel4.Controls.Add(this.robot1GravityVis2);
            this.flowLayoutPanel4.Controls.Add(this.robot1GravityVis3);
            this.flowLayoutPanel4.Location = new System.Drawing.Point(4, 161);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(264, 86);
            this.flowLayoutPanel4.TabIndex = 6;
            // 
            // robot1GravityVis1
            // 
            this.robot1GravityVis1.IndicatorColor = System.Drawing.Color.Green;
            this.robot1GravityVis1.IndicatorScale = 0.1F;
            this.robot1GravityVis1.Location = new System.Drawing.Point(6, 8);
            this.robot1GravityVis1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.robot1GravityVis1.Name = "robot1GravityVis1";
            this.robot1GravityVis1.Size = new System.Drawing.Size(75, 77);
            this.robot1GravityVis1.TabIndex = 0;
            this.robot1GravityVis1.X = 9.81F;
            this.robot1GravityVis1.Y = 0F;
            // 
            // robot1GravityVis2
            // 
            this.robot1GravityVis2.IndicatorColor = System.Drawing.Color.Blue;
            this.robot1GravityVis2.IndicatorScale = 0.1F;
            this.robot1GravityVis2.Location = new System.Drawing.Point(93, 8);
            this.robot1GravityVis2.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.robot1GravityVis2.Name = "robot1GravityVis2";
            this.robot1GravityVis2.Size = new System.Drawing.Size(75, 77);
            this.robot1GravityVis2.TabIndex = 1;
            this.robot1GravityVis2.X = 9.81F;
            this.robot1GravityVis2.Y = 0F;
            // 
            // robot1GravityVis3
            // 
            this.robot1GravityVis3.IndicatorColor = System.Drawing.Color.Red;
            this.robot1GravityVis3.IndicatorScale = 0.1F;
            this.robot1GravityVis3.Location = new System.Drawing.Point(180, 8);
            this.robot1GravityVis3.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.robot1GravityVis3.Name = "robot1GravityVis3";
            this.robot1GravityVis3.Size = new System.Drawing.Size(75, 77);
            this.robot1GravityVis3.TabIndex = 2;
            this.robot1GravityVis3.X = 9.81F;
            this.robot1GravityVis3.Y = 0F;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 252);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 20);
            this.label8.TabIndex = 10;
            this.label8.Text = "Position:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.57143F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.42857F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 328F));
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label9, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.robot1PositionX, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.robot1PositionY, 3, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 277);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(474, 31);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "X:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(116, 0);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 20);
            this.label9.TabIndex = 1;
            this.label9.Text = "Y:";
            // 
            // robot1PositionX
            // 
            this.robot1PositionX.AutoSize = true;
            this.robot1PositionX.Location = new System.Drawing.Point(36, 0);
            this.robot1PositionX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.robot1PositionX.Name = "robot1PositionX";
            this.robot1PositionX.Size = new System.Drawing.Size(67, 31);
            this.robot1PositionX.TabIndex = 2;
            this.robot1PositionX.Text = "Unknown";
            // 
            // robot1PositionY
            // 
            this.robot1PositionY.AutoSize = true;
            this.robot1PositionY.Location = new System.Drawing.Point(150, 0);
            this.robot1PositionY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.robot1PositionY.Name = "robot1PositionY";
            this.robot1PositionY.Size = new System.Drawing.Size(76, 20);
            this.robot1PositionY.TabIndex = 3;
            this.robot1PositionY.Text = "Unknown";
            // 
            // flowLayoutPanel7
            // 
            this.flowLayoutPanel7.Controls.Add(this.penDown);
            this.flowLayoutPanel7.Controls.Add(this.penUp);
            this.flowLayoutPanel7.Controls.Add(this.eraserDown);
            this.flowLayoutPanel7.Controls.Add(this.eraserUp);
            this.flowLayoutPanel7.Controls.Add(this.button1);
            this.flowLayoutPanel7.Location = new System.Drawing.Point(4, 318);
            this.flowLayoutPanel7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel7.Name = "flowLayoutPanel7";
            this.flowLayoutPanel7.Size = new System.Drawing.Size(474, 49);
            this.flowLayoutPanel7.TabIndex = 8;
            // 
            // penDown
            // 
            this.penDown.Location = new System.Drawing.Point(4, 5);
            this.penDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.penDown.Name = "penDown";
            this.penDown.Size = new System.Drawing.Size(93, 35);
            this.penDown.TabIndex = 7;
            this.penDown.Text = "pen down";
            this.penDown.UseVisualStyleBackColor = true;
            this.penDown.Click += new System.EventHandler(this.penDown_Click);
            // 
            // penUp
            // 
            this.penUp.Location = new System.Drawing.Point(105, 5);
            this.penUp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.penUp.Name = "penUp";
            this.penUp.Size = new System.Drawing.Size(72, 35);
            this.penUp.TabIndex = 8;
            this.penUp.Text = "pen up";
            this.penUp.UseVisualStyleBackColor = true;
            this.penUp.Click += new System.EventHandler(this.penUp_Click);
            // 
            // eraserDown
            // 
            this.eraserDown.Location = new System.Drawing.Point(185, 5);
            this.eraserDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.eraserDown.Name = "eraserDown";
            this.eraserDown.Size = new System.Drawing.Size(68, 35);
            this.eraserDown.TabIndex = 9;
            this.eraserDown.Text = "eraser down";
            this.eraserDown.UseVisualStyleBackColor = true;
            this.eraserDown.Click += new System.EventHandler(this.eraserDown_Click);
            // 
            // eraserUp
            // 
            this.eraserUp.Location = new System.Drawing.Point(261, 5);
            this.eraserUp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.eraserUp.Name = "eraserUp";
            this.eraserUp.Size = new System.Drawing.Size(90, 35);
            this.eraserUp.TabIndex = 10;
            this.eraserUp.Text = "eraser up";
            this.eraserUp.UseVisualStyleBackColor = true;
            this.eraserUp.Click += new System.EventHandler(this.eraserUp_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(359, 5);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 35);
            this.button1.TabIndex = 11;
            this.button1.Text = "disable all";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(4, 5);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(478, 369);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.flowLayoutPanel5);
            this.flowLayoutPanel2.Controls.Add(this.label5);
            this.flowLayoutPanel2.Controls.Add(this.flowLayoutPanel6);
            this.flowLayoutPanel2.Controls.Add(this.label10);
            this.flowLayoutPanel2.Controls.Add(this.tableLayoutPanel3);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(492, 390);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(515, 463);
            this.flowLayoutPanel2.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Robot 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Orientation:";
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Controls.Add(this.robot2OrientationVis1);
            this.flowLayoutPanel5.Controls.Add(this.robot2OrientationVis2);
            this.flowLayoutPanel5.Controls.Add(this.robot2OrientationVis3);
            this.flowLayoutPanel5.Location = new System.Drawing.Point(4, 45);
            this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(264, 86);
            this.flowLayoutPanel5.TabIndex = 6;
            // 
            // robot2OrientationVis1
            // 
            this.robot2OrientationVis1.IndicatorColor = System.Drawing.Color.Green;
            this.robot2OrientationVis1.IndicatorScale = 1F;
            this.robot2OrientationVis1.Location = new System.Drawing.Point(6, 8);
            this.robot2OrientationVis1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.robot2OrientationVis1.Name = "robot2OrientationVis1";
            this.robot2OrientationVis1.Size = new System.Drawing.Size(75, 77);
            this.robot2OrientationVis1.TabIndex = 0;
            this.robot2OrientationVis1.X = 1F;
            this.robot2OrientationVis1.Y = 0F;
            // 
            // robot2OrientationVis2
            // 
            this.robot2OrientationVis2.IndicatorColor = System.Drawing.Color.Blue;
            this.robot2OrientationVis2.IndicatorScale = 1F;
            this.robot2OrientationVis2.Location = new System.Drawing.Point(93, 8);
            this.robot2OrientationVis2.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.robot2OrientationVis2.Name = "robot2OrientationVis2";
            this.robot2OrientationVis2.Size = new System.Drawing.Size(75, 77);
            this.robot2OrientationVis2.TabIndex = 1;
            this.robot2OrientationVis2.X = 1F;
            this.robot2OrientationVis2.Y = 0F;
            // 
            // robot2OrientationVis3
            // 
            this.robot2OrientationVis3.IndicatorColor = System.Drawing.Color.Red;
            this.robot2OrientationVis3.IndicatorScale = 1F;
            this.robot2OrientationVis3.Location = new System.Drawing.Point(180, 8);
            this.robot2OrientationVis3.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.robot2OrientationVis3.Name = "robot2OrientationVis3";
            this.robot2OrientationVis3.Size = new System.Drawing.Size(75, 77);
            this.robot2OrientationVis3.TabIndex = 2;
            this.robot2OrientationVis3.X = 1F;
            this.robot2OrientationVis3.Y = 0F;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 136);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Gravity:";
            // 
            // flowLayoutPanel6
            // 
            this.flowLayoutPanel6.Controls.Add(this.robot2GravityVis1);
            this.flowLayoutPanel6.Controls.Add(this.robot2GravityVis2);
            this.flowLayoutPanel6.Controls.Add(this.robot2GravityVis3);
            this.flowLayoutPanel6.Location = new System.Drawing.Point(4, 161);
            this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.flowLayoutPanel6.Size = new System.Drawing.Size(264, 86);
            this.flowLayoutPanel6.TabIndex = 7;
            // 
            // robot2GravityVis1
            // 
            this.robot2GravityVis1.IndicatorColor = System.Drawing.Color.Green;
            this.robot2GravityVis1.IndicatorScale = 0.1F;
            this.robot2GravityVis1.Location = new System.Drawing.Point(6, 8);
            this.robot2GravityVis1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.robot2GravityVis1.Name = "robot2GravityVis1";
            this.robot2GravityVis1.Size = new System.Drawing.Size(75, 77);
            this.robot2GravityVis1.TabIndex = 0;
            this.robot2GravityVis1.X = 9.81F;
            this.robot2GravityVis1.Y = 0F;
            // 
            // robot2GravityVis2
            // 
            this.robot2GravityVis2.IndicatorColor = System.Drawing.Color.Blue;
            this.robot2GravityVis2.IndicatorScale = 0.1F;
            this.robot2GravityVis2.Location = new System.Drawing.Point(93, 8);
            this.robot2GravityVis2.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.robot2GravityVis2.Name = "robot2GravityVis2";
            this.robot2GravityVis2.Size = new System.Drawing.Size(75, 77);
            this.robot2GravityVis2.TabIndex = 1;
            this.robot2GravityVis2.X = 9.81F;
            this.robot2GravityVis2.Y = 0F;
            // 
            // robot2GravityVis3
            // 
            this.robot2GravityVis3.IndicatorColor = System.Drawing.Color.Red;
            this.robot2GravityVis3.IndicatorScale = 0.1F;
            this.robot2GravityVis3.Location = new System.Drawing.Point(180, 8);
            this.robot2GravityVis3.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.robot2GravityVis3.Name = "robot2GravityVis3";
            this.robot2GravityVis3.Size = new System.Drawing.Size(75, 77);
            this.robot2GravityVis3.TabIndex = 2;
            this.robot2GravityVis3.X = 9.81F;
            this.robot2GravityVis3.Y = 0F;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 252);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 20);
            this.label10.TabIndex = 12;
            this.label10.Text = "Position:";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.23529F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.76471F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 372F));
            this.tableLayoutPanel3.Controls.Add(this.label11, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label12, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.robot2PositionX, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.robot2PositionY, 3, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(4, 277);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(510, 31);
            this.tableLayoutPanel3.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 0);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 31);
            this.label11.TabIndex = 0;
            this.label11.Text = "X:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(105, 0);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(24, 20);
            this.label12.TabIndex = 1;
            this.label12.Text = "Y:";
            // 
            // robot2PositionX
            // 
            this.robot2PositionX.AutoSize = true;
            this.robot2PositionX.Location = new System.Drawing.Point(32, 0);
            this.robot2PositionX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.robot2PositionX.Name = "robot2PositionX";
            this.robot2PositionX.Size = new System.Drawing.Size(56, 31);
            this.robot2PositionX.TabIndex = 2;
            this.robot2PositionX.Text = "Unknown";
            // 
            // robot2PositionY
            // 
            this.robot2PositionY.AutoSize = true;
            this.robot2PositionY.Location = new System.Drawing.Point(141, 0);
            this.robot2PositionY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.robot2PositionY.Name = "robot2PositionY";
            this.robot2PositionY.Size = new System.Drawing.Size(76, 20);
            this.robot2PositionY.TabIndex = 3;
            this.robot2PositionY.Text = "Unknown";
            // 
            // flowLayoutPanel8
            // 
            this.flowLayoutPanel8.Controls.Add(this.changePositions);
            this.flowLayoutPanel8.Controls.Add(this.button2);
            this.flowLayoutPanel8.Controls.Add(this.drawAxes);
            this.flowLayoutPanel8.Controls.Add(this.drawFunction);
            this.flowLayoutPanel8.Controls.Add(this.drawNiceFunction);
            this.flowLayoutPanel8.Controls.Add(this.stop);
            this.flowLayoutPanel8.Controls.Add(this.flowLayoutPanel9);
            this.flowLayoutPanel8.Controls.Add(this.moveForward);
            this.flowLayoutPanel8.Controls.Add(this.btnStraightSamples);
            this.flowLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel8.Location = new System.Drawing.Point(492, 5);
            this.flowLayoutPanel8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel8.Name = "flowLayoutPanel8";
            this.flowLayoutPanel8.Size = new System.Drawing.Size(515, 375);
            this.flowLayoutPanel8.TabIndex = 12;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(192, 5);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(158, 35);
            this.button2.TabIndex = 8;
            this.button2.Text = "draw rectangle";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // drawAxes
            // 
            this.drawAxes.Location = new System.Drawing.Point(358, 5);
            this.drawAxes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.drawAxes.Name = "drawAxes";
            this.drawAxes.Size = new System.Drawing.Size(112, 35);
            this.drawAxes.TabIndex = 12;
            this.drawAxes.Text = "draw axes";
            this.drawAxes.UseVisualStyleBackColor = true;
            this.drawAxes.Click += new System.EventHandler(this.drawAxes_Click);
            // 
            // drawFunction
            // 
            this.drawFunction.Location = new System.Drawing.Point(4, 50);
            this.drawFunction.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.drawFunction.Name = "drawFunction";
            this.drawFunction.Size = new System.Drawing.Size(126, 35);
            this.drawFunction.TabIndex = 9;
            this.drawFunction.Text = "draw sin";
            this.drawFunction.UseVisualStyleBackColor = true;
            this.drawFunction.Click += new System.EventHandler(this.drawSineFunction_Click);
            // 
            // drawNiceFunction
            // 
            this.drawNiceFunction.Location = new System.Drawing.Point(138, 50);
            this.drawNiceFunction.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.drawNiceFunction.Name = "drawNiceFunction";
            this.drawNiceFunction.Size = new System.Drawing.Size(164, 35);
            this.drawNiceFunction.TabIndex = 13;
            this.drawNiceFunction.Text = "draw nice function";
            this.drawNiceFunction.UseVisualStyleBackColor = true;
            this.drawNiceFunction.Click += new System.EventHandler(this.drawNiceFunction_Click);
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(310, 50);
            this.stop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(112, 35);
            this.stop.TabIndex = 10;
            this.stop.Text = "Stop";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // flowLayoutPanel9
            // 
            this.flowLayoutPanel9.Controls.Add(this.label13);
            this.flowLayoutPanel9.Controls.Add(this.kp);
            this.flowLayoutPanel9.Controls.Add(this.label14);
            this.flowLayoutPanel9.Controls.Add(this.ki);
            this.flowLayoutPanel9.Controls.Add(this.label15);
            this.flowLayoutPanel9.Controls.Add(this.kd);
            this.flowLayoutPanel9.Controls.Add(this.btnStartControllers);
            this.flowLayoutPanel9.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel9.Location = new System.Drawing.Point(4, 95);
            this.flowLayoutPanel9.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel9.Name = "flowLayoutPanel9";
            this.flowLayoutPanel9.Size = new System.Drawing.Size(510, 189);
            this.flowLayoutPanel9.TabIndex = 11;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(4, 0);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(28, 20);
            this.label13.TabIndex = 0;
            this.label13.Text = "Kp";
            // 
            // kp
            // 
            this.kp.Location = new System.Drawing.Point(4, 25);
            this.kp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.kp.Name = "kp";
            this.kp.Size = new System.Drawing.Size(148, 26);
            this.kp.TabIndex = 1;
            this.kp.TextChanged += new System.EventHandler(this.kp_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(4, 56);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(22, 20);
            this.label14.TabIndex = 2;
            this.label14.Text = "Ki";
            // 
            // ki
            // 
            this.ki.Location = new System.Drawing.Point(4, 81);
            this.ki.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ki.Name = "ki";
            this.ki.Size = new System.Drawing.Size(148, 26);
            this.ki.TabIndex = 3;
            this.ki.TextChanged += new System.EventHandler(this.ki_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(4, 112);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(28, 20);
            this.label15.TabIndex = 4;
            this.label15.Text = "Kd";
            // 
            // kd
            // 
            this.kd.Location = new System.Drawing.Point(4, 137);
            this.kd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.kd.Name = "kd";
            this.kd.Size = new System.Drawing.Size(148, 26);
            this.kd.TabIndex = 5;
            this.kd.TextChanged += new System.EventHandler(this.kd_TextChanged);
            // 
            // moveForward
            // 
            this.moveForward.Location = new System.Drawing.Point(3, 292);
            this.moveForward.Name = "moveForward";
            this.moveForward.Size = new System.Drawing.Size(124, 44);
            this.moveForward.TabIndex = 14;
            this.moveForward.Text = "move forward";
            this.moveForward.UseVisualStyleBackColor = true;
            this.moveForward.Click += new System.EventHandler(this.moveForward_Click);
            // 
            // btnStraightSamples
            // 
            this.btnStraightSamples.Location = new System.Drawing.Point(133, 292);
            this.btnStraightSamples.Name = "btnStraightSamples";
            this.btnStraightSamples.Size = new System.Drawing.Size(135, 44);
            this.btnStraightSamples.TabIndex = 15;
            this.btnStraightSamples.Text = "straight samples";
            this.btnStraightSamples.UseVisualStyleBackColor = true;
            this.btnStraightSamples.Click += new System.EventHandler(this.btnStraightSamples_Click);
            // 
            // btnStartControllers
            // 
            this.btnStartControllers.Location = new System.Drawing.Point(159, 3);
            this.btnStartControllers.Name = "btnStartControllers";
            this.btnStartControllers.Size = new System.Drawing.Size(165, 60);
            this.btnStartControllers.TabIndex = 6;
            this.btnStartControllers.Text = "start controllers";
            this.btnStartControllers.UseVisualStyleBackColor = true;
            this.btnStartControllers.Click += new System.EventHandler(this.btnStartControllers_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 917);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.flowLayoutPanel8.ResumeLayout(false);
            this.flowLayoutPanel9.ResumeLayout(false);
            this.flowLayoutPanel9.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button changePositions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private DirectionVisualizer robot1OrientationVis1;
        private DirectionVisualizer robot1OrientationVis2;
        private DirectionVisualizer robot1OrientationVis3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private DirectionVisualizer robot1GravityVis1;
        private DirectionVisualizer robot1GravityVis2;
        private DirectionVisualizer robot1GravityVis3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private DirectionVisualizer robot2OrientationVis1;
        private DirectionVisualizer robot2OrientationVis2;
        private DirectionVisualizer robot2OrientationVis3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
        private DirectionVisualizer robot2GravityVis1;
        private DirectionVisualizer robot2GravityVis2;
        private DirectionVisualizer robot2GravityVis3;
        private System.Windows.Forms.Button penDown;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
        private System.Windows.Forms.Button penUp;
        private System.Windows.Forms.Button eraserDown;
        private System.Windows.Forms.Button eraserUp;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel8;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label robot1PositionX;
        private System.Windows.Forms.Label robot1PositionY;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label robot2PositionX;
        private System.Windows.Forms.Label robot2PositionY;
        private System.Windows.Forms.Button drawFunction;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox kp;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox ki;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox kd;
        private System.Windows.Forms.Button drawAxes;
        private System.Windows.Forms.Button drawNiceFunction;
        private System.Windows.Forms.Button moveForward;
        private System.Windows.Forms.Button btnStraightSamples;
        private System.Windows.Forms.Button btnStartControllers;
    }
}

