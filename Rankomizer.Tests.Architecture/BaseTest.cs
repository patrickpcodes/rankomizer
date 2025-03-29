using System.Reflection;
using Rankomizer.Application.Abstractions.Data;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Infrastructure.Database;
using Rankomizer.Server.Api;

namespace Rankomizer.Tests.Architecture;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Result).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(IApplicationDbContext).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly; 
}