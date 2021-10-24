using System.ComponentModel.DataAnnotations;

namespace DigitalWallet.Application.Inputs
{
    public class DepositOrWithdrawInput
    {
        [Required]
        public decimal Credit { get; set; }
    }
}
