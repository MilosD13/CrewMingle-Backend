using CMLibrary.DataAccess;
using CMLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrewController : ControllerBase
    {
        private readonly ICrewDataAccess _crewData;

        public CrewController(ICrewDataAccess crewData)
        {
            _crewData = crewData;
        }

        [HttpGet("active-crew")]
        public async Task<ActionResult<List<CrewResultsModel>>> GetCrew (int pageNumber = 1, int pageSize = 10)
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
                var crew = await _crewData.GetActiveCrew(userId, pageNumber, pageSize);

                return Ok(crew);
            }
            catch (Exception ex)
            {
                return BadRequest("There was an issue with your request");
            }
        }

        [HttpGet("active-crew/{crewId}")]
        public async Task<ActionResult<CrewResultsModel>> GetSingleCrew(Guid crewId)
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
                var crew = await _crewData.GetSingleActiveCrew(userId, crewId);

                return Ok(crew);
            }
            catch (Exception ex)
            {
                return BadRequest("There was an issue with your request");
            }
        }

        [HttpGet("pending-crew")]
        public async Task<ActionResult<CrewResultsModel>> GetPendingCrew(int pageNumber = 1, int pageSize = 10)
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
                var crew = await _crewData.GetPendingCrew(userId, pageNumber, pageSize);

                return Ok(crew);
            }
            catch (Exception ex)
            {
                return BadRequest("There was an issue with your request");
            }
        }

        [HttpGet("blocked-crew")]
        public async Task<ActionResult<CrewResultsModel>> GetBlockedCrew(int pageNumber = 1, int pageSize = 10)
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
                var crew = await _crewData.GetBlockedCrew(userId, pageNumber, pageSize);

                return Ok(crew);
            }
            catch (Exception ex)
            {
                return BadRequest("There was an issue with your request");
            }
        }

        [HttpGet("crew-by-ship")]
        public async Task<ActionResult<CrewShipResultsModel>> GetCrewByShip(int shipId, DateTime startDate, DateTime endDate,  int pageNumber = 1, int pageSize = 10)
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
                var crew = await _crewData.GetCrewOnShip(userId, shipId, startDate, endDate, pageNumber, pageSize);

                return Ok(crew);
            }
            catch (Exception ex)
            {
                return BadRequest("There was an issue with your request");
            }
        }

        [HttpGet("crew-ships")]
        public async Task<ActionResult<CrewShipResultsModel>> GetCrewShips(int pageNumber = 1, int pageSize = 10)
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
                var crew = await _crewData.GetCrewShips(userId, pageNumber, pageSize);

                return Ok(crew);
            }
            catch (Exception ex)
            {
                return BadRequest("There was an issue with your request");
            }
        }

        [HttpPost("add-crew")]
        public async Task<IActionResult> AddCrew([FromBody] Guid crewId)
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

            var success = await _crewData.AddCrew(userId, crewId);
            if (!success)
            {
                return StatusCode(500, "Failed to add crew connection.");
            }

            return Ok("Crew connection added successfully.");
        }

        
        [HttpPut("edit-crew")]
        public async Task<IActionResult> EditCrew([FromBody] CrewEditModel crew)
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

            var success = await _crewData.EditCrew(userId, crew);
            if (!success)
            {
                return StatusCode(500, "Failed to modify crew connection.");
            }

            return Ok("Crew connection edited successfully.");
        }

        //// DELETE api/<CrewController>/5
        [HttpDelete("delete-crew")]
        public async Task<IActionResult> DeleteCrew([FromBody] CrewEditModel crew)
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
            crew.Status = "DELETED";
            var success = await _crewData.EditCrew(userId, crew);
            if (!success)
            {
                return StatusCode(500, "Failed to modify crew connection.");
            }

            return Ok("Crew connection deleted successfully.");
        }
    }
}
