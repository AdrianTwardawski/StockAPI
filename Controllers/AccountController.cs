using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Models;
using StockAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service;
        }
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _service.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _service.GenerateJwt(dto);

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true
            });
            return Ok(token);
        }

        [HttpGet("user")]
        public ActionResult LoggedUser()
        {
            var jwt = Request.Cookies["jwt"];
            var token = _service.Verify(jwt);
            var userId = int.Parse(token.Issuer);
            var user = _service.GetById(userId);
            return Ok(user);
        }

        [HttpPost("logout")]
        public ActionResult Loggout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new
            {
                message = "success"
            });
        }
    }
}
