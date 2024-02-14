using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentralMotors.Models
{
    [Table("TipoCombustivel")]
    public class TipoCombustivel
    {
        [Key]
        public int TipoCombustivelId { get; set; }

        [Required(ErrorMessage = "Informe o nome do tipo de combustível!")]
        [StringLength(20, ErrorMessage = "O tamanho máximo para o nome do tipo de câmbio é de 20 caracteres!")]
        [Display(Name = "Nome")]
        public string Nome {  get; set; }
    }
}
