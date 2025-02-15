using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace viamatica_backend.Models.Request
{
    public class PersonaRequest : IValidatableObject
    {

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [MinLength(8, ErrorMessage = "El nombre de usuario debe tener al menos 8 caracteres.")]
        [MaxLength(20, ErrorMessage = "El nombre de usuario no debe exceder los 20 caracteres.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)[^\W_]+$", ErrorMessage = "El nombre de usuario debe contener al menos una letra mayúscula, un número y no debe contener signos.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "La identificación debe tener exactamente 10 dígitos y solo números.")]
        public string Identificacion { get; set; } = null!;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateOnly FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\W)(?!.*\s).*$", ErrorMessage = "La contraseña debe tener al menos una letra mayúscula, un signo y no debe contener espacios.")]
        public string Contrasena { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validación de identificación: No debe tener un número repetido 4 veces seguidas
            if (Regex.IsMatch(Identificacion, @"(\d)\1{3}"))
            {
                yield return new ValidationResult("La identificación no puede contener un número repetido 4 veces seguidas.", new[] { nameof(Identificacion) });
            }
        }
    }
}
