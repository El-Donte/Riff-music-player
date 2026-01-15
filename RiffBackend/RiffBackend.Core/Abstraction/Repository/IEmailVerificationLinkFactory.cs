using RiffBackend.Core.Models;

namespace RiffBackend.Core.Abstraction.Repository;

public interface IEmailVerificationLinkFactory
{ 
    string Create(EmailVerificationToken token);
}