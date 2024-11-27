using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PII; // Namespace onde está a sua classe ConexaoNeo4j

namespace PII
{
    public partial class Ouvidoria : Form
    {
        private readonly ConexaoNeo4j _conexaoNeo4j;

        public Ouvidoria()
        {
            InitializeComponent();
            // Inicialize a conexão com o Neo4j, passando os parâmetros de conexão
            _conexaoNeo4j = new ConexaoNeo4j("neo4j+s://b91f0365.databases.neo4j.io:7687", "neo4j", "lVzihGZt8f77f_Puc3o7K0gBFAfrMZegd5jGq5pBNsM");
        }
        private void Ouvidoria_FormClosed(object sender, FormClosedEventArgs e)
        {
            _conexaoNeo4j.Dispose(); // Fechar a conexão com o banco Neo4j
        }

        private async void button7_Click_1(object sender, EventArgs e)
        {
            // Obtendo os valores dos campos no formulário
            string tipoFeedback = radioButton1.Checked ? "Positivo" :
                                  radioButton2.Checked ? "Negativo" :
                                  radioButton3.Checked ? "Sugestão" : "";

            string dataSelecionada = comboBox1.Text;
            string matricula = textBox2.Text;
            string descricao = textBox1.Text;

            // Validação simples dos campos
            if (string.IsNullOrWhiteSpace(tipoFeedback) ||
                string.IsNullOrWhiteSpace(dataSelecionada) ||
                string.IsNullOrWhiteSpace(matricula) ||
                string.IsNullOrWhiteSpace(descricao))
            {
                MessageBox.Show("Todos os campos devem ser preenchidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Envia os dados ao banco Neo4j
                await _conexaoNeo4j.RegistrarFeedbackAsync(tipoFeedback, dataSelecionada, matricula, descricao);
                MessageBox.Show("Feedback enviado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpar os campos após o envio
                comboBox1.SelectedIndex = -1;
                textBox2.Clear();
                textBox1.Clear();
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao enviar feedback: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
