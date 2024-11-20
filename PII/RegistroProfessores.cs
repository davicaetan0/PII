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
            try
            {
                string uri = "bolt://ba3ed93c.databases.neo4j.io:7687";
                string user = "neo4j";
                string password = "9pZkRYX23ksULG-D7jRCPeeVtyvft2fGIsfmKVYAbjQ";

                using (var conexao = new ConexaoNeo4j(uri, user, password))
                {
                    await conexao.PrintGreetingAsync("Olá, Neo4j!");
                    MessageBox.Show("Mensagem enviada e nó criado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


    }
}
