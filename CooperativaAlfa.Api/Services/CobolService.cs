using System.Diagnostics;
using CooperativaAlfa.Api.Models;
using Microsoft.Extensions.Logging; // Importação necessária

namespace CooperativaAlfa.Api.Services
{
    public class CobolService : ICobolService
    {
        private readonly string _caminhoCobol = "/app/cobol/CLIENTES";
        private readonly ILogger<CobolService> _logger; // Declaração do Logger

        // Injeta o Logger no construtor
        public CobolService(ILogger<CobolService> logger)
        {
            _logger = logger;
        }

        public Cliente? ProcessarTransacao(Cliente cliente, char acao)
        {
            string inputCobol = cliente.ToCobolString(acao);
            string outputCobol = string.Empty;

            // Loga a intenção de execução
            _logger.LogInformation("Iniciando comunicação COBOL. Ação: {Acao}. Dados enviados: [{Input}]", acao, inputCobol);

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = _caminhoCobol,
                    Arguments = $"\"{inputCobol}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        outputCobol = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                    }
                }

                // Loga o retorno bruto do legado
                _logger.LogInformation("Retorno do COBOL recebido: [{Output}]", outputCobol);

                if (!string.IsNullOrWhiteSpace(outputCobol) && outputCobol.Length >= 99)
                {
                    string status = outputCobol.Substring(97, 2); // Status agora começa na posição 97

                    if (status == "04")
                    {
                        _logger.LogWarning("COBOL retornou status 04 (Não Encontrado) para o CPF: {Cpf}", cliente.Cpf);
                        return null;
                    }
                    if (status == "05")
                    {
                        _logger.LogWarning("COBOL retornou status 05 (Já Existente) para o CPF: {Cpf}", cliente.Cpf);
                        throw new InvalidOperationException("Cliente já cadastrado com este CPF.");
                    }

                    return Cliente.ParseFromCobolString(outputCobol);
                }

                _logger.LogError("Retorno do COBOL foi inválido ou vazio. Tamanho recebido: {Tamanho}", outputCobol?.Length ?? 0);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha crítica ao tentar executar o componente legado COBOL.");
                throw new Exception("Erro de comunicação com o componente legado.", ex);
            }
        }
    }
}