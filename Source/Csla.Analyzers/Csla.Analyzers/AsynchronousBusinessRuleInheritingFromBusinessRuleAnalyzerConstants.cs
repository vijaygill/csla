﻿namespace Csla.Analyzers
{
  public static class AsynchronousBusinessRuleInheritingFromBusinessRuleAnalyzerConstants
  {
    public const string Title = "Find Asynchronous Business Rules That Do Not Derive From BusinessRuleAsync";
    public const string Message = "Asynchronous business rules should derive from BusinessRuleAsync.";
  }
}
