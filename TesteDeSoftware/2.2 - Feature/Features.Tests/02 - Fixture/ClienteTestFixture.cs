using Bogus;
using Bogus.DataSets;
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
            var genero = new Faker().PickRandom<Name.Gender>();

            var cliente = new Faker<Cliente>("pt_BR")
                .CustomInstantiator( f => new Cliente(
                     Guid.NewGuid(),
                     f.Name.FirstName(genero),
                     f.Name.LastName(genero),
                     f.Date.Past(80,DateTime.Now.AddYears(-18)),
                     "",
                     true,
                     DateTime.Now))
                .RuleFor(c => c.Email,(f,c)=> f.Internet.Email(
                    c.Nome.ToLower(),
                    c.Sobrenome.ToLower()));


            //var email = new Faker().Internet.Email("Eduardo","Pires","gamil");
            //var clienteFaker = new Faker<Cliente>();
            //clienteFaker.RuleFor(c => c.Nome, (f, c) => f.Name.FirstName());

            //var cliente = new Cliente(
            //   Guid.NewGuid(),
            //   "Eduardo",
            //   "Pires",
            //   DateTime.Now.AddYears(-30),
            //   "edu@edu.com",
            //   true,
            //   DateTime.Now
            //   );

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
