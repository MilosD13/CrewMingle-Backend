using Microsoft.AspNetCore.Mvc;
using CMLibrary.DataAccess;
using CMLibrary.Models;

namespace CMapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShipController : ControllerBase
{
    private readonly IShipDataAccess _shipData;

    public ShipController(IShipDataAccess shipData)
    {
        _shipData = shipData;
    }

    // GET: api/ship/cruise-lines
    [HttpGet("cruise-lines")]
    public async Task<ActionResult<List<CruiseLineModel>>> GetCruiseLines()
    {
        try
        {
            var lines = await _shipData.GetCruiseLines();
            return Ok(lines);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get cruise lines.");
        }
    }

    // GET: api/ship/ships?cruiseLineId=2
    [HttpGet("ships")]
    public async Task<ActionResult<List<ShipModel>>> GetShips([FromQuery] int? cruiseLineId)
    {
        try
        {
            var ships = await _shipData.GetShips(cruiseLineId);
            return Ok(ships);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get ships.");
        }
    }

    // GET: api/ship/itinerary-structured?shipId=636
    [HttpGet("itinerary")]
    public async Task<ActionResult<ShipModel>> GetShipItineraryStructured(int shipId)
    {
        try
        {
            var model = await _shipData.GetShipItineraryStructured(shipId);
            if (model == null)
                return NotFound($"No itinerary found for ship ID {shipId}");

            return Ok(model);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get itinerary data.");
        }
    }
}
