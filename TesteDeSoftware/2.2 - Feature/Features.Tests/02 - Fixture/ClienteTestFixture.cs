using Features.Clientes;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Features.Tests._02___Fixture
{
    [CollectionDefinition(nameof(ClienteCollection))]
    public class ClienteCollection : ICollectionFixture<ClienteTestFixture>
    {

    }


    public class ClienteTestFixture : IDisposable
    {
        public Cliente gerarClienteValido()
        {
            var cliente = new Cliente(
               Guid.NewGuid(),
               "Eduardo",
               "Pires",
               DateTime.Now.AddYears(-30),
               "edu@edu.com",
               true,
               DateTime.Now
               );

            return cliente;
        }


        public Cliente gerarClienteInValido()
        {
            var cliente = new Cliente(
            Guid.NewGuid(),
            "",
            "",
            DateTime.Now,
            "edu@edu.com",
            true,
            DateTime.Now
            );

            return cliente;
        }

        public void Dispose()
        {
            
        }
    }
}
