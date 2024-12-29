using CMLibrary.DataAccess;
using CMLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



namespace CMapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PortController : ControllerBase
{
    private readonly IPortDataAccess _portData;

    public PortController(IPortDataAccess portData)
    {
         _portData = portData;
    }

    [HttpGet("port-crew")]
    public async Task<ActionResult<CrewScheduleResultsModel>> GetCrewByPortItinerary(int portId, DateTime arrivalDate, DateTime departureDate, TimeSpan arrivalTime, TimeSpan departureTime, int pageNumber = 1, int pageSize = 10)
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

        try
        {
            PortScheduleModel port = new PortScheduleModel
            {
                PortId = portId,
                ArrivalDate = arrivalDate,
                DepartureDate = departureDate,
                ArrivalTime = arrivalTime,
                DepartureTime = departureTime
            };

            var data = await _portData.GetCrewByPort(userId, port, pageNumber, pageSize);

            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest("There was an issue with your request");
        }
    }
    [HttpGet("ship-schedule-crew")]
    public async Task<ActionResult<CrewScheduleResultsModel>> GetCrewbyShipSchedule(int shipId, DateTime arrivalDate,DateTime departureDate,  int pageNumber = 1, int pageSize = 10)
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

        try
        {
            ShipCrewScheduleModel ship = new ShipCrewScheduleModel
            {
                ShipId = shipId,
                ArrivalDate = arrivalDate,
                DepartureDate = departureDate
            };

            var data = await _portData.GetCrewScheduleByShip(userId, ship, pageNumber, pageSize);

            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest("There was an issue with your request");
        }
    }
}
