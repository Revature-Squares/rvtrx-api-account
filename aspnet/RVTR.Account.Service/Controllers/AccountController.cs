using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVTR.Account.Domain.Interfaces;
using RVTR.Account.Domain.Models;
using RVTR.Account.Service.ResponseObjects;

namespace RVTR.Account.Service.Controllers
{
  /// <summary>
  ///
  /// </summary>
  [ApiController]
  [ApiVersion("0.0")]
  [EnableCors("Public")]
  [Route("rest/account/{version:apiVersion}/[controller]")]
  public class AccountController : ControllerBase
  {
    private readonly ILogger<AccountController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    ///
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="unitOfWork"></param>
    public AccountController(ILogger<AccountController> logger, IUnitOfWork unitOfWork)
    {
      _logger = logger;
      _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Delete a user's account by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpDelete("{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string email)
    {
      try
      {
        _logger.LogDebug("Deleting an account by its email...");

        // Instead of directly deleting by passed ID, search for account (& it's ID) from passed email first
        AccountModel accountModel = await _unitOfWork.Account.SelectByEmailAsync(email);

        await _unitOfWork.Account.DeleteAsync(accountModel.EntityId);
        await _unitOfWork.CommitAsync();


        _logger.LogInformation($"Deleted the account with email {email}.");

        return Ok(MessageObject.Success);
      }
      catch
      {
        _logger.LogWarning($"Account with email {email} does not exist.");

        return NotFound(new ErrorObject($"Account with email {email} does not exist"));
      }
    }

    /// <summary>
    /// Get all user accounts available
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AccountModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
      _logger.LogInformation($"Retrieved the accounts.");

      return Ok(await _unitOfWork.Account.SelectAsync());

    }

