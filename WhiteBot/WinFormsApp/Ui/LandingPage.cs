using WhiteBot.RobotController.MLRobotController;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhiteBot.Ui
{
    public partial class LandingPage : Form
    {
        private Form1 form;

        public LandingPage()
        {
            InitializeComponent();
            form = new Form1();
        }

        private void ShowOtherForm()
        {
            this.Hide();
            form.Show();
            form.FormClosed += (object sender2, FormClosedEventArgs e2) => this.Close();
        }

        private void btnStartPID_Click(object sender, EventArgs e)
        {
            //create PID controller and pass it to the new form
            form.InitializeRobot(RobotControllerType.PidController);

            ShowOtherForm();
        }
        
        private void btnStartML_Click(object sender, EventArgs e)
        {
            form.InitializeRobot(RobotControllerType.MLController);
            ShowOtherForm();
        }

        private void btnLoadML_Click(object sender, EventArgs e)
        {

            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.CurrentDirectory;
            var result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;

            var filepath = dialog.FileName;
            var learnedData = Learner.LoadFromFile(null, Vector2.Zero, filepath);
            form.InitializeRobot(learnedData);

            ShowOtherForm();
        }

    }
}
