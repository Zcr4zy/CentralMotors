using Humanizer.Localisation.TimeToClockNotation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentralMotors.Models
{
    [Table("Veiculo")]
    public class Veiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VeiculoId { get; set; }


        [Required(ErrorMessage = "Informe o ano do modelo do veículo!")]
        [Display(Name = "AnoModelo")]
        public int AnoModelo {  get; set; }


        [Required(ErrorMessage = "Informe o ano de fabricação do veículo!")]
        [Display(Name = "AnoFabricação")]
        public int AnoFabricacao { get; set; }


        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Informe o valor do veículo!")]
        [Display(Name = "Valor")]
        public double Valor { get; set; }

        [Required(ErrorMessage = "Informe a quantidade de quilômetros rodados pelo veículo!")]
        [Display(Name = "KmRodados")]
        public string KmRodados { get; set; }

        [Required(ErrorMessage = "Informe o motor do veículo!")]
        [Display(Name = "Motor")]
        public string Motor {  get; set; }

        [Required(ErrorMessage = "Informe a localização do veículo!")]
        [Display(Name = "Localização")]
        public string Localizacao { get; set; }

        [Display(Name ="Foto")]
        public string Foto1 {  get; set; }

        [Display(Name ="Foto2")]
        public string Foto2 { get; set; }



        [Required]
        public int CorId { get; set; }
        [ForeignKey("CorId")]
        public Cor Cor {  get; set; }



        [Required]
        public int ModeloId { get; set; }

        [ForeignKey("ModeloId")]
        public Modelo Modelo { get; set; }


        [Required]
        public int TipoId { get; set; }
        [ForeignKey("TipoId")]
        public Tipo Tipo { get; set; }


        [Required]
        public int TipoCombustivelId { get; set; }
        [ForeignKey("TipoCombustivelId")]
        public TipoCombustivel TipoCombustivel { get; set; }


        [Required]
        public int TipoTransmissaoId { get; set; }
        [ForeignKey("TipoTransmissaoId")]
        public TipoTransmissao TipoTransmissao { get; set; }

    }
}
