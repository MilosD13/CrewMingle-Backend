using Microsoft.AspNetCore.Mvc;
using CMLibrary.DataAccess;
using CMLibrary.Models;
using System.Security.Claims;

namespace CMapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserContractController : ControllerBase
{
    private readonly IUserContractDataAccess _userContractData;

    public UserContractController(IUserContractDataAccess userContractData)
    {
        _userContractData = userContractData;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(UserContractModel model)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        UserFirebaseModel user = new UserFirebaseModel
        {
            UserId = userId,
            Email = email
        };

        if (user == null || userId == null)
        {
            return Unauthorized();
        }

        if (model == null)
            return BadRequest("Invalid model.");

        var newId = await _userContractData.CreateContract(model, userId);

        if (newId.HasValue && newId != Guid.Empty)
        {
            return Ok(new { ContractId = newId.Value });
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create contract.");
        }
    }

    [HttpGet("get-contract-by-id")]
    public async Task<ActionResult<UserContractModel>> GetContract(Guid id)
    {
        var contract = await _userContractData.GetContractById(id);

        if (contract == null)
            return NotFound($"Contract {id} not found.");

        return Ok(contract);
    }

    [HttpGet("get-all-contracts")]
    public async Task<ActionResult<List<UserContractModel>>> GetContractsByUser(int pageNumber = 1, int pageSize = 10)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        UserFirebaseModel user = new UserFirebaseModel
        {
            UserId = userId,
            Email = email
        };

        if (user == null || userId == null)
        {
            return Unauthorized();
        }

        // Optionally, we can cross-check with the user's token if needed
        var contracts = await _userContractData.GetContractsByUser(userId, pageNumber, pageSize);
        return Ok(contracts);
    }

    [HttpPut("update-contract")]
    public async Task<IActionResult> Update(UserContractModel model)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        UserFirebaseModel user = new UserFirebaseModel
        {
            UserId = userId,
            Email = email
        };

        if (user == null || userId == null)
        {
            return Unauthorized();
        }

        // Validate model
        if (model == null)
        {
            return BadRequest("Invalid model.");
        }

        if (model.Id == Guid.Empty)
        {
            return BadRequest("Invalid contract ID.");
        }

        var success = await _userContractData.UpdateContract(model, userId);

        if (!success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update contract.");
        }

        return Ok("Contract updated successfully.");
    }

    [HttpDelete(template: "delete-contract")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        UserFirebaseModel user = new UserFirebaseModel
        {
            UserId = userId,
            Email = email
        };

        if (user == null || userId == null)
        {
            return Unauthorized();
        }

        var success = await _userContractData.SoftDeleteContract(id, userId);

        if (!success)
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete contract.");

        return Ok("Contract deleted (soft-delete) successfully.");
    }
}
