﻿//-----------------------------------------------------------------------
// <copyright file="UsernameCriteria.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>Criteria class for passing a</summary>
//-----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Csla.Security
{
  /// <summary>
  /// Criteria class for passing a
  /// username/password pair to a
  /// custom identity class.
  /// </summary>
  [Serializable]
  public class UsernameCriteria : ReadOnlyBase<UsernameCriteria>
  {
    /// <summary>
    /// Username property definition.
    /// </summary>
    public static readonly PropertyInfo<string> UsernameProperty = RegisterProperty<string>(c => c.Username);
    /// <summary>
    /// Username property definition.
    /// </summary>
    public string Username
    {
      get => ReadProperty(UsernameProperty)!;
      private set => LoadProperty(UsernameProperty, value);
    }

    /// <summary>
    /// Password property definition.
    /// </summary>
    public static readonly PropertyInfo<string> PasswordProperty = RegisterProperty<string>(c => c.Password);
    /// <summary>
    /// Gets the password.
    /// </summary>
    public string Password
    {
      get => ReadProperty(PasswordProperty)!;
      private set => LoadProperty(PasswordProperty, value);
    }

    /// <summary>
    /// Creates a new instance of the object.
    /// </summary>
    /// <param name="username">
    /// Username value.
    /// </param>
    /// <param name="password">
    /// Password value.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="username"/> or <paramref name="password"/> is <see langword="null"/>.</exception>
    public UsernameCriteria(string username, string password)
    {
      Username = username ?? throw new ArgumentNullException(nameof(username));
      Password = password ?? throw new ArgumentNullException(nameof(password));
    }

    /// <summary>
    /// Creates a new instance of the object.
    /// </summary>
#if ANDROID || IOS
    public UsernameCriteria()
    { }
#else
    protected UsernameCriteria()
    { }
#endif
  }
}