    /// <summary>
    /// Get a user's account via email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpGet("{email}")]
    [ProducesResponseType(typeof(AccountModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string email)
    {
      _logger.LogDebug("Getting an account by its email...");

      AccountModel accountModel = await _unitOfWork.Account.SelectByEmailAsync(email);

      if (accountModel is AccountModel theAccount)
      {
        _logger.LogInformation($"Retrieved the account with email {email}.");

        return Ok(theAccount);
      }

      _logger.LogWarning($"Account with email {email} does not exist.");

      return NotFound(new ErrorObject($"Account with email {email} does not exist."));
    }

    /// <summary>
    /// Add an account
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> PostAccount([FromBody] AccountModel account)
    {

      _logger.LogDebug("Adding an account...");

      await _unitOfWork.Account.InsertAsync(account);
      await _unitOfWork.CommitAsync();

      _logger.LogInformation($"Successfully added the account {account}.");

      return Accepted(account);

    }

    /// <summary>
    /// Update an existing account
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put([FromBody] AccountModel account)
    {
      try
      {
        _logger.LogDebug("Updating an account...");

        _unitOfWork.Account.Update(account);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation($"Successfully updated the account {account}.");

        return Accepted(account);
      }

      catch
      {
        _logger.LogWarning($"This account does not exist.");

        return NotFound(new ErrorObject($"Account with ID number {account.EntityId} does not exist"));
      }

    }
    /// <summary>
    /// Delete a user's address
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAddress(int id)
    {
      try
      {
        _logger.LogDebug("Deleting an address by its ID number...");

        await _unitOfWork.Address.DeleteAsync(id);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation($"Deleted the address with ID number {id}.");

        return Ok(MessageObject.Success);
      }
      catch
      {
        _logger.LogWarning($"Address with ID number {id} does not exist.");

        return NotFound(new ErrorObject($"Address with ID number {id} does not exist."));
      }
    }

    /// <summary>
    /// Get all addresses
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AddressModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAddress()
    {
      _logger.LogInformation($"Retrieved the addresses.");

      return Ok(await _unitOfWork.Address.SelectAsync());
    }

    /// <summary>
    /// Get a user's address with address ID number
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AddressModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAddress(int id)
    {
      AddressModel addressModel;

      _logger.LogDebug("Getting an address by its ID number...");

      addressModel = await _unitOfWork.Address.SelectAsync(id);


      if (addressModel is AddressModel theAddress)
      {
        _logger.LogInformation($"Retrieved the address with ID: {id}.");

        return Ok(theAddress);
      }

      _logger.LogWarning($"Address with ID number {id} does not exist.");

      return NotFound(new ErrorObject($"Address with ID number {id} does not exist."));
    }

    /// <summary>
    /// Add an address to an account
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> PostAddress(AddressModel address)
    {
      _logger.LogDebug("Adding an address...");

      await _unitOfWork.Address.InsertAsync(address);
      await _unitOfWork.CommitAsync();

      _logger.LogInformation($"Successfully added the address {address}.");

      return Accepted(address);
    }

    /// <summary>
    /// Update a user's address
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutAddress(AddressModel address)
    {
      try
      {
        _logger.LogDebug("Updating an address...");

        _unitOfWork.Address.Update(address);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation($"Successfully updated the address {address}.");

        return Accepted(address);

      }
      catch
      {
        _logger.LogWarning($"This address does not exist.");

        return NotFound(new ErrorObject($"Address with ID number {address.EntityId} does not exist."));

      }

    }
    /// <summary>
    /// Deletes a user's payment information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePayment(int id)
    {
      try
      {
        _logger.LogDebug("Deleting a payment by its ID number...");

        await _unitOfWork.Payment.DeleteAsync(id);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation($"Deleted the payment with ID number {id}.");

        return Ok(MessageObject.Success);
      }
      catch
      {
        _logger.LogWarning($"Payment with ID number {id} does not exist.");

        return NotFound(new ErrorObject($"Payment with ID number {id} does not exist"));
      }

    }

    /// <summary>
    /// Retrieves all payments
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PaymentModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPayment()
    {
      _logger.LogInformation($"Retrieved the payments.");

      return Ok(await _unitOfWork.Payment.SelectAsync());
    }

    /// <summary>
    /// Retrieves a payment by payment ID number
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PaymentModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPayment(int id)
    {
      PaymentModel paymentModel;

      _logger.LogDebug("Getting a payment by its ID number...");

      paymentModel = await _unitOfWork.Payment.SelectAsync(id);


      if (paymentModel is PaymentModel thePayment)
      {
        _logger.LogInformation($"Retrieved the payment with ID: {id}.");

        return Ok(thePayment);
      }

      _logger.LogWarning($"Payment with ID number {id} does not exist.");

      return NotFound(new ErrorObject($"Payment with ID number {id} does not exist."));
    }

    /// <summary>
    /// Adds a payment to an account
    /// </summary>
    /// <param name="payment"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> PostPayment(PaymentModel payment)
    {
      _logger.LogDebug("Adding a payment...");

      await _unitOfWork.Payment.InsertAsync(payment);
      await _unitOfWork.CommitAsync();

      _logger.LogInformation($"Successfully added the payment {payment}.");

      return Accepted(payment);

    }

    /// <summary>
    /// Updates a payment
    /// </summary>
    /// <param name="payment"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutPayment(PaymentModel payment)
    {
      try
      {
        _logger.LogDebug("Updating a payment...");

        _unitOfWork.Payment.Update(payment);
        await _unitOfWork.CommitAsync();


        _logger.LogInformation($"Successfully updated the payment {payment}.");

        return Accepted(payment);
      }

      catch
      {
        _logger.LogWarning($"This payment does not exist.");

        return NotFound(new ErrorObject($"Payment with ID number {payment.EntityId} does not exist"));
      }
    }
    /// <summary>
    /// Delete a user's profile
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProfile(int id)
    {
      try
      {
        _logger.LogDebug("Deleting a profile by its ID number...");

        await _unitOfWork.Profile.DeleteAsync(id);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation($"Deleted the profile with ID number {id}.");

        return Ok(MessageObject.Success);
      }
      catch
      {
        _logger.LogWarning($"Profile with ID number {id} does not exist.");

        return NotFound(new ErrorObject($"Profile with ID number {id} does not exist."));
      }
    }

    /// <summary>
    /// Get all profiles
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProfileModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile()
    {
      _logger.LogInformation($"Retrieved the profiles.");

      return Ok(await _unitOfWork.Profile.SelectAsync());
    }

    /// <summary>
    /// Get a user's profile with profile ID number
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProfileModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(int id)
    {
      ProfileModel profileModel;

      _logger.LogDebug("Getting a profile by its ID number...");

      profileModel = await _unitOfWork.Profile.SelectAsync(id);


      if (profileModel is ProfileModel theProfile)
      {
        _logger.LogInformation($"Retrieved the profile with ID: {id}.");

        return Ok(theProfile);
      }

      _logger.LogWarning($"Profile with ID number {id} does not exist.");

      return NotFound(new ErrorObject($"Profile with ID number {id} does not exist."));
    }

    /// <summary>
    /// Add a profile to an account
    /// </summary>
    /// <param name="profile"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> PostProfile(ProfileModel profile)
    {
      _logger.LogDebug("Adding a profile...");

      await _unitOfWork.Profile.InsertAsync(profile);
      await _unitOfWork.CommitAsync();

      _logger.LogInformation($"Successfully added the profile {profile}.");

      return Accepted(profile);
    }

    /// <summary>
    /// Update a user's profile
    /// </summary>
    /// <param name="profile"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutProfile(ProfileModel profile)
    {
      try
      {
        _logger.LogDebug("Updating a profile...");

        _unitOfWork.Profile.Update(profile);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation($"Successfully updated the profile {profile}.");

        return Accepted(profile);
      }
      catch
      {
        _logger.LogWarning($"This profile does not exist.");

        return NotFound(new ErrorObject($"Profile with ID number {profile.EntityId} does not exist."));
      }

    }
  }
}
