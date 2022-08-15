﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)
        {
            var user = _userRepo.Authenticate(model.UserName, model.Password);
            if(user==null)
            {
                return BadRequest(new { message = "Username or Password is incorrect" });
            }
            return Ok(user);
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] AuthenticationModel model)
        {
            bool ifUserNameExists = _userRepo.IsUniqueUser(model.UserName);
            if(!ifUserNameExists)
            {
                return BadRequest(new { message = "UserName already exists!" });
            }
            var user = _userRepo.Register(model.UserName, model.Password);
            if(user == null)
            {
                return BadRequest(new { message = "Error while registering" });
            }
            return Ok();
        }
    }
}
