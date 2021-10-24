using System;

namespace DigitalWallet.Application.Dtos
{
    public class WalletDto
    {
        public Guid Id { get; set; }

        public string Owner { get; set; }

        public decimal Credit { get; set; }
    }
}
