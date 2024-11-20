using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace PII
{
    public class ConexaoNeo4j : IDisposable
    {
        private readonly IDriver _driver;

        public ConexaoNeo4j(string uri, string user, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        public async Task RegistrarProfessorAsync(string nome, string endereco, string email, string formacao, string matricula, string dataNascimento, string cpf, string registroGeral)
        {
            await using var session = _driver.AsyncSession();
            await session.ExecuteWriteAsync(
                async tx =>
                {
                    var query = @"
                        CREATE (p:Professor {
                            Nome: $nome,
                            Endereco: $endereco,
                            Email: $email,
                            Formacao: $formacao,
                            Matricula: $matricula,
                            DataNascimento: $dataNascimento,
                            CPF: $cpf,
                            RegistroGeral: $registroGeral
                        })
                        RETURN p";
                    await tx.RunAsync(query, new
                    {
                        nome,
                        endereco,
                        email,
                        formacao,
                        matricula,
                        dataNascimento,
                        cpf,
                        registroGeral
                    });
                });
        }

        public async Task PrintGreetingAsync(string message)
        {
            await using var session = _driver.AsyncSession();
            var greeting = await session.ExecuteWriteAsync(
                async tx =>
                {
                    var result = await tx.RunAsync(
                        "CREATE (a:Greeting) " +
                        "SET a.message = $message " +
                        "RETURN a.message + ', from node ' + id(a)",
                        new { message });

                    var record = await result.SingleAsync();
                    return record[0].As<string>();
                });

            Console.WriteLine(greeting);
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}