using DigitalWallet.Application.Base;
using DigitalWallet.Application.Dtos;
using DigitalWallet.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace DigitalWallet.Application.Services
{
    public class GetService
    {
        private readonly IRepository _repository;

        public GetService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<WalletDto>> GetAsync(Guid id)
        {
            var wallet = await _repository.FindAsync(id);

            if (wallet.Id == default)
            {
                return ServiceResult<WalletDto>.NotFound();
            }

            var dto = new WalletDto
            {
                Id = wallet.Id,
                Credit = wallet.Credit,
                Owner = wallet.Owner
            };

            return ServiceResult<WalletDto>.Ok(dto);
        }
    }
}
