using Humanizer.Localisation.TimeToClockNotation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentralMotors.Models
{
    [Table("Modelo")]
    public class Modelo
    {
        [Key]
        public int ModeloId { get; set; }

        [Required(ErrorMessage = "Informe o nome do modelo do veiculo!")]
        [StringLength(50, ErrorMessage = "O tamanho máximo para o nome do modelo de veículo é de 50 caracteres!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }


        [Required]
        public int FabricanteId { get; set; }
        [ForeignKey("FabricanteId")]
        public Fabricante Fabricante { get; set; }
    }
}
