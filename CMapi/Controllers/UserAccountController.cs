using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin.Auth;
using FirebaseAdmin;
using CMLibrary.Models;
using CMLibrary.DataAccess;



namespace CMapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserAccountController : ControllerBase
{
    private readonly FirebaseApp _firebaseApp;
    private readonly IUserAccountData _userData;

    public UserAccountController(FirebaseApp firebaseApp, IUserAccountData userData)
    {
        _firebaseApp = firebaseApp;
        _userData = userData;
    }

    // GET: api/<UserAccountController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    [HttpPost("login-register")]
    public async Task<IActionResult> LoginRegister(string idToken)
    {
        UserFirebaseModel user = await CheckAccountStatus(idToken);
        if (user== null)
        {
            return Unauthorized();
        }
        var userAccount = await _userData.VerifyUserAccount(user);

        if (userAccount == null)
        {
            return Unauthorized();
        }

        return Ok(userAccount);

    }
    private async Task<UserFirebaseModel>? CheckAccountStatus(string idToken)
    {
        try
        {
            var firebaseAuth = FirebaseAuth.GetAuth(_firebaseApp);
            var decodedToken = await firebaseAuth.VerifyIdTokenAsync(idToken);

            var emailDecoded = decodedToken.Claims.TryGetValue("email", out var email) ? email : null;

            UserFirebaseModel user = new UserFirebaseModel
            {
                UserId = decodedToken.Uid,
                Email = emailDecoded.ToString()
            };

            return user;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

}
