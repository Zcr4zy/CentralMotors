using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentralMotors.Models
{
    [Table("Tipo")]
    public class Tipo
    {
        [Key]
        public int TipoId { get; set; }

        [Required(ErrorMessage = "Informe o nome do tipo de veículo!")]
        [StringLength(30, ErrorMessage = "O tamanho máximo para o nome do tipo de veículo é de 30 caracteres!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
    }
}
