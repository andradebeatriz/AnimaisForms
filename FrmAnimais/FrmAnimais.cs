using FrmAnimais.animaisTableAdapters;
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace FrmAnimais
{
    public partial class FrmAnimal : Form
    {
        bool incluir = false;
        public FrmAnimal()
        {
            InitializeComponent();
        }

        private void FrmAnimal_Load(object sender, EventArgs e)
        {
            // estilo ao DataGridView
            dgvAnimal.EnableHeadersVisualStyles = false;

            // cabeçalho
            dgvAnimal.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
            {
                BackColor = Color.Peru,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };

            // estilo padrão das células
            dgvAnimal.DefaultCellStyle = new DataGridViewCellStyle()
            {
                BackColor = Color.Bisque,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10),
                SelectionBackColor = Color.Sienna,
                SelectionForeColor = Color.Snow
            };

            // linhas alternadas
            dgvAnimal.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle()
            {
                BackColor = Color.BurlyWood
            };

            // outras personalizações visuais
            dgvAnimal.BackgroundColor = Color.Bisque;
            dgvAnimal.BorderStyle = BorderStyle.None;
            dgvAnimal.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAnimal.GridColor = Color.SaddleBrown;

            dgvAnimal.RowHeadersDefaultCellStyle.BackColor = Color.Bisque;
            dgvAnimal.RowHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvAnimal.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Sienna;
            dgvAnimal.RowHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvAnimal.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;


            //ICONES
            btnIncluir.Image = Properties.Resources.plus;
            btnIncluir.ImageAlign = ContentAlignment.MiddleLeft;

            btnEditar.Image = Properties.Resources.pencil;
            btnEditar.ImageAlign = ContentAlignment.MiddleLeft;

            btnApagar.Image = Properties.Resources.trash;
            btnApagar.ImageAlign = ContentAlignment.MiddleLeft;

            btnGravar.Image = Properties.Resources.download;
            btnGravar.ImageAlign = ContentAlignment.MiddleLeft;

            btnCancelar.Image = Properties.Resources.cross;
            btnCancelar.ImageAlign = ContentAlignment.MiddleLeft;

            btnListar.Image = Properties.Resources.lista_de_mesas;
            btnListar.ImageAlign = ContentAlignment.MiddleLeft;

            btnSair.Image = Properties.Resources.sair;
            btnSair.ImageAlign = ContentAlignment.MiddleLeft;

            btnPesquisar.Image = Properties.Resources.search;
            btnPesquisar.ImageAlign = ContentAlignment.MiddleLeft;

            btnUpload.Image = Properties.Resources.upload;
            btnUpload.ImageAlign = ContentAlignment.MiddleLeft;

            try
            {
                if (Environment.MachineName == "DESKTOP-PLHVR5D") // PC de casa
                {
                    // Conexão para casa, sem schema na consulta
                    string conexao = FrmAnimais.Properties.Settings.Default.ConexaoCasa;

                    using (OleDbConnection conn = new OleDbConnection(conexao))
                    {
                        conn.Open();
                        // Consulta simples sem schema
                        OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT id, nome, tutor, tipo, datanasc, foto FROM animal", conn);

                        DataTable tabela = new DataTable();
                        adapter.Fill(tabela);

                        dgvAnimal.DataSource = tabela;
                    }
                }
                else
                {
                    // Na escola, usa TableAdapter original com schema
                    string conexao = FrmAnimais.Properties.Settings.Default.ConexaoEscola;

                    animalTableAdapter.Connection.ConnectionString = conexao;
                    this.animalTableAdapter.Fill(this.animais.animal);

                    dgvAnimal.DataSource = this.animais.animal;
                }

                HabilitaCampos(grpCampos, false);
                HabilitaBotoes(true);
                LimpaCampos(grpCampos);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message);
            }
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            HabilitaCampos(grpCampos, true);
            HabilitaBotoes(false);
            incluir = true;
            LimpaCampos(grpCampos);
            txtNome.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvAnimal.SelectedRows.Count > 0)
            {
                incluir = false;
                HabilitaCampos(grpCampos, true);
                HabilitaBotoes(false);

                var row = dgvAnimal.CurrentRow;

                try
                {
                    // Casa (DataTable simples) → usar por índice
                    if (Environment.MachineName == "DESKTOP-PLHVR5D")
                    {
                        txtId.Text = row.Cells[0].Value.ToString();
                        txtNome.Text = row.Cells[1].Value.ToString();
                        txtTutor.Text = row.Cells[2].Value.ToString();
                        txtTipo.Text = row.Cells[3].Value.ToString();
                        dtpDataNasc.Value = Convert.ToDateTime(row.Cells[4].Value);
                        txtCaminhoImagem.Text = row.Cells[5].Value?.ToString();
                    }
                    else // Escola → pode usar nomes de coluna
                    {
                        txtId.Text = row.Cells["id"].Value.ToString();
                        txtNome.Text = row.Cells["nome"].Value.ToString();
                        txtTutor.Text = row.Cells["tutor"].Value.ToString();
                        txtTipo.Text = row.Cells["tipo"].Value.ToString();
                        dtpDataNasc.Value = Convert.ToDateTime(row.Cells["datanasc"].Value);
                        txtCaminhoImagem.Text = row.Cells["foto"].Value?.ToString();
                    }

                    // Carrega imagem se existir
                    if (!string.IsNullOrEmpty(txtCaminhoImagem.Text) && System.IO.File.Exists(txtCaminhoImagem.Text))
                        fotoTextBox.Image = Image.FromFile(txtCaminhoImagem.Text);
                    else
                        fotoTextBox.Image = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar dados para edição: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Selecione um animal primeiro!");
            }
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                string conexao = (Environment.MachineName == "DESKTOP-PLHVR5D") ?
                    FrmAnimais.Properties.Settings.Default.ConexaoCasa :
                    FrmAnimais.Properties.Settings.Default.ConexaoEscola;

                using (OleDbConnection conn = new OleDbConnection(conexao))
                {
                    conn.Open();
                    OleDbCommand cmd;

                    if (incluir)
                    {
                        string sqlInsert = "INSERT INTO animal (nome, tutor, tipo, datanasc, foto) VALUES (?, ?, ?, ?, ?)";
                        cmd = new OleDbCommand(sqlInsert, conn);
                        cmd.Parameters.AddWithValue("?", txtNome.Text);
                        cmd.Parameters.AddWithValue("?", txtTutor.Text);
                        cmd.Parameters.AddWithValue("?", txtTipo.Text);
                        cmd.Parameters.AddWithValue("?", dtpDataNasc.Value);
                        cmd.Parameters.AddWithValue("?", txtCaminhoImagem.Text);
                    }
                    else
                    {
                        string sqlUpdate = "UPDATE animal SET nome=?, tutor=?, tipo=?, datanasc=?, foto=? WHERE id=?";
                        cmd = new OleDbCommand(sqlUpdate, conn);
                        cmd.Parameters.AddWithValue("?", txtNome.Text);
                        cmd.Parameters.AddWithValue("?", txtTutor.Text);
                        cmd.Parameters.AddWithValue("?", txtTipo.Text);
                        cmd.Parameters.AddWithValue("?", dtpDataNasc.Value);
                        cmd.Parameters.AddWithValue("?", txtCaminhoImagem.Text);
                        cmd.Parameters.AddWithValue("?", int.Parse(txtId.Text));
                    }

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show(incluir ? "Registro incluído com sucesso!" : "Registro alterado com sucesso!");
                FrmAnimal_Load(null, null);
                btnCancelar_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            if (dgvAnimal.SelectedRows.Count > 0)
            {
                int id = int.Parse(dgvAnimal.CurrentRow.Cells[0].Value.ToString());

                if (MessageBox.Show("Deseja excluir este registro?", "Atenção", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        string conexao = (Environment.MachineName == "DESKTOP-PLHVR5D") ?
                            FrmAnimais.Properties.Settings.Default.ConexaoCasa :
                            FrmAnimais.Properties.Settings.Default.ConexaoEscola;

                        using (OleDbConnection conn = new OleDbConnection(conexao))
                        {
                            conn.Open();
                            string sqlDelete = "DELETE FROM animal WHERE id = ?";
                            OleDbCommand cmd = new OleDbCommand(sqlDelete, conn);
                            cmd.Parameters.AddWithValue("?", id);
                            cmd.ExecuteNonQuery();
                        }

                        FrmAnimal_Load(null, null);
                        MessageBox.Show("Registro excluído com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao apagar: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um registro primeiro.");
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            HabilitaCampos(grpCampos, false);
            HabilitaBotoes(true);
            LimpaCampos(grpCampos);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Imagens válidas|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string caminho = ofd.FileName;

                // Verifica se o arquivo existe e é uma imagem suportada
                if (System.IO.File.Exists(caminho) &&
                    (caminho.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                     caminho.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                     caminho.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                     caminho.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase)))
                {
                    try
                    {
                        // Cria cópia da imagem para evitar travamento
                        using (var stream = new System.IO.FileStream(caminho, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            Image img = Image.FromStream(stream);
                            fotoTextBox.Image = new Bitmap(img); // Cria cópia segura
                        }

                        fotoTextBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        txtCaminhoImagem.Text = caminho;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao carregar a imagem:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        fotoTextBox.Image = null;
                        txtCaminhoImagem.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Arquivo inválido. Escolha uma imagem JPG, PNG ou BMP.");
                }
            }
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            FrmAnimal_Load(null, null);
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmAnimal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Deseja mesmo sair?", "Atenção", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void HabilitaCampos(Control container, bool hab)
        {
            foreach (Control c in container.Controls)
                c.Enabled = hab;
        }

        private void HabilitaBotoes(bool hab)
        {
            btnIncluir.Enabled = hab;
            btnEditar.Enabled = hab;
            btnApagar.Enabled = hab;
            btnPesquisar.Enabled = hab;
            btnSair.Enabled = hab;
            btnGravar.Enabled = !hab;
            btnCancelar.Enabled = !hab;
        }

        private void LimpaCampos(Control container)
        {
            foreach (Control c in container.Controls)
            {
                if (c is TextBox) ((TextBox)c).Clear();
                else if (c is DateTimePicker) ((DateTimePicker)c).Value = DateTime.Now;
                else if (c is PictureBox) ((PictureBox)c).Image = null;
            }
        }

        private void dgvAnimal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string colName = dgvAnimal.Columns[e.ColumnIndex].Name;

            if (colName == "btnEditar")
            {
                btnEditar_Click(sender, e);
            }
            else if (colName == "btnApagar")
            {
                btnApagar_Click(sender, e);
            }
        }
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            string nomeProcurado = txtPesquisar.Text.Trim();

            if (string.IsNullOrEmpty(nomeProcurado))
            {
                MessageBox.Show("Digite um nome para pesquisar.");
                return;
            }

            try
            {
                string conexao = (Environment.MachineName == "DESKTOP-PLHVR5D") ?
                    FrmAnimais.Properties.Settings.Default.ConexaoCasa :
                    FrmAnimais.Properties.Settings.Default.ConexaoEscola;

                if (Environment.MachineName == "DESKTOP-PLHVR5D")
                {
                    using (OleDbConnection conn = new OleDbConnection(conexao))
                    {
                        conn.Open();
                        string sql = "SELECT id, nome, tutor, tipo, datanasc, foto FROM animal WHERE nome LIKE ?";
                        OleDbCommand cmd = new OleDbCommand(sql, conn);
                        cmd.Parameters.AddWithValue("?", "%" + nomeProcurado + "%");

                        OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                        DataTable tabela = new DataTable();
                        adapter.Fill(tabela);

                        dgvAnimal.DataSource = tabela;
                    }
                }
                else
                {
                    animalTableAdapter.Connection.ConnectionString = conexao;
                    this.animalTableAdapter.FillByNome(this.animais.animal, "%" + nomeProcurado + "%");
                    dgvAnimal.DataSource = this.animais.animal;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na pesquisa: " + ex.Message);
            }
        }
    }
}
