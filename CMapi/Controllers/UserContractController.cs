using Microsoft.AspNetCore.Mvc;
using CMLibrary.DataAccess;
using CMLibrary.Models;

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
        if (model == null)
            return BadRequest("Invalid model.");

        var newId = await _userContractData.CreateContract(model);

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
    public async Task<ActionResult<List<UserContractModel>>> GetContractsByUser(Guid userAccountId, int pageNumber = 1, int pageSize = 10)
    {
        // Optionally, we can cross-check with the user's token if needed
        var contracts = await _userContractData.GetContractsByUser(userAccountId, pageNumber, pageSize);
        return Ok(contracts);
    }

    [HttpPut("update-contract")]
    public async Task<IActionResult> Update(UserContractModel model)
    {
        // Validate model
        if (model == null)
        {
            return BadRequest("Invalid model.");
        }

        if (model.Id == Guid.Empty)
        {
            return BadRequest("Invalid contract ID.");
        }

        var success = await _userContractData.UpdateContract(model);

        if (!success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update contract.");
        }

        return Ok("Contract updated successfully.");
    }

    [HttpDelete(template: "delete-contract")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _userContractData.SoftDeleteContract(id);

        if (!success)
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete contract.");

        return Ok("Contract deleted (soft-delete) successfully.");
    }
}
