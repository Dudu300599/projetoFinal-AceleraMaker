using System.ComponentModel.DataAnnotations;

namespace CooperativaAlfa.Api.Models
{
    public class Cliente
    {
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 dígitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas números.")]
        public string Cpf { get; set; } = string.Empty;

        [StringLength(30, ErrorMessage = "O nome não pode exceder 30 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "O telefone deve conter apenas números.")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        [StringLength(40, ErrorMessage = "O e-mail não pode exceder 40 caracteres.")]
        public string Email { get; set; } = string.Empty;

        public string ToCobolString(char acao)
        {
            string acaoStr = acao.ToString();
            string cpfStr = (Cpf ?? "").PadRight(11, ' ').Substring(0, 11);
            string nomeStr = (Nome ?? "").PadRight(30, ' ').Substring(0, 30);
            string telefoneStr = (Telefone ?? "").PadRight(15, ' ').Substring(0, 15);
            string emailStr = (Email ?? "").PadRight(40, ' ').Substring(0, 40);
            string statusStr = "00";

            // Novo tamanho total: 99 caracteres
            return $"{acaoStr}{cpfStr}{nomeStr}{telefoneStr}{emailStr}{statusStr}";
        }

        public static Cliente? ParseFromCobolString(string cobolData)
        {
            if (string.IsNullOrWhiteSpace(cobolData) || cobolData.Length < 99)
                return null;

            return new Cliente
            {
                Cpf = cobolData.Substring(1, 11).Trim(),
                Nome = cobolData.Substring(12, 30).Trim(),
                Telefone = cobolData.Substring(42, 15).Trim(),
                Email = cobolData.Substring(57, 40).Trim()
            };
        }
    }
}