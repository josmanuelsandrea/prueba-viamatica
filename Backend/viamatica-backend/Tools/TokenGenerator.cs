using System.Security.Cryptography;

namespace viamatica_backend.Tools
{
    public class TokenGenerator
    {
        public static string GenerateToken(int userId)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] tokenData = new byte[32]; // Generamos 32 bytes aleatorios
                rng.GetBytes(tokenData);
                string token = Convert.ToBase64String(tokenData);

                // Opcional: Agregamos el ID del usuario como parte del token
                return $"{userId}-{token}";
            }
        }
    }
}
