using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentValidation.Results;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain
{
    public class Pedido 
    {
        public static int Max_Unidades_Item => 15;
        public static int Min_Unidades_Item => 1;


        protected Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }
        public decimal ValorTotal { get; private set; }
        public decimal Desconto { get; private set; }

        private readonly List<PedidoItem> _pedidoItems;

        public PedidoStatus PedidoStatus { get; private set; }

        public bool VoucherUtilizado { get; set; }
        public Voucher Voucher { get; private set; }
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        public Guid ClienteId { get; private set; }


        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var result = voucher.ValidarSeAplicavel();

            if (!result.IsValid) return result;

            Voucher = voucher;
            VoucherUtilizado = true;

            CalcularValorTotalDesconto();

            return result;
        }


        public void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal descponto = 0;
            var valor = ValorTotal;

            if(Voucher.TipoDescontoVoucher == TipoDescontoVoucher.Valor)
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    descponto = Voucher.ValorDesconto.Value;
                    valor -= descponto;
                }
            }
            else
            {
                if (Voucher.PercentualDesconto.HasValue)
                {
                    descponto = (ValorTotal * Voucher.PercentualDesconto.Value) / 100;
                    valor -= descponto;
                }
                        
            }

            ValorTotal = valor < 0 ? 0 : valor;
            if (ValorTotal < 0) ValorTotal = 0;
            Desconto = descponto;

        }
        private void CalcularValorPedido()
        {
            ValorTotal = PedidoItems.Sum(i => i.CalcularValor());
            CalcularValorTotalDesconto();
        }

        private bool PedidoExistente(PedidoItem pedidoItem)
        {
            return _pedidoItems.Any(p => p.ProdutoId == pedidoItem.ProdutoId);
        }

        private void ValidarPedidoItemInexistente(PedidoItem item)
        {
            if (!PedidoExistente(item)) throw new DomainException($"O item não existe no pedido");
        }

        private void ValidarQuantidadeItemPermitida(PedidoItem item)
        {
            var quantidadeItems = item.Quantidade;

            if (PedidoExistente(item))
            {
                var itemExistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
                quantidadeItems += itemExistente.Quantidade;
            }

            if(quantidadeItems > Max_Unidades_Item) throw new DomainException($"Màximo de {Max_Unidades_Item} unidades por produto");
        }

        public void AdicionarItem(PedidoItem pedidoItem)
        {
            ValidarQuantidadeItemPermitida(pedidoItem);

            if (PedidoExistente(pedidoItem))
            {
                var itemexistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);
                itemexistente.AdicionarUnidades(pedidoItem.Quantidade);
                pedidoItem = itemexistente;

                _pedidoItems.Remove(itemexistente);
            }

            _pedidoItems.Add(pedidoItem);
            CalcularValorPedido();
           
        }

        public void AtualizarItem(PedidoItem pedidoItem)
        {
            ValidarPedidoItemInexistente(pedidoItem);
            ValidarQuantidadeItemPermitida(pedidoItem);

            var itemExistente = PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);
            _pedidoItems.Remove(itemExistente);
            _pedidoItems.Add(pedidoItem);
            CalcularValorPedido();
        }

        public void RemoverItem(PedidoItem pedidoItem)
        {
            ValidarPedidoItemInexistente(pedidoItem);
            _pedidoItems.Remove(pedidoItem);
            CalcularValorPedido();
        }

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }

        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteId,
                };

                pedido.TornarRascunho();
                return pedido;
            }
        }
      
    }
}