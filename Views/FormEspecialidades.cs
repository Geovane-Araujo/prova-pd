using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace provaSuficiencia.Views
{
    public partial class FormEspecialidades : Form
    {
        Boolean AddMode = true;
        Boolean EditMode = false;
        Int32 id = 0;
        AConnections aConnections = new AConnections();
        public FormEspecialidades()
        {
            InitializeComponent();
            GetAll();
        }

        private void Especialidades_Load(object sender, EventArgs e)
        {

        }


        public void Save()
        {

            SqlConnection con = aConnections.NewInstance();
            SqlCommand command = null;

            String sql = "";
            try
            {

                if (AddMode)
                {
                    sql = "insert into especialidades(descricao) values(@Descricao)";
                }
                else if (EditMode)
                    sql = "update especialidades set descricao=@Descricao where id=@ID";
                else
                    sql = "delete from especialidades where id=@ID";

                command = new SqlCommand(sql, con);
                if(!AddMode)
                    command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@Descricao", txtnome.Text);
                command.ExecuteNonQuery();

                command.Dispose();
                con.Close();
                GetAll();
                ClearForm();
                MessageBox.Show("salvo com sucesso!!!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public Boolean Validate()
        {
            Boolean ret = false;
            if (txtnome.Text.Equals(""))
            {
                MessageBox.Show("Por Favor Insira a Descricao");
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        public void ClearForm()
        {

            txtnome.Text = "";
            id = 0;
            AddMode = true;
            EditMode = false;
        }
        public void GetAll()
        {

            SqlConnection con = aConnections.NewInstance();
            SqlCommand command = null;
            SqlDataReader dr = null;

            String sql = "";
            try
            {

                sql = "select id,descricao from especialidades ";

                command = new SqlCommand(sql, con);
                dr = command.ExecuteReader();

                grid.Rows.Clear();
                while (dr.Read())
                {
                    Int32 id = dr.GetInt32(dr.GetOrdinal("id"));
                    String especialidade = dr.GetString(dr.GetOrdinal("descricao"));
                    grid.Rows.Add(id, especialidade);
                }

                dr.Close();
                command.Dispose();
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Validate()){
                Save();
            }
            
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow row = grid.Rows[index];
            AddMode = false;
            EditMode = true;
            txtnome.Text = row.Cells[1].Value.ToString();
            id = Convert.ToInt32(row.Cells[0].Value.ToString());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (id != 0)
            {
                AddMode = false;
                EditMode = false;
                Save();
            }
        }
    }
}
