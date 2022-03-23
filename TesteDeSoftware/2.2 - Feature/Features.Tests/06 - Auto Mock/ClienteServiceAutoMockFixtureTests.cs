using Bogus;
using Bogus.DataSets;
using Features.Clientes;
using MediatR;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace Features.Tests._06___Auto_Mock
{

    [Collection(nameof(ClienteAutoMockerCollection))]
    public class ClienteServiceAutoMockFixtureTests
    {
        readonly ClienteTestsAutoMockerFixture ClienteTestAutoMockerFixture;
        private readonly ClienteService _clienteService;

        public ClienteServiceAutoMockFixtureTests(ClienteTestsAutoMockerFixture clienteTestsFixture)
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
            Assert.True(cliente.EhValido());
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
            Assert.False(cliente.EhValido());
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
            Assert.True(clientes.Any());
            Assert.False(clientes.Count(c => !c.Ativo) > 0);

        }
    }
}
