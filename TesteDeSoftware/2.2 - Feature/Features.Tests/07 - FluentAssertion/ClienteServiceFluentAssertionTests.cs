using Features.Clientes;
using Features.Tests._06___Auto_Mock;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace Features.Tests._07___FluentAssertion
{
    [Collection(nameof(ClienteAutoMockerCollection))]
    public class ClienteServiceFluentAssertionTests
    {
        readonly ClienteTestsAutoMockerFixture ClienteTestAutoMockerFixture;
        private readonly ClienteService _clienteService;

        public ClienteServiceFluentAssertionTests(ClienteTestsAutoMockerFixture clienteTestsFixture)
        {
            ClienteTestAutoMockerFixture = clienteTestsFixture;
            _clienteService = ClienteTestAutoMockerFixture.ObterClienteService();
        }


        [Fact(DisplayName = "Adicionar Cliente com Sucesso")]
        [Trait("Categoria", "Cliente Service AutoMockFixture Tests")]
        public void ClienteService_Adicionar_DeveExecutarComSucesso()
        {
            // Arrange
            var cliente = ClienteTestAutoMockerFixture.GerarClienteValido();

            // Act
            _clienteService.Adicionar(cliente);

            // Assert
            cliente.EhValido().Should().BeTrue();
            ClienteTestAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.Adicionar(cliente), Times.Once); // faz um asseert no metodo
            ClienteTestAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once); // Passou pelo metodo
        }

        [Fact(DisplayName = "Adicionar Cliente com Falha")]
        [Trait("Categoria", "Cliente Service AutoMockFixture Tests")]
        public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
        {
            // Arrange
            var cliente = ClienteTestAutoMockerFixture.GerarClienteInvalido();

            // Act
            _clienteService.Adicionar(cliente);

            // Assert
            cliente.EhValido().Should().BeFalse("Possui inconsistências");
            ClienteTestAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.Adicionar(cliente), Times.Never); // faz um asseert no metodo
            ClienteTestAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never); // Passou pelo metodo

        }

        [Fact(DisplayName = "Obter Clientes Ativos")]
        [Trait("Categoria", "Cliente Service AutoMockFixture Tests")]
        public void ClienteService_ObterTodosAtivos_DeveRetornarApenasClientesAtivos()
        {
            // Arrange
            ClienteTestAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Setup(c => c.ObterTodos()).Returns(ClienteTestAutoMockerFixture.ObterClientesVariados);
            // Act
            var clientes = _clienteService.ObterTodosAtivos();
            // Assert 
            ClienteTestAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.ObterTodos(), Times.Once);
            clientes.Should().HaveCountGreaterOrEqualTo(1).And.OnlyHaveUniqueItems();
            clientes.Should().NotContain(c => !c.Ativo);

        }
    }
}

