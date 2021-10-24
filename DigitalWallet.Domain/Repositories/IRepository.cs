using DigitalWallet.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace DigitalWallet.Domain.Repositories
{
    public interface IRepository
    {
        Task<Wallet> FindAsync(Guid id);

        Task AddAsync(Wallet wallet);

        Task SaveChangesAsync(Wallet wallet);
    }
}
