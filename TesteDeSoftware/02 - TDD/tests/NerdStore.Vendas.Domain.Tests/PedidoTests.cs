using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {


        [Fact(DisplayName = "Adicionar Novo Pedido ")]
        [Trait("Categoria", "Vendas - Pedido")]

        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);

            //Act
            pedido.AdicionarItem(pedidoItem);

            //Assert
            Assert.Equal(200, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item Peido Existente")]
        [Trait("Categoria", "Vendas - Pedido")]

        public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadesSomarValores()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 1, 100);

            //Act
            pedido.AdicionarItem(pedidoItem2);

            //Assert
            Assert.Equal(300, pedido.ValorTotal);
            Assert.Equal(1, pedido.PedidoItems.Count);
            Assert.Equal(3, pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Adicionar Item Peido Acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Adicionar_Item_PeidoAcimaDoPermitido_DeveRetornarExpection()
        {
            //Arrange 
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", Pedido.Max_Unidades_Item + 1, 100);

            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem));
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente Acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistenteSomaUnidadeAcimaDoPermitido_DeveRetornarExpection()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 1, 100);
            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", Pedido.Max_Unidades_Item, 100);

            //Act
            pedido.AdicionarItem(pedidoItem2);

            //Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));

        }

        [Fact(DisplayName = "Atualizar Item Pedido Existente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItemAtualizado = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 100);

            //Act
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Valido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidade()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 100);
            var novaQuantidade = pedidoItemAtualizado.Quantidade;
            //Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            //Assert
            Assert.Equal(novaQuantidade,pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();

            var pedidoExistente1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            var pedidoExistente2 = new PedidoItem(produtoId, "Produto Teste", 3, 15);

            pedido.AdicionarItem(pedidoExistente1);
            pedido.AdicionarItem(pedidoExistente2);

            var pedidoAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 15);

            var totalPedido = pedidoExistente1.Quantidade * pedidoExistente1.ValorUnitario +
                              pedidoAtualizado.Quantidade * pedidoAtualizado.ValorUnitario;

            //Act
            pedido.AtualizarItem(pedidoAtualizado);

            //Assert
            Assert.Equal(totalPedido, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Atualizar Item Pedido Quantidade Acima do Permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();

            var pedidoItemExistente = new PedidoItem(produtoId, "Produto Teste", 3, 15);

            pedido.AdicionarItem(pedidoItemExistente);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", Pedido.Max_Unidades_Item +1, 15);

            //Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));

        }

        [Fact(DisplayName = "Remover Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemNaoExisteNaLista_DeveRetornarUmaaException()
        {
            //Arrange 
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItemRemover = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 100);

            //Act e Assert
            Assert.Throws<DomainException>(() => pedido.RemoverItem(pedidoItemRemover));
        }

        [Fact(DisplayName = "Remover Item Pedido Deve Calcular Valor Total ")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemExistente_DeveAtualizarValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            var pedidoItem2 = new PedidoItem(productId, "Produto Teste", 3, 15);

            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);


            var totalPedido = pedidoItem2.Quantidade * pedidoItem2.ValorUnitario;

            //Act
            pedido.RemoverItem(pedidoItem1);
            //Assert
            Assert.Equal(totalPedido, pedido.ValorTotal);


        }

        [Fact(DisplayName = "Aplicar Voucher Vàlido")]
        [Trait("Categoria", "Pedido -Voucher ")]
        public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1,
                TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

            var valorComDesconto = pedido.ValorTotal - voucher.ValorDesconto;

            //Act
            var result = pedido.AplicarVoucher(voucher);

            //Assert
            //Assert.Equal(valorComDesconto, pedido.ValorTotal);
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar Voucher Invàlido")]
        [Trait("Categoria", "Pedido -Voucher ")]
        public void Pedido_AplicarVoucherValido_DeveRetornarComErros()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1,
                TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), true, true);

            var valorComDesconto = pedido.ValorTotal - voucher.ValorDesconto;

            //Act
            var result = pedido.AplicarVoucher(voucher);

            //Assert
            //Assert.Equal(valorComDesconto, pedido.ValorTotal);
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher tipo percentual desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void  AplicarVoucher_VoucherTipoValorDesconto_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 3, 15);
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1,
                TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

            var valorComDesconto = pedido.ValorTotal - voucher.ValorDesconto;

            // Act
            pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(valorComDesconto, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher tipo percentual desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoPercentualDesconto_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 3, 15);
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var voucher = new Voucher("PROMO-15-OFF", 15, null, 1,
                TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(10), true, false);

            var valorDesconto = (pedido.ValorTotal * voucher.PercentualDesconto) / 100;
            var valorTotalComDesconto = pedido.ValorTotal - valorDesconto;

            // Act
            pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(valorTotalComDesconto, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher excede o valor total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_DescontoExcedeValorTotalPedido_PedidoDeveTerValorZerado()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            pedido.AdicionarItem(pedidoItem);

            var voucher = new Voucher("PROMO-15-REAIS", null, 300, 1,
               TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

            //Act
            pedido.AplicarVoucher(voucher);

            //Assert
            Assert.Equal(0, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher excede o valor total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_ModificarItensOeido_DeveCalcularDescontoValorTotal()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            pedido.AdicionarItem(pedidoItem);

            var voucher = new Voucher("PROMO-15-OFF", null, 50, 1,
               TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 4, 25);
            pedido.AdicionarItem(pedidoItem2);

            //Act
           pedido.AplicarVoucher(voucher);

            //Assert
            var totalesperado = pedido.PedidoItems.Sum(i => i.Quantidade * i.ValorUnitario) - voucher.ValorDesconto;
            Assert.Equal(totalesperado, pedido.ValorTotal);
        }
    }
}
