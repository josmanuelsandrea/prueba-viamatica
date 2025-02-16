using System.ComponentModel.DataAnnotations;

namespace viamatica_backend.Models.Request
{
    public class TokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
