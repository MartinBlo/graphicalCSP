using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicalConstraintProgramming
{
    public partial class WaitingScreen : Form
    {
        private List<Thread> t;

        public WaitingScreen(List<Thread> t)
        {
            InitializeComponent();
            
            this.t = t;

            foreach(Thread thread in t)
            {
                thread.Start();
            }

            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(Thread thread in t) thread.Abort();
            timer1.Stop();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            Boolean oneAlive = false;

            foreach (Thread thread in t)
            {

                if (thread.IsAlive)
                {
                    oneAlive = true;
                }
                

                if (!thread.IsAlive && MiniZincInterface.gotResult)
                {
                    this.Close();
                }
            }

            if (!oneAlive)
            {
                this.Close();
            }
        }
    }
}
