using System.Text.RegularExpressions;

namespace viamatica_backend.Tools
{
    public class UsernameGeneration
    {
        private static Random _random = new Random();

        public static string GenerateUsername(string nombres, string apellidos)
        {
            if (string.IsNullOrWhiteSpace(nombres) || string.IsNullOrWhiteSpace(apellidos))
                throw new ArgumentException("Los nombres y apellidos no pueden estar vacíos");

            // Dividir nombres y apellidos en palabras
            var nombresArray = nombres.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var apellidosArray = apellidos.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            // Limpiar caracteres especiales y espacios extra
            string nombre1 = nombresArray.Length > 0 ? Regex.Replace(nombresArray[0].Trim(), @"[^a-zA-Z]", "") : "";
            string nombre2 = nombresArray.Length > 1 ? Regex.Replace(nombresArray[1].Trim(), @"[^a-zA-Z]", "") : "";

            string apellido1 = apellidosArray.Length > 0 ? Regex.Replace(apellidosArray[0].Trim(), @"[^a-zA-Z]", "") : "";
            string apellido2 = apellidosArray.Length > 1 ? Regex.Replace(apellidosArray[1].Trim(), @"[^a-zA-Z]", "") : "";

            // Construir el nombre de usuario basado en lo que esté disponible
            string username = "";

            if (!string.IsNullOrEmpty(nombre1))
                username += char.ToUpper(nombre1[0]) + nombre1.Substring(1).ToLower(); // Primera letra mayúscula

            if (!string.IsNullOrEmpty(nombre2))
                username += nombre2.ToLower();

            if (!string.IsNullOrEmpty(apellido1))
                username += char.ToUpper(apellido1[0]) + apellido1.Substring(1).ToLower(); // Primera letra mayúscula

            if (!string.IsNullOrEmpty(apellido2))
                username += apellido2.ToLower();

            // Agregar un número aleatorio al final (100-999)
            int randomNumber = _random.Next(100, 999);
            username += randomNumber;

            return username;
        }
    }
}
