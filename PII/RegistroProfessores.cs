using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PII
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            registro reg = new registro();
            reg.ShowDialog();

        }

        private async void button2_Click(object sender, EventArgs e)
        {

            // Captura os valores do formulário
            string nome = txtNome.Text;
            string endereco = txtEndereco.Text;
            string email = txtEmail.Text;
            string curso = comboBoxCurso.SelectedItem?.ToString() ?? "";
            string dataNascimento = txtData.Text; // Certifique-se de que este é o formato desejado
            string cpf = textBox2.Text;
            string registroGeral = textBox1.Text; // Substitua pelo nome correto do TextBox


            string uri = "neo4j+s://b91f0365.databases.neo4j.io:7687";
            string user = "neo4j";
            string password = "lVzihGZt8f77f_Puc3o7K0gBFAfrMZegd5jGq5pBNsM";

            // Cria uma conexão com o Neo4j
            using (var conexao = new ConexaoNeo4j(uri, user, password))
            {
                try
                {
                    // Registra o professor no banco de dados
                    await conexao.RegistrarProfessorAsync(nome, endereco, email, curso, dataNascimento, cpf, registroGeral);
                    MessageBox.Show("Professor registrado com sucesso!", "Registro", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Atualiza o DataGridView com os professores
                    await PreencherDataGridView(conexao);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao registrar professor: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            string matricula = txtFormacao.Text;
            string nome = txtNome.Text;
            string endereco = txtEndereco.Text;
            string email = txtEmail.Text;
            string curso = comboBoxCurso.SelectedItem?.ToString() ?? "";
            string dataNascimento = txtData.Text;
            string cpf = textBox2.Text;
            string registroGeral = textBox1.Text;

            if (string.IsNullOrWhiteSpace(matricula) ||
                string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Por favor, preencha os campos obrigatórios.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string uri = "neo4j+s://b91f0365.databases.neo4j.io:7687";
            string user = "neo4j";
            string password = "lVzihGZt8f77f_Puc3o7K0gBFAfrMZegd5jGq5pBNsM";

            using (var conexao = new ConexaoNeo4j(uri, user, password))
            {
                try
                {
                    await conexao.AtualizarProfessorAsync(matricula, nome, endereco, email, curso, dataNascimento, cpf, registroGeral);
                    MessageBox.Show("Dados do professor atualizados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao atualizar professor: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            string cpf = textBox2.Text; // Campo onde o CPF é inserido

            string uri = "neo4j+s://b91f0365.databases.neo4j.io:7687";
            string user = "neo4j";
            string password = "lVzihGZt8f77f_Puc3o7K0gBFAfrMZegd5jGq5pBNsM";

            using (var conexao = new ConexaoNeo4j(uri, user, password))
            {
                try
                {
                    var professor = await conexao.BuscarProfessorPorCpfAsync(cpf);

                    if (professor != null)
                    {
                        txtNome.Text = professor["Nome"];
                        txtEndereco.Text = professor["Endereco"];
                        txtEmail.Text = professor["Email"];
                        txtFormacao.Text = professor["Formacao"];
                        txtData.Text = professor["DataNascimento"];
                        textBox2.Text = professor["CPF"];
                        textBox1.Text = professor["RegistroGeral"];

                        MessageBox.Show("Professor encontrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Nenhum professor encontrado com este CPF.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao buscar professor: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task PreencherDataGridView(ConexaoNeo4j conexao)
        {
            try
            {
                // Obtém a lista de professores do banco
                var professores = await conexao.ListarProfessoresAsync();

                // Cria uma tabela temporária para armazenar os dados
                var tabela = new DataTable();
                tabela.Columns.Add("Nome");
                tabela.Columns.Add("Endereço");
                tabela.Columns.Add("Email");
                tabela.Columns.Add("Formação");
                tabela.Columns.Add("Data de Nascimento");
                tabela.Columns.Add("CPF");
                tabela.Columns.Add("Registro Geral");

                foreach (var professor in professores)
                {
                    tabela.Rows.Add(
                        professor["Nome"],
                        professor["Endereco"],
                        professor["Email"],
                        professor["Formacao"],
                        professor["DataNascimento"],
                        professor["CPF"],
                        professor["RegistroGeral"]
                    );
                }

                // Atribui a tabela ao DataGridView
                ListarProfessor.DataSource = tabela;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar os professores: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private async void buttonExcluir_Click(object sender, EventArgs e)
        {
            string cpf = textBox2.Text; // Supondo que o CPF esteja no TextBox txtCPF

            if (string.IsNullOrWhiteSpace(cpf))
            {
                MessageBox.Show("Por favor, insira o CPF do professor.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string uri = "neo4j+s://b91f0365.databases.neo4j.io:7687";
            string user = "neo4j";
            string password = "lVzihGZt8f77f_Puc3o7K0gBFAfrMZegd5jGq5pBNsM";

            // Cria uma conexão com o Neo4j
            using (var conexao = new ConexaoNeo4j(uri, user, password))
            {
                try
                {
                    // Exclui o professor no banco de dados com base no CPF
                    await conexao.ExcluirProfessorAsync(cpf);
                    MessageBox.Show("Professor excluído com sucesso!", "Exclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Atualiza o DataGridView após a exclusão
                    await PreencherDataGridView(conexao);

                    LimparCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir professor: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private void LimparCampos()
        {
            txtNome.Clear();
            txtFormacao.Clear();
            txtEndereco.Clear();
            txtEmail.Clear();
            comboBoxCurso.SelectedIndex = -1;  // Limpar a seleção do ComboBox
            txtData.Clear();
            textBox2.Clear();
            textBox1.Clear();
        }
    }
}
