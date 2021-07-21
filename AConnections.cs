using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace provaSuficiencia
{
    class AConnections
    {
        public SqlConnection NewInstance()
        {
            SqlConnection cnn = new SqlConnection("Data Source=" +Environment.MachineName+";Initial Catalog=Prova;User=sa;Password=Atmus@#4080");

            try
            {
                cnn.Open();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return cnn;
        }
    }
}
