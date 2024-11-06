using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PII
{
    internal class Conexao
    {

            SqlConnection conn = new SqlConnection();


            public void Conectar()
            {
                string aux = "SERVER=.\\SQLEXPRESS;Database=Teste;UID=sa;PWD=123";
                conn.ConnectionString = aux;
                conn.Open();
            }

            public void Desconectar()
            {
                conn.Close();
            }

            public void Executar(string sql)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }


            public DataSet ListarDados(string sql)
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }

        public void InserirRegistro(string nome, DateTime dataNascimento, int idCurso, string endereco, string email, string matricula)
        {
            // SQL para inserir os dados na tabela Alunos
            string sql = "INSERT INTO Aluno (nomeAluno, dataNascimento, idCurso, endereco, email, matricula) " +
                         "VALUES (@nome, @dataNascimento, @idCurso, @endereco, @email, @matricula)";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // Adiciona os parâmetros ao comando
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@dataNascimento", dataNascimento);
                cmd.Parameters.AddWithValue("@idCurso", idCurso);
                cmd.Parameters.AddWithValue("@endereco", endereco);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@matricula", matricula);

                Conectar();
                try
                {
                    // Executa o comando
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao inserir registro: " + ex.Message);
                }
                finally
                {
                    Desconectar();
                }
            }
        }

        public List<KeyValuePair<int, string>> ObterCursos()
        {
            string sql = "SELECT idCurso, nomeCurso FROM Curso";
            List<KeyValuePair<int, string>> cursos = new List<KeyValuePair<int, string>>();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                Conectar();
                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idCurso = reader.GetInt32(0);
                            string nomeCurso = reader.GetString(1);
                            cursos.Add(new KeyValuePair<int, string>(idCurso, nomeCurso));
                        }
                    }

                    if (cursos.Count == 0)
                    {
                        throw new Exception("Nenhum curso encontrado no banco de dados.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao obter cursos: " + ex.Message);
                }
                finally
                {
                    Desconectar();
                }
            }
            return cursos;
        }

        public DataTable ObterAlunos()
        {
            string sql = "SELECT nomeAluno, dataNascimento, idCurso, endereco, email, matricula FROM Aluno";
            DataTable alunos = new DataTable();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                Conectar();
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(alunos);  // Preenche o DataTable com os dados
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao obter alunos: " + ex.Message);
                }
                finally
                {
                    Desconectar();
                }
            }
            return alunos;
        }


    }


}


