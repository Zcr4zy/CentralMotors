using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentralMotors.Models
{
    [Table("Fabricante")]
    public class Fabricante
    {
        public int FabricanteId { get; set; }

        [Required(ErrorMessage = "Informe o nome do fabricante do veiculo!")]
        [StringLength(40, ErrorMessage = "O tamanho máximo para o nome do fabricante é de 40 caracteres!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
    }
}
