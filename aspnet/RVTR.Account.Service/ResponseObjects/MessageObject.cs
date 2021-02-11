namespace RVTR.Account.Service.ResponseObjects
{
  /// <summary>
  /// The _Message Object_ class
  /// </summary>
  public class MessageObject
  {
    /// <summary>
    /// used to display a success reponse message alongside a status code
    /// </summary>
    public static readonly MessageObject Success = new MessageObject("Success");

    /// <summary>
    /// The _Message Object_ class has a message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// The _Message Object_ constructor
    /// </summary>
    /// <param name="message"></param>
    public MessageObject(string message)
    {
      Message = message;
    }
  }
}
