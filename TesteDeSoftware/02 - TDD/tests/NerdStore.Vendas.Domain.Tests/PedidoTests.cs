using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName ="Adicionar Novo Pedido ")] 
        [Trait("Categoria","Pedido Tests")]

        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            //Arrange
            var pedido = new Pedido();
            var pedidoItem = new PedidoItem(Guid.NewGuid(),"Produto Teste",2,100);

            //Act

            pedido.AdicionarItem(pedidoItem);

            //Assert

            Assert.Equal(200, pedido.ValorTotal);
        }
    }
}
