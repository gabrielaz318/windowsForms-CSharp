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
            tbId.Clear();
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
                    DataGridViewRow row = (DataGridViewRow)dgDados.Rows[0].Clone();
                    row.Cells[0].Value = reader.GetInt32(0);
                    row.Cells[1].Value = reader.GetString(1);
                    row.Cells[2].Value = reader.GetString(2);
                    row.Cells[3].Value = reader.GetString(3);
                    row.Cells[4].Value = reader.GetString(4);
                    dgDados.Rows.Add(row);
                }

                realizaConexacoBD.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não conseguimos realizar a conexão com o banco de dados!");
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
                MessageBox.Show(comandoMySql.CommandText.ToString());
                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close();
                MessageBox.Show("Inserido com sucesso");
                atualizaDados();
                limparCampos();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            if (tbId.Text != "")
            {
                MySqlConnectionStringBuilder conexaoBD = ConexaoBanco();
                MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
                try
                {
                    realizaConexacoBD.Open();

                    MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();
                    comandoMySql.CommandText = "DELETE FROM lancamentos WHERE id = " + tbId.Text + "";

                    comandoMySql.ExecuteNonQuery();

                    realizaConexacoBD.Close();
                    MessageBox.Show("Registro número: " + tbId.Text + " deletado com sucesso!");
                    atualizaDados();
                    limparCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Houve um problema ao tentar deletar registro!");
                }
            }
            else
            {
                MessageBox.Show("Selecione um registro para excluir!");
            }
        }

        private void dgDados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgDados.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgDados.CurrentRow.Selected = true;
                tbId.Text = dgDados.Rows[e.RowIndex].Cells["colID"].FormattedValue.ToString();
                tbNome.Text = dgDados.Rows[e.RowIndex].Cells["colNome"].FormattedValue.ToString();
                tbGenero.Text = dgDados.Rows[e.RowIndex].Cells["colGenero"].FormattedValue.ToString();
                tbDiretor.Text = dgDados.Rows[e.RowIndex].Cells["colDiretor"].FormattedValue.ToString();
                tbAno.Text = dgDados.Rows[e.RowIndex].Cells["colAno"].FormattedValue.ToString();
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (tbId.Text != "")
            {
                MySqlConnectionStringBuilder conexaoBD = ConexaoBanco();
                MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
                try
                {
                    realizaConexacoBD.Open();

                    MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();
                    comandoMySql.CommandText = "UPDATE lancamentos SET nome = '" +
                        "" + tbNome.Text + "', " +
                        "genero = '" + tbGenero.Text + "', " +
                        "diretor = '" + tbDiretor.Text + "', " +
                        "ano = '" + tbAno.Text + "' " +
                        " WHERE id = " + Convert.ToInt16(tbId.Text) + "";
                    comandoMySql.ExecuteNonQuery();

                    realizaConexacoBD.Close();
                    MessageBox.Show("Registro número: " + tbId.Text + " atualizado com sucesso!");
                    atualizaDados();
                    limparCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Houve um erro ao atualizar o registro!\n\n" + ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("Selecione um registro antes de tentar alterar!");
            }
        }
    }
}
