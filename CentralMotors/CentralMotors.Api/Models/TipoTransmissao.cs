using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentralMotors.Models
{
    [Table("TipoTransmissao")]
    public class TipoTransmissao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TipoTransmissaoId { get; set; }

        [Required(ErrorMessage = "Informe o nome do tipo de transmissão!")]
        [StringLength(20, ErrorMessage = "O tamanho máximo para o nome do tipo de transmissão é de 20 caracteres!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
    }
}
