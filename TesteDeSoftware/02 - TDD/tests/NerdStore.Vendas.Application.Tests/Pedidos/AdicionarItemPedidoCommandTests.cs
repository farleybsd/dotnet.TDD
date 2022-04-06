using NerdStore.Vendas.Application.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class AdicionarItemPedidoCommandTests
    {
        [Fact(DisplayName = "Adicionar Item Command Vàlido ")]
        [Trait("Categoria", "Vendas - Pedido Commads")]
        public void AdicionarItemPedidoCommand_CommandoEstadoValido_DevePassarNaValidacao()
        {
            //Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 2, 100);

            //Act
            var result = pedidoCommand.EhValido();

            //Assert
            Assert.True(result);
        }
    }
}
