using CooperativaAlfa.Api.Controllers;
using CooperativaAlfa.Api.Models;
using CooperativaAlfa.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CooperativaAlfa.Testes
{
    public class ClientesControllerTests
    {
        private readonly Mock<ICobolService> _mockCobolService;
        private readonly ClientesController _controller;

        public ClientesControllerTests()
        {
            // Prepara o Mock (Simulador) do serviço COBOL
            _mockCobolService = new Mock<ICobolService>();
            _controller = new ClientesController(_mockCobolService.Object);
        }

        [Fact]
        public void Consultar_DeveRetornarOk_QuandoClienteExiste()
        {
            // Arrange
            var cpfTeste = "12345678901";
            var clienteMock = new Cliente { Cpf = cpfTeste, Nome = "João" };

            // Simula que o COBOL encontrou o cliente
            _mockCobolService.Setup(s => s.ProcessarTransacao(It.IsAny<Cliente>(), 'C'))
                             .Returns(clienteMock);

            // Act
            var resultado = _controller.Consultar(cpfTeste);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var clienteRetornado = Assert.IsType<Cliente>(okResult.Value);
            Assert.Equal(cpfTeste, clienteRetornado.Cpf);
        }

        [Fact]
        public void Consultar_DeveRetornarNotFound_QuandoClienteNaoExiste()
        {
            // Arrange
            _mockCobolService.Setup(s => s.ProcessarTransacao(It.IsAny<Cliente>(), 'C'))
                             .Returns((Cliente?)null);

            // Act
            var resultado = _controller.Consultar("00000000000");

            // Assert
            Assert.IsType<NotFoundObjectResult>(resultado);
        }

        [Fact]
        public void Cadastrar_DeveRetornarCreated_QuandoCpfForNovo()
        {
            // Arrange
            var novoCliente = new Cliente { Cpf = "11122233344", Nome = "Maria" };
            _mockCobolService.Setup(s => s.ProcessarTransacao(novoCliente, 'I'))
                             .Returns(novoCliente);

            // Act
            var resultado = _controller.Cadastrar(novoCliente);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(resultado);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public void Atualizar_DeveRetornarOk_QuandoAtualizacaoComSucesso()
        {
            // Arrange
            var cpfAtualizacao = "12345678901";
            var dadosAtualizados = new Cliente { Cpf = cpfAtualizacao, Telefone = "11999999999" };

            _mockCobolService.Setup(s => s.ProcessarTransacao(It.IsAny<Cliente>(), 'A'))
                             .Returns(dadosAtualizados);

            // Act
            var resultado = _controller.Atualizar(cpfAtualizacao, dadosAtualizados);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Excluir_DeveRetornarNoContent_QuandoClienteForExcluido()
        {
            // Arrange
            var clienteSimulado = new Cliente { Cpf = "12345678901" };
            _mockCobolService.Setup(s => s.ProcessarTransacao(It.IsAny<Cliente>(), 'E'))
                             .Returns(clienteSimulado);

            // Act
            var resultado = _controller.Excluir("12345678901");

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(resultado);
            Assert.Equal(204, noContentResult.StatusCode);
        }
    }
}