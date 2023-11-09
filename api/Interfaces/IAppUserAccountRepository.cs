using api.Models;

namespace api.Interfaces;

public interface IAppUserAccountRepository
{
    Task<AppUserDto?> Create(RegisterDto userInput, CancellationToken cancellationToken);
}
