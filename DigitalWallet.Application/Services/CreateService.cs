using DigitalWallet.Application.Base;
using DigitalWallet.Application.Inputs;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Repositories;
using DigitalWallet.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace DigitalWallet.Application.Services
{
    public class CreateService
    {
        private readonly IRepository _repository;

        public CreateService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<Guid>> CreateAsync(CreateInput input)
        {
            var wallet = new Wallet(new UserId(input.Owner));

            await _repository.AddAsync(wallet);

            return ServiceResult<Guid>.Ok(wallet.Id);
        }
    }
}
