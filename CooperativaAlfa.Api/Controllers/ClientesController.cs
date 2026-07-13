using Microsoft.AspNetCore.Mvc;
using CooperativaAlfa.Api.Models;
using CooperativaAlfa.Api.Services;

namespace CooperativaAlfa.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        // Alterado para ICobolService
        private readonly ICobolService _cobolService;

        // Construtor alterado para receber ICobolService
        public ClientesController(ICobolService cobolService)
        {
            _cobolService = cobolService;
        }

        [HttpGet("{cpf}")]
        public IActionResult Consultar(string cpf)
        {
            var clienteRequest = new Cliente { Cpf = cpf };
            var resultado = _cobolService.ProcessarTransacao(clienteRequest, 'C');

            if (resultado == null) return NotFound(new { mensagem = "Cliente não encontrado." });
            return Ok(resultado);
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] Cliente novoCliente)
        {
            try
            {
                var resultado = _cobolService.ProcessarTransacao(novoCliente, 'I');
                return CreatedAtAction(nameof(Consultar), new { cpf = novoCliente.Cpf }, resultado);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{cpf}")]
        public IActionResult Atualizar(string cpf, [FromBody] Cliente dadosAtualizacao)
        {
            dadosAtualizacao.Cpf = cpf;
            var resultado = _cobolService.ProcessarTransacao(dadosAtualizacao, 'A');

            if (resultado == null) return NotFound(new { mensagem = "Cliente não encontrado." });
            return Ok(resultado);
        }

        [HttpDelete("{cpf}")]
        public IActionResult Excluir(string cpf)
        {
            var clienteRequest = new Cliente { Cpf = cpf };
            var resultado = _cobolService.ProcessarTransacao(clienteRequest, 'E');

            if (resultado == null) return NotFound(new { mensagem = "Cliente não encontrado." });
            return NoContent();
        }
    }
}
