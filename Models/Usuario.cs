using System.ComponentModel.DataAnnotations;
namespace TiendaVirtualValentina.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Correo inválido")]
    public string Correo { get; set; }

    [Required]
    public string Rol { get; set; }

    [Required]
    [RegularExpression(@"^3\d{9}$",
    ErrorMessage = "El celular debe estar entre 3000000000 y 3999999999")]
    public string Celular { get; set; }

    [Required]
    [MinLength(4, ErrorMessage = "Mínimo 4 caracteres")]
    public string Clave { get; set; }
}



