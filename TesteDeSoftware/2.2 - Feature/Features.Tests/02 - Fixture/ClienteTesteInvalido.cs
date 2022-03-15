using Features.Clientes;
using Features.Tests._02___Fixture;
using System;
using Xunit;

namespace Features.Tests._02___Fixture
{
    [Collection(nameof(ClienteCollection))]
    public class ClienteTesteInvalido
    {

        private readonly ClienteTestFixture _clienteTestFixture;

        public ClienteTesteInvalido(ClienteTestFixture clienteTestFixture)
        {
            _clienteTestFixture = clienteTestFixture;
        }

        [Fact(DisplayName = "Cliente InValido")]
        [Trait("Categoria", "Cliente Trait Teste")]
        public void Cliente_NovoCliente_DeveEstarInValido()
        {
            // Arrange 
            var cliente = _clienteTestFixture.gerarClienteInValido();


            //Act
            var result = cliente.EhValido();

            //Assert
            Assert.False(result);
            Assert.NotEqual(0, cliente.ValidationResult.Errors.Count);
        }
    }

}
