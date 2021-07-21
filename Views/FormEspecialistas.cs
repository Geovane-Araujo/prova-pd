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
    public partial class FormEspecialistas : Form
    {
        Boolean AddMode = true;
        Boolean EditMode = false;
        Int32 idespecialista = 0;
        AConnections aConnections = new AConnections();
        public FormEspecialistas()
        {
            InitializeComponent();
            GetAll();
            Getespecialidades();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Validate())
                Save();
        }

        public Boolean Validate()
        {
            Boolean ret = false;
            if(txtcrm.Text == "")
            {
                MessageBox.Show("Por Favor Insira o CRM");
            }
            else if (txtnome.Text.Equals(""))
            {
                MessageBox.Show("Por Favor Insira o Nome do Especialista");
            }
            else if (cbEspecialidade.Text.Equals(""))
            {
                MessageBox.Show("Por Favor selecione a Especialidade");
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        public void ClearForm()
        {

            txtcrm.Text = "";
            txtnome.Text = "";
            cbEspecialidade.Text = "";
            AddMode = true;
            EditMode = false;
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
                    sql = "set IDENTITY_INSERT especialistas on; insert into especialistas(crm,nome,especialidadesId) values(@CRM,@NOME,@IDespecialidades);set IDENTITY_INSERT especialistas off;";
                }
                else if (EditMode)
                    sql = "update especialistas set crm=@CRM,nome=@Nome,EspecialidadeID=@IDEspecialidades where crm=@crm";
                else
                    sql = "delete from especialistas where crm = @crm";

                command = new SqlCommand(sql, con);

                command.Parameters.AddWithValue("@crm",Convert.ToInt32(txtcrm.Text));
                command.Parameters.AddWithValue("@Nome", txtnome.Text);
                String selecionado = cbEspecialidade.Text;
                String[] sl = selecionado.Split("-");
                command.Parameters.AddWithValue("@IDespecialidades", Convert.ToInt32(sl[0]));
                command.ExecuteNonQuery();

                command.Dispose();
                con.Close();
                GetAll();
                ClearForm();
                MessageBox.Show("salvo com sucesso!!!");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void GetAll()
        {

            SqlConnection con = aConnections.NewInstance();
            SqlCommand command = null;
            SqlDataReader dr = null;

            String sql = "";
            try
            {

                sql = "select crm,nome,especialidades.descricao, especialidades.id from especialistas inner join especialidades on especialidades.id = especialidadesid";

                command = new SqlCommand(sql, con);
                dr = command.ExecuteReader();

                grid.Rows.Clear();
                while (dr.Read())
                {
                    Int32 crm = dr.GetInt32(dr.GetOrdinal("crm"));
                    String nome = dr.GetString(dr.GetOrdinal("nome"));
                    String especialidade = dr.GetString(dr.GetOrdinal("descricao"));
                    Int32 idespecialidade = dr.GetInt32(dr.GetOrdinal("id"));
                    grid.Rows.Add(crm, nome, especialidade, idespecialidade);
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

        public void Getespecialidades()
        {

            SqlConnection con = aConnections.NewInstance();
            SqlCommand command = null;
            SqlDataReader dr = null;

            String sql = "";
            try
            {

                sql = "select id, descricao from especialidades where id > 0";

                command = new SqlCommand(sql, con);
                dr = command.ExecuteReader();
                

                while (dr.Read())
                {
                    Int32 crm = dr.GetInt32(dr.GetOrdinal("id"));
                    String especialidade = dr.GetString(dr.GetOrdinal("descricao"));
                    cbEspecialidade.Items.Add(crm + " - " + especialidade);
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

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grid_DoubleClick(object sender, EventArgs e)
        {
            //int index = e.;
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow row = grid.Rows[index];
            AddMode = false;
            EditMode = true;
            // idespecialista = Convert.ToInt32(row.Cells[0].Value.ToString());
            txtcrm.Text = row.Cells[0].Value.ToString();
            txtnome.Text = row.Cells[1].Value.ToString();
            cbEspecialidade.Text = row.Cells[3].Value.ToString() + " - " + row.Cells[2].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!txtcrm.Text.Equals(""))
            {
                AddMode = false;
                EditMode = false;
                Save();
            }
                
        }

        private void Especialistas_Load(object sender, EventArgs e)
        {

        }
    }
}