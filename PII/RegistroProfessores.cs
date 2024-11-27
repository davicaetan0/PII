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
            string matricula = txtMatricula.Text;
            string dataNascimento = txtData.Text; // Certifique-se de que este é o formato desejado
            string cpf = textBox2.Text;
            string registroGeral = textBox1.Text;

            string uri = "neo4j+s://b91f0365.databases.neo4j.io:7687";
            string user = "neo4j";
            string password = "lVzihGZt8f77f_Puc3o7K0gBFAfrMZegd5jGq5pBNsM";

            // Cria uma conexão com o Neo4j
            using (var conexao = new ConexaoNeo4j(uri, user, password))
            {
                try
                {
                    // Chama o método para inserir os dados
                    await conexao.RegistrarProfessorAsync(nome, endereco, email, curso, matricula, dataNascimento, cpf, registroGeral);
                    MessageBox.Show("Professor registrado com sucesso!", "Registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao registrar professor: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}
