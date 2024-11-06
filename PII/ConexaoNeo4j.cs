﻿using System;
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