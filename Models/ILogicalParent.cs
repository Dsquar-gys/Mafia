using System;

namespace Mafia.Models;

public interface ILogicalParent
{
    IServiceProvider Services { get; }
}