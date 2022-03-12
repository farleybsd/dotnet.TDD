using Features.Clientes;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Features.Tests._01___Traits
{
    public class ClienteTests
    {
        //Categorizar os teste
        [Fact(DisplayName = "Cliente Valido")]
        [Trait("Categoria","Cliente Trait Teste")]
        public void Cliente_NovoCliente_DeveEstarValido()
        {
            // Arrange 
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Eduardo",
                "Pires",
                DateTime.Now.AddYears(-30),
                "edu@edu.com",
                true,
                DateTime.Now
                );


            //Act
            var result = cliente.EhValido();

            //Assert
            Assert.True(result);
            Assert.Equal(0, cliente.ValidationResult.Errors.Count);
        }

        [Fact(DisplayName = "Cliente InValido")]
        [Trait("Categoria", "Cliente Trait Teste")]
        public void Cliente_NovoCliente_DeveEstarInValido()
        {
            // Arrange 
            var cliente = new Cliente(
                Guid.NewGuid(),
                "",
                "",
                DateTime.Now,
                "edu@edu.com",
                true,
                DateTime.Now
                );


            //Act
            var result = cliente.EhValido();

            //Assert
            Assert.False(result);
            Assert.NotEqual(0, cliente.ValidationResult.Errors.Count);
        }

    }
}
