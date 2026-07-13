using CooperativaAlfa.Api.Models;

namespace CooperativaAlfa.Api.Services
{
    public interface ICobolService
    {
        Cliente? ProcessarTransacao(Cliente cliente, char acao);
    }
}