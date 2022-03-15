using Features.Clientes;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
namespace Features.Tests._02___Fixture
{
    [Collection(nameof(ClienteCollection))]
    public class ClienteTestValido
    {
        private readonly ClienteTestFixture _clienteTestFixture;

        public ClienteTestValido(ClienteTestFixture clienteTestFixture)
        {
            _clienteTestFixture = clienteTestFixture;
        }

        //Categorizar os teste
        [Fact(DisplayName = "Cliente Valido")]
        [Trait("Categoria", "Cliente Trait Teste")]
        public void Cliente_NovoCliente_DeveEstarValido()
        {
            // Arrange 
            var cliente = _clienteTestFixture.gerarClienteValido();


            //Act
            var result = cliente.EhValido();

            //Assert
            Assert.True(result);
            Assert.Equal(0, cliente.ValidationResult.Errors.Count);
        }


    }
}
