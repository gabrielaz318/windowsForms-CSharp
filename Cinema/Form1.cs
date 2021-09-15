using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient ;

namespace Cinema
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private MySqlConnectionStringBuilder ConexaoBanco()
        {
            MySqlConnectionStringBuilder conexaoBD = new MySqlConnectionStringBuilder();
            conexaoBD.Server = "127.0.0.1";
            conexaoBD.Database = "cinema";
            conexaoBD.UserID = "root";
            conexaoBD.Password = "";
            conexaoBD.SslMode = 0;
            return conexaoBD;
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparCampos();
        }

        private void limparCampos()
        {
            tbNome.Clear();
            tbGenero.Clear();
            tbDiretor.Clear();
            tbAno.Clear();
            tbNome.Focus();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            atualizaDados();
        }

        private void atualizaDados()
        {
            MySqlConnectionStringBuilder conexaoBD = ConexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();
                comandoMySql.CommandText = "SELECT * FROM lancamentos";
                MySqlDataReader reader = comandoMySql.ExecuteReader();

                dgDados.Rows.Clear();

                while (reader.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)dgDados.Rows[0].Clone();//FAZ UM CAST E CLONA A LINHA DA TABELA
                    row.Cells[0].Value = reader.GetInt32(0);//ID
                    row.Cells[1].Value = reader.GetString(1);//NOME
                    row.Cells[2].Value = reader.GetString(2);//GENERO
                    row.Cells[3].Value = reader.GetString(3);//DIRETOR
                    row.Cells[4].Value = reader.GetString(4);//ANO
                    dgDados.Rows.Add(row);//ADICIONO A LINHA NA TABELA
                }

                realizaConexacoBD.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
                Console.WriteLine(ex.Message);
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conexaoBD = ConexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();

                comandoMySql.CommandText = "INSERT INTO lancamentos (nome,genero,diretor,ano) " +
                    "VALUES('" + tbNome.Text + "', '" + tbGenero.Text + "','" + tbDiretor.Text + "', " + Convert.ToInt16(tbAno.Text) + ")";
                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close();
                MessageBox.Show("Inserido com sucesso");
                atualizaDados();
                limparCampos();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
