using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Models;

namespace RiffBackend.Infrastructure;

public sealed class EmailVerificationLinkFactory(IHttpContextAccessor contextAccessor, LinkGenerator generator) : IEmailVerificationLinkFactory
{
    public string Create(EmailVerificationToken token)
    {
        string? verificationLink =
            generator.GetUriByName(contextAccessor.HttpContext!, "VerifyEmail", new {token = token.Id});
        
        return verificationLink ??  string.Empty;
    }
}