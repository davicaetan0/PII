using System;
using System.Data;

namespace PII
{
    internal class Curso
    {
        private readonly Conexao objetoConexao = new Conexao();

        public int CodigoCurso { get; set; }
        public string NomeCurso { get; set; }

        public void Incluir()
        {
            string sql = $"INSERT INTO Curso(nomeCurso) VALUES ('{NomeCurso}')";
            ExecutarComando(sql);
        }

        public void Alterar()
        {
            string sql = $"UPDATE Curso SET nomeCurso = '{NomeCurso}' WHERE idCurso = {CodigoCurso}";
            ExecutarComando(sql);
        }

        public void Excluir()
        {
            objetoConexao.Conectar();
            try
            {
                // Atualizar referências antes de excluir
                string sqlAtualizar = $"UPDATE AulaReforco SET idDisciplina = NULL WHERE idDisciplina = {CodigoCurso}";
                objetoConexao.Executar(sqlAtualizar);

                // Excluir o curso
                string sqlExcluir = $"DELETE FROM Disciplina WHERE idDisciplina = {CodigoCurso}";
                objetoConexao.Executar(sqlExcluir);
            }
            finally
            {
                objetoConexao.Desconectar();
            }
        }

        public DataSet PesquisaDados()
        {
            string sql = "SELECT * FROM Curso";
            return ObterDados(sql);
        }

        private void ExecutarComando(string sql)
        {
            objetoConexao.Conectar();
            try
            {
                objetoConexao.Executar(sql);
            }
            finally
            {
                objetoConexao.Desconectar();
            }
        }

        private DataSet ObterDados(string sql)
        {
            objetoConexao.Conectar();
            try
            {
                return objetoConexao.ListarDados(sql);
            }
            finally
            {
                objetoConexao.Desconectar();
            }
        }
    }
}
