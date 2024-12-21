using CMLibrary.DataAccess;
using CMLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<CrewController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<CrewController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        
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
