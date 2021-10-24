using System.ComponentModel.DataAnnotations;

namespace DigitalWallet.Application.Inputs
{
    public class CreateInput
    {
        [Required]
        [MaxLength(128)]
        public string Owner { get; set; }
    }
}
