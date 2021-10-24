using DigitalWallet.Application.Base;
using DigitalWallet.Application.Inputs;
using DigitalWallet.Domain.Repositories;
using DigitalWallet.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace DigitalWallet.Application.Services
{
    public class DepositService
    {
        private readonly IRepository _repository;

        public DepositService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult> DepositAsync(Guid id, DepositOrWithdrawInput input)
        {
            if (input.Credit <= 0)
            {
                return ServiceResult.Error(ErrorCodes.InvalidCredit);
            }

            var wallet = await _repository.FindAsync(id);

            if (wallet.Id == default)
            {
                return ServiceResult.NotFound();
            }

            wallet.Deposit(new Money(input.Credit));

            await _repository.SaveChangesAsync(wallet);

            return ServiceResult.Ok();
        }
    }
}
