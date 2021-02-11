using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVTR.Account.Domain.Models
{
  /// <summary>
  /// Represents the _Payment_ model
  /// </summary>
  public class PaymentModel : AEntity, IValidatableObject
  {

    [Required(ErrorMessage = "Expiration date required")]
    public DateTime CardExpirationDate { get; set; }

    [Required(ErrorMessage = "Card number required")]
    [RegularExpression(@"(^\d{16}$|^\d{4}-\d{4}-\d{4}-\d{4}$)", ErrorMessage = "Credit card must be properly formatted as 16 digits with or without dashes")]
    public string CardNumber { get; set; }

    [Required(ErrorMessage = "Security code required")]
    [StringLength(3, ErrorMessage = "Security code must be 3 digits long")]
    [RegularExpression(@"^\d{3}$", ErrorMessage = "Security code must be 3 digits")]
    public string SecurityCode { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Name must start with a capital letter and only use letters.")]
    public string CardName { get; set; }

    public int AccountModelId { get; set; }

    /// <summary>
    /// Represents the _Payment_ `Validate` method
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (string.IsNullOrEmpty(CardName))
      {
        yield return new ValidationResult("cardName cannot be null.");
      }
      if (string.IsNullOrEmpty(CardNumber))
      {
        yield return new ValidationResult("cardNumber cannot be null.");
      }
    }
  }
}
