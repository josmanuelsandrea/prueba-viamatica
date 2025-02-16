using OfficeOpenXml;
using viamatica_backend.Models.Request;

namespace viamatica_backend.Tools
{
    public class ExcelReader
    {
        public static async Task<List<PersonaRequest>> ParseUsuariosDesdeExcelAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("El archivo XLSX es requerido.");

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; // Requerido por EPPlus

            var usuarios = new List<PersonaRequest>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Obtener la primera hoja
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // Saltar la primera fila (encabezados)
                    {
                        var fechaNacimiento = ConvertirFecha(worksheet.Cells[row, 4].Value);
                        var Nombres = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                        var Apellidos = worksheet.Cells[row, 2].Value?.ToString()?.Trim();

                        var usuario = new PersonaRequest
                        {
                            Nombres = Nombres,
                            Apellidos = Apellidos,
                            Identificacion = worksheet.Cells[row, 3].Value?.ToString()?.Trim(),
                            Contrasena = "default-password",
                            FechaNacimiento = ConvertirFecha(worksheet.Cells[row, 4].Value) ?? new DateOnly(2000, 1, 1),
                            UserName = UsernameGeneration.GenerateUsername(Nombres, Apellidos)
                        };

                        usuarios.Add(usuario);
                    }
                }
            }

            return usuarios;
        }

        private static DateOnly? ConvertirFecha(object value)
        {
            if (value == null)
                return null;

            if (value is double excelDate) // Caso: Fecha almacenada como número en Excel
            {
                try
                {
                    DateTime fecha = DateTime.FromOADate(excelDate);
                    return DateOnly.FromDateTime(fecha);
                }
                catch
                {
                    return null; // Fecha inválida
                }
            }

            if (value is string fechaStr) // Caso: Fecha almacenada como texto
            {
                if (DateOnly.TryParse(fechaStr, out var fechaParseada))
                    return fechaParseada;

                // Intentar con formato común "dd/MM/yyyy"
                if (DateOnly.TryParseExact(fechaStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var fechaExacta))
                    return fechaExacta;
            }

            return null; // No se pudo convertir
        }
    }
}
