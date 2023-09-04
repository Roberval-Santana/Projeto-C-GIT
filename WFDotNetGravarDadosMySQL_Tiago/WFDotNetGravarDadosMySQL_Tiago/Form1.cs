using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace WFDotNetGravarDadosMySQL_Tiago
{
    public partial class FrmCarro : Form
    {
        

        private MySqlConnection conexao;
        private string data_source = "datasource=localhost;username=root;password=admin;database=transporte";
        private int ?id_Carro_Selecionado = null;

        public FrmCarro()
        {
            InitializeComponent();
            lstCarro.View = View.Details;
            lstCarro.LabelEdit = true;
            lstCarro.AllowColumnReorder = true;
            lstCarro.FullRowSelect = true;
            lstCarro.GridLines = true;
            
            lstCarro.Columns.Add("Id", 50, HorizontalAlignment.Left);
            lstCarro.Columns.Add("Descrição", 150, HorizontalAlignment.Left);
            lstCarro.Columns.Add("Placa", 150, HorizontalAlignment.Left);
            carrega_Carro();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //criar Conexao  com Mysql
                conexao = new MySqlConnection(data_source);

                conexao.Open();

                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = conexao;


                if (id_Carro_Selecionado == null)
                {
                    cmd.CommandText = "Insert into Carro( carr_tx_descricao, carr_tx_placa) VALUES " +
                        " ( @descricao, @placa)";

                    cmd.Parameters.AddWithValue("@descricao", txtDescricao.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@placa", txtPlaca.Text.ToUpper());

                    cmd.Prepare();
                     
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();
                    /*   string sql = "Insert into Carro( carr_tx_descricao, carr_tx_placa) VALUES " +
                           " ('" + txtDescricao.Text.ToUpper() + "', '" + txtPlaca.Text.ToUpper() + "')";

                        MySqlCommand comando = new MySqlCommand(sql, conexao);
                       conexao.Open();
                       comando.ExecuteReader(); */
                    MessageBox.Show("CARRO cadastrado com sucesso!",
                        "Inclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else 
                {

                    cmd.CommandText = "UPDATE Carro SET  carr_tx_descricao = @descricao, carr_tx_placa = @placa " +
                        "where carr_id_carro = @id";

                    cmd.Parameters.AddWithValue("@descricao", txtDescricao.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@placa", txtPlaca.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@id", id_Carro_Selecionado);
                    cmd.Prepare();

                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();

                    MessageBox.Show("CARRO Alterado com sucesso!",
                        "Alteração", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                limpa_formulario();
                carrega_Carro();

            }
            catch (MySqlException Ex)
            {
                MessageBox.Show("Erro" + Ex.Number + "Ocorreu : " + Ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            finally {
                conexao.Close();
            }
        }

        private void carrega_Carro()
        {
            try
            {
                /*   --------------------------------------------- */
                //criar Conexao  com Mysql
                conexao = new MySqlConnection(data_source);
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = conexao;

                cmd.CommandText = "Select * FROM Carro ORDER BY carr_tx_descricao DESC";
                cmd.Prepare();

                MySqlDataReader listaCarroSelect = cmd.ExecuteReader();

                lstCarro.Items.Clear();

                while (listaCarroSelect.Read())
                {
                    string[] linhacarro = {
                    listaCarroSelect.GetString(0),
                    listaCarroSelect.GetString(1),
                    listaCarroSelect.GetString(2),
                    };
                    var linha_listview = new ListViewItem(linhacarro);

                    lstCarro.Items.Add(linha_listview);
                }


            }
            catch (MySqlException Ex)
            {
                MessageBox.Show("Erro" + Ex.Number + "Ocorreu : " + Ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                /*   --------------------------------------------- */
                //criar Conexao  com Mysql
                conexao = new MySqlConnection(data_source);
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = conexao;

                cmd.CommandText =  "Select * FROM Carro WHERE " +
                            " carr_tx_descricao LIKE @strBusca OR carr_tx_placa LIKE @strBusca";


                cmd.Parameters.AddWithValue("@strBusca", "%" + txtBusca.Text.ToUpper()+ "%");
                cmd.Prepare();

                /*   --------------------------------------------- */
                /* versao Antiga antes do statems
                string strBusca = "'%" + txtBusca.Text.ToUpper() + "%'";
                //criar Conexao  com Mysql
                conexao = new MySqlConnection(data_source);

                string sql = "Select * FROM Carro " +
                    " WHERE " +
                    " carr_tx_descricao LIKE " + strBusca + " OR carr_tx_placa LIKE " + strBusca;

                */

                                              
                MySqlDataReader listaCarroSelect = cmd.ExecuteReader();

                lstCarro.Items.Clear();                    

                while(listaCarroSelect.Read())
                {
                    string[] linhacarro = {
                    listaCarroSelect.GetString(0),
                    listaCarroSelect.GetString(1),
                    listaCarroSelect.GetString(2),
                    };
                    var linha_listview = new ListViewItem(linhacarro);

                     lstCarro.Items.Add(linha_listview);
                } 
                 

            }
            catch (MySqlException Ex)
            {
                MessageBox.Show("Erro" + Ex.Number + "Ocorreu : " + Ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }

        


        private void button3_Click(object sender, EventArgs e)
        {
            limpa_formulario();
        }

        private void limpa_formulario()
        {
            id_Carro_Selecionado = null;
            txtDescricao.Text = String.Empty;
            txtPlaca.Text = "";
            button3.Visible = false;
            txtDescricao.Focus();
        }

        private void lstCarro_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_Selecionados = lstCarro.SelectedItems;
            foreach (ListViewItem item in itens_Selecionados)
            {
                id_Carro_Selecionado = Convert.ToInt32(item.SubItems[0].Text);
                txtDescricao.Text = item.SubItems[1].Text;
                txtPlaca.Text = item.SubItems[2].Text;
                button3.Visible = true;
            } 


        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            excluir_Carro();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            //butao excluir
            excluir_Carro();
        }

        private void excluir_Carro()
        {
            try
            {
                DialogResult conf = MessageBox.Show("Confirma a exclusão do Carro",
                                                    "Exclusao de carro",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Warning);
                if (conf == DialogResult.Yes)
                {
                    conexao = new MySqlConnection(data_source);
                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand();

                    cmd.Connection = conexao;
                    cmd.Parameters.AddWithValue("@id", id_Carro_Selecionado);
                    cmd.CommandText = "DELETE  FROM Carro WHERE carr_ID_CARRO=@id";
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("CARRO Excluido com sucesso!",
                                    "Exclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpa_formulario();
                    carrega_Carro();                    
                    // MessageBox.Show("Preparando para Exclusao Id ->" + id_Carro_Selecionado);
                }

            }
            catch (MySqlException Ex)
            {
                MessageBox.Show("Erro" + Ex.Number + "Ocorreu : " + Ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            finally
            {
                conexao.Close();
            }

        }
    }
}
