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
    public partial class FormExames : Form
    {
        Boolean AddMode = true;
        Boolean EditMode = false;
        Int32 idEspecialidade = 0;
        Int32 idExame = 0;
        AConnections aConnections = new AConnections();
        public FormExames()
        {
            InitializeComponent();
            GetAll();
            GetEspecialistas();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Validate())
                Save();
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
                    sql = " insert into exames(Descricao,custo,EspecialistaID) values(@Descricao,@custo,@EspecialistaID);";
                }
                else if (EditMode)
                    sql = "update exames set Descricao=@Descricao,custo=@custo,EspecialistaID=@EspecialistaID where ExameID=@ExameID";
                else
                    sql = "delete from exames where ExameID = @ExameID";

                command = new SqlCommand(sql, con);
                if (!AddMode)
                    command.Parameters.AddWithValue("@ExameID", idExame);
                command.Parameters.AddWithValue("@Descricao", txtnome.Text);
                command.Parameters.AddWithValue("@custo", Convert.ToDecimal(txtValor.Text));
                String selecionado = cbEspecialidade.Text;
                String[] sl = selecionado.Split("-");
                command.Parameters.AddWithValue("@EspecialistaID", Convert.ToInt32(sl[0]));
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
            if (txtValor.Text == "")
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

            txtValor.Text = "";
            txtnome.Text = "";
            cbEspecialidade.Text = "";
            idExame = 0;
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

                sql = "select ExameID,Descricao,especialistas.nome,especialistas.crm, custo from exames inner join especialistas on crm = EspecialistaID";

                command = new SqlCommand(sql, con);
                dr = command.ExecuteReader();

                grid.Rows.Clear();
                while (dr.Read())
                {
                    Int32 exameId = dr.GetInt32(dr.GetOrdinal("ExameID"));
                    String descricao = dr.GetString(dr.GetOrdinal("descricao"));
                    String nome = dr.GetString(dr.GetOrdinal("nome"));
                    Int32 crm = dr.GetInt32(dr.GetOrdinal("crm"));
                    String valor = dr.GetDecimal(dr.GetOrdinal("custo")).ToString("F");
                    grid.Rows.Add(exameId, descricao, nome, valor, crm);
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

        public void GetEspecialistas()
        {

            SqlConnection con = aConnections.NewInstance();
            SqlCommand command = null;
            SqlDataReader dr = null;

            String sql = "";
            try
            {

                sql = "select CRM, nome from especialistas where CRM > 0";

                command = new SqlCommand(sql, con);
                dr = command.ExecuteReader();


                while (dr.Read())
                {
                    Int32 crm = dr.GetInt32(dr.GetOrdinal("CRM"));
                    String especialidade = dr.GetString(dr.GetOrdinal("nome"));
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

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow row = grid.Rows[index];
            AddMode = false;
            EditMode = true;
            txtValor.Text = row.Cells[3].Value.ToString();
            txtnome.Text = row.Cells[1].Value.ToString();
            idExame = Convert.ToInt32(row.Cells[0].Value.ToString());
            cbEspecialidade.Text = row.Cells[4].Value.ToString() + " - " + row.Cells[2].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (idExame != 0)
            {
                AddMode = false;
                EditMode = false;
                Save();
            }
        }
    }
}
