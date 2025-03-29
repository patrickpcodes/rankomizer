using Rankomizer.Domain.User;

namespace Rankomizer.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string Create(ApplicationUser user);
}