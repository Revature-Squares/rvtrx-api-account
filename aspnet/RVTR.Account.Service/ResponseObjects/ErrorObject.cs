namespace RVTR.Account.Service.ResponseObjects
{
  /// <summary>
  /// Represents the _Error Object_ class
  /// </summary>
  public class ErrorObject : MessageObject
  {
    /// <summary>
    /// The _Error Object_ class has an error message
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Used to display an error response message alongside a status code
    /// </summary>
    /// <param name="message"></param>
    public ErrorObject(string message) : base("")
    {
      ErrorMessage = message;
    }
  }
}
