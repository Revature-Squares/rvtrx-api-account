using RVTR.Account.Domain.Models;
using System.Threading.Tasks;

namespace RVTR.Account.Domain.Interfaces
{
  public interface IAccountRepository : IRepository<AccountModel>
  {
    Task<AccountModel> SelectByEmailAsync(string email);
  }
}
