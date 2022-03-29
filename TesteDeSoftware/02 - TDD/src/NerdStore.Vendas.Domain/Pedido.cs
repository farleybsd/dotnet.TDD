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
        protected Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }
        public decimal ValorTotal { get; private set; }

        private readonly List<PedidoItem> _pedidoItems;

        public PedidoStatus PedidoStatus { get; private set; }

        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        public Guid ClienteId { get; private set; }

        public void CalcularValorPedido()
        {
            ValorTotal = PedidoItems.Sum(i => i.CalcularValor());
        }

        public void AdicionarItem(PedidoItem pedidoItem)
        {
            if(_pedidoItems.Any(p => p.ProdutoId == pedidoItem.ProdutoId))
            {
                var itemexistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);
                itemexistente.AdicionarUnidades(pedidoItem.Quantidade);
                pedidoItem = itemexistente;

                _pedidoItems.Remove(itemexistente);
            }

            _pedidoItems.Add(pedidoItem);
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