using CooperativaAlfa.Api.Models;
using Xunit;

namespace CooperativaAlfa.Testes
{
    public class ClienteTests
    {
        [Fact]
        public void ToCobolString_DeveFormatarCorretamente_Com99Posicoes()
        {
            // Arrange (Preparação dos dados com a nova estrutura de CPF)
            var cliente = new Cliente
            {
                Cpf = "12345678901",
                Nome = "CLIENTE DE TESTE",
                Telefone = "21988887777",
                Email = "teste@alfa.com"
            };

            // Act (Ação: Simulando uma Inserção 'I')
            string retornoString = cliente.ToCobolString('I');

            // Assert (Validações do resultado esperado)
            Assert.Equal(99, retornoString.Length); // Garante que o novo layout tem exatas 99 posições
            Assert.StartsWith("I12345678901CLIENTE DE TESTE", retornoString); // Garante a ação, o CPF e o início do nome
            Assert.EndsWith("00", retornoString); // Garante que o status inicial enviado é 00
        }

        [Fact]
        public void ParseFromCobolString_DeveConverterStringParaObjetoCorretamente()
        {
            // Arrange: Uma string de 99 posições simulando exatamente o retorno de sucesso do COBOL
            // Ação(1) + CPF(11) + Nome(30) + Telefone(15) + Email(40) + Status(2)
            string retornoCobol = "C17205635780MARIA DA SILVA                11977776666    maria.silva@email.com                   00";

            // Act
            var cliente = Cliente.ParseFromCobolString(retornoCobol);

            // Assert
            Assert.NotNull(cliente);
            Assert.Equal("17205635780", cliente.Cpf);
            Assert.Equal("MARIA DA SILVA", cliente.Nome); // Garante que os espaços em branco extras foram removidos com o Trim()
            Assert.Equal("11977776666", cliente.Telefone);
            Assert.Equal("maria.silva@email.com", cliente.Email);
        }

        [Fact]
        public void ParseFromCobolString_DeveRetornarNuloSeStringForInvalida()
        {
            // Arrange: Uma string menor do que o layout esperado de 99 posições
            string retornoInvalido = "C12345678901NOME CURTO";

            // Act
            var cliente = Cliente.ParseFromCobolString(retornoInvalido);

            // Assert
            Assert.Null(cliente);
        }
    }
}