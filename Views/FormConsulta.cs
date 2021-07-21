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
    public partial class FormConsulta : Form
    {
        AConnections aConnections = new AConnections();
        public FormConsulta()
        {
            InitializeComponent();
            GetEspecialistas();
            Getespecialidades();
            GetAll();
        }

        private void FormConsulta_Load(object sender, EventArgs e)
        {

        }

        public void GetAll()
        {
            
            SqlConnection con = aConnections.NewInstance();
            SqlCommand command = null;
            SqlDataReader dr = null;

            String sql = "";
            try
            {
                Int32 id = 0;
                String filters = "";
                String selecionado = cbespecialistas.Text;
                String[] sl = selecionado.Split("-");
                if (!cbespecialistas.Text.Equals(""))
                {
                    filters = " and EspecialistaID = " + sl[0];
                }
                if(cbespecialistas.Text.Equals("0 - Todos"))
                {
                    filters = "";
                }

                sql = "select ExameID,exames.Descricao,especialistas.nome,especialistas.crm, custo,Especialidades.descricao as espe  " +
                    "from exames " +
                    "inner join especialistas on crm = EspecialistaID " +
                    " inner join Especialidades on especialistas.especialidadesId = Especialidades.id " +
                    "where ExameID > 0 " + filters;

                command = new SqlCommand(sql, con);
                dr = command.ExecuteReader();

                grid.Rows.Clear();
                while (dr.Read())
                {
                    Int32 exameId = dr.GetInt32(dr.GetOrdinal("ExameID"));
                    String descricao = dr.GetString(dr.GetOrdinal("descricao"));
                    String nome = dr.GetString(dr.GetOrdinal("nome"));
                    String especialidade = dr.GetString(dr.GetOrdinal("espe"));
                    Int32 crm = dr.GetInt32(dr.GetOrdinal("crm"));
                    String valor = dr.GetDecimal(dr.GetOrdinal("custo")).ToString("F");
                    grid.Rows.Add(exameId, descricao, nome, especialidade, valor, crm);
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

                cbEspecialidades.Items.Add("0 - Todos");
                while (dr.Read())
                {
                    Int32 crm = dr.GetInt32(dr.GetOrdinal("id"));
                    String especialidade = dr.GetString(dr.GetOrdinal("descricao"));
                    cbEspecialidades.Items.Add(crm + " - " + especialidade);
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

                cbespecialistas.Items.Add("0 - Todos");
                while (dr.Read())
                {
                    Int32 crm = dr.GetInt32(dr.GetOrdinal("CRM"));
                    String especialidade = dr.GetString(dr.GetOrdinal("nome"));
                    cbespecialistas.Items.Add(crm + " - " + especialidade);
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
            GetAll();
        }
    }
}
