﻿using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin.Auth;
using FirebaseAdmin;
using CMLibrary.Models;
using CMLibrary.DataAccess;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace CMapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserAccountController : ControllerBase
{
    private readonly IUserAccountData _userData;

    public UserAccountController(IUserAccountData userData)
    {
        _userData = userData;
    }

    [HttpPost("login-register")]
    public async Task<IActionResult> LoginRegister()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        UserFirebaseModel user = new UserFirebaseModel
        {
            UserId = userId,
            Email = email
        };
        
        if (user == null || userId == null )
        {
            return Unauthorized();
        }
        var userAccount = await _userData.VerifyUserAccount(user)!;

        if (userAccount == null)
        {
            return Unauthorized();
        }

        return Ok(userAccount);
    }

}
