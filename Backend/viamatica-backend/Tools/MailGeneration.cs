using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace viamatica_backend.Tools
{
    public static class MailGeneration
    {
        public static string GenerateEmail(string nombres, string apellidos, int? numero = null, string dominio = "mail.com")
        {
            if (string.IsNullOrWhiteSpace(nombres) || string.IsNullOrWhiteSpace(apellidos))
            {
                throw new ArgumentException("Los nombres y apellidos no pueden estar vacíos.");
            }

            // Separar nombres y apellidos
            string[] nombresArray = nombres.Trim().Split(' ');
            string[] apellidosArray = apellidos.Trim().Split(' ');

            // Obtener la primera letra del primer nombre
            string inicialNombre = nombresArray[0].Substring(0, 1).ToLower();

            // Obtener el primer apellido completo
            string primerApellido = apellidosArray[0].ToLower();

            // Obtener la primera letra del segundo apellido si existe
            string inicialSegundoApellido = apellidosArray.Length > 1 ? apellidosArray[1].Substring(0, 1).ToLower() : "";

            // Generar el email base
            string emailBase = $"{inicialNombre}{primerApellido}{inicialSegundoApellido}";

            // Si se pasa un número, concatenarlo
            if (numero.HasValue)
            {
                emailBase += numero.Value;
            }

            // Agregar el dominio
            string email = $"{emailBase}@{dominio}";

            // Remover caracteres especiales y tildes
            email = RemoveSpecialCharacters(email);

            return email;
        }

        private static string RemoveSpecialCharacters(string input)
        {
            // Reemplazar caracteres con tilde por su versión sin tilde
            string normalized = input.Normalize(System.Text.NormalizationForm.FormD);
            Regex regex = new Regex("[^a-zA-Z0-9@.]");
            return regex.Replace(normalized, "");
        }
    }
}
