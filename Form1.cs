using provaSuficiencia.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace provaSuficiencia
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void especialistasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEspecialistas esp = new FormEspecialistas();
            esp.MdiParent = this;

            esp.Show();
        }

        private void especialidadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEspecialidades esp = new FormEspecialidades();
            esp.MdiParent = this;

            esp.Show();
        }

        private void examesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormExames exa = new FormExames();
            exa.MdiParent = this;

            exa.Show();
        }

        private void pesquisarExamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConsulta exa = new FormConsulta();
            exa.MdiParent = this;

            exa.Show();
        }
    }
}
