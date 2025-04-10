﻿//-----------------------------------------------------------------------
// <copyright file="ReadOnlyPerson.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>no summary</summary>
//-----------------------------------------------------------------------

using Csla.TestHelpers;

namespace Csla.Test.Authorization
{
#if TESTING
  [DebuggerNonUserCode]
#endif
  [Serializable]
  public class ReadOnlyPerson : ReadOnlyBase<ReadOnlyPerson>
  {
    private bool _authorizationCheckDisabled;
    protected override bool IsCanReadPropertyAuthorizationCheckDisabled => _authorizationCheckDisabled;

    public static ReadOnlyPerson GetReadOnlyPerson(TestDIContext serviceProvider)
    {
      return serviceProvider.CreateDataPortal<ReadOnlyPerson>().Create();
    }

    [Create, RunLocal]
    private void Create()
    {
      LoadProperty(FirstNameProperty, "John");
      LoadProperty(LastNameProperty, "Doe");
      LoadProperty(MiddleNameProperty, "A");
      LoadProperty(PlaceOfBirthProperty, "New York");
    }

    private static PropertyInfo<string> FirstNameProperty = RegisterProperty<string>(new PropertyInfo<string>("FirstName"));
    public string FirstName
    {
      get
      {
        return GetProperty(FirstNameProperty, Csla.Security.NoAccessBehavior.ThrowException);
      }
    }

   
    private static PropertyInfo<string> LastNameProperty = RegisterProperty<string>(new PropertyInfo<string>("LastName"));

    public string LastName
    {
      get
      {
        return GetProperty(LastNameProperty, Csla.Security.NoAccessBehavior.ThrowException);
      }
    }

    private static PropertyInfo<string> MiddleNameProperty = RegisterProperty<string>(new PropertyInfo<string>("MiddleName"));
    public string MiddleName
    {
      get
      {
        return GetProperty(MiddleNameProperty, Csla.Security.NoAccessBehavior.ThrowException);
      }
    }


    private static PropertyInfo<string> PlaceOfBirthProperty = RegisterProperty<string>(new PropertyInfo<string>("PlaceOfBirth"));

    public string PlaceOfBirth
    {
      get
      {
        return GetProperty(PlaceOfBirthProperty, Csla.Security.NoAccessBehavior.ThrowException);
      }
    }

    protected override void AddBusinessRules()
    {
      BusinessRules.AddRule(new Rules.CommonRules.IsInRole(Rules.AuthorizationActions.ReadProperty, FirstNameProperty, new List<string> { "Admin" }));
      BusinessRules.AddRule(new Rules.CommonRules.IsNotInRole(Rules.AuthorizationActions.ReadProperty, MiddleNameProperty, new List<string> { "Admin" }));
    }

    internal void SetDisableCanReadAuthorizationChecks(bool isCanReadAuthorizationChecksDisabled) => _authorizationCheckDisabled = isCanReadAuthorizationChecksDisabled;

    //protected override void AddInstanceAuthorizationRules()
    //{
    //  base.AddInstanceAuthorizationRules();
    //  AuthorizationRules.InstanceAllowRead("LastName", "Admin");
    //  AuthorizationRules.InstanceDenyRead("PlaceOfBirth", "Admin");
    //}

    //protected override object GetIdValue()
    //{
    //  return null;
    //}
  }
}