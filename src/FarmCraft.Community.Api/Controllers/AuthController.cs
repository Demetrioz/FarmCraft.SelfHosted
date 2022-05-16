﻿using Akka.Actor;
using FarmCraft.Community.Api.Config;
using FarmCraft.Community.Data.DTOs;
using FarmCraft.Community.Data.DTOs.Requests;
using FarmCraft.Community.Data.Messages.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FarmCraft.Community.Api.Controllers
{
    [Route("authentication")]
    public class AuthController : FarmCraftController
    {
        public AuthController(IActorRef root, IOptions<AppSettings> settings) 
            : base(root, settings)
        {
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            FarmCraftActorResponse? response = await _rootActor.Ask(
                new AskToLogin(login.Username, login.Password),
                TimeSpan.FromSeconds(_defaultWait)
            ) as FarmCraftActorResponse;

            if (response == null)
                return ApiResponse.Failure("No response from server");
            else if (response.Status == ResponseStatus.Success && response.Data != null)
                return ApiResponse.Success(response.Data);
            else
                return ApiResponse.Unauthorized();
        }
    }
}
