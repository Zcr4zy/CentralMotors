using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CentralMotors.Models
{
    public class Cor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CorId { get; set; }

        [Required(ErrorMessage = "Informe o nome da cor!")]
        [StringLength(30, ErrorMessage = "O tamanho máximo para o nome da cor é de 30 caracteres!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
    }
}
