using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoItemTests
    {
        [Fact(DisplayName = "Novo Item Pedido com unidades Abaixo Do Permitido")]
        [Trait("Categoria", "Vendas - Pedido Item")]
        public void Adicionar_Item_PeidoAbaixoDoPermitido_DeveRetornarExpection()
        {
            //Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.Min_Unidades_Item - 1, 100));
        }
    }
}
