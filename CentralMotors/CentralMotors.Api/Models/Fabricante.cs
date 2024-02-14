using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CentralMotors.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;

namespace CentralMotors.Models
{
    [Table("Fabricante")]
    public class Fabricante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FabricanteId { get; set; }

        [Required(ErrorMessage = "Informe o nome do fabricante do veiculo!")]
        [StringLength(40, ErrorMessage = "O tamanho máximo para o nome do fabricante é de 40 caracteres!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
    }

}
