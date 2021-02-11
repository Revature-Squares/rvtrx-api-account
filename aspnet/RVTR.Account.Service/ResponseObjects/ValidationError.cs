using System;
namespace RVTR.Account.Service.ResponseObjects
{
  /// <summary>
  /// The _Validation Error_ class
  /// </summary>
  public class ValidationError : ErrorObject
  {
    /// <summary>
    /// The _Validation Error_ constructor
    /// </summary>
    /// <param name="e"></param>
    public ValidationError(ArgumentException e) : base(e.Message)
    {
    }
  }
}
