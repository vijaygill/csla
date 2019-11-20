﻿using System.Threading.Tasks;
using Csla.Rules;

namespace Csla.Analyzers.IntegrationTests
{
  public class ExecuteWithoutAdd  
    : BusinessRule
  {
    protected override void Execute(IRuleContext context)
    {
      // This method should have an error
      // because there are no Add...() calls.
    }
  }

  public class AsynchronousRuleOldSchool
    : BusinessRule
  {
    protected override async void Execute(IRuleContext context)
    {
      // This method should have an error
      // because we are not inheriting from BusinessRuleAsync.
      await DummyAsync();
      context.Complete();
    }

    private static Task DummyAsync() => Task.CompletedTask;
  }

  public class NotCallingComplete
    : BusinessRuleAsync
  {
    protected override Task ExecuteAsync(IRuleContext context)
    {
      // This method should have an error
      // because we don't call Complete().
      return Task.CompletedTask;
    }
  }
}
