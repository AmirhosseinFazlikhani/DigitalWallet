using DigitalWallet.Application.Base;
using DigitalWallet.Application.Inputs;
using DigitalWallet.Domain.Repositories;
using DigitalWallet.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace DigitalWallet.Application.Services
{
    public class WithdrawService
    {
        private readonly IRepository _repository;

        public WithdrawService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult> WithdrawAsync(Guid id, DepositOrWithdrawInput input)
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

            if (input.Credit > wallet.Credit)
            {
                return ServiceResult.Error(ErrorCodes.LackOfCredit);
            }

            wallet.Withdraw(new Money(input.Credit));

            await _repository.SaveChangesAsync(wallet);

            return ServiceResult.Ok();
        }
    }
}
