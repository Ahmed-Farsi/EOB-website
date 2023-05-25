using Eob_Web.Core;
using Eob_Web.Core.Models;
using Eob_Web.Core.Services;
using Eob_Web.Frontend.Areas.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Eob_Web.Frontend.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private IEob_Service _eob_Service;
        private IUser_Service _user_Service;
        private IConfiguration _configuration;
        private IHttpContextAccessor _http_Context_Accessor;
        private UserManager<ApplicationUser> _user_Manager;

        private string _key;

        private protected class ReturnMessage
        {
            public string status { get; set; } = "failed";
            public string message { get; set; } = "unkown";
        }

        private class Auth_Result
        {
            public User User { get; set; }
            public string Token { get; set; }
        }

        public class Login
        {
            public string Email_Address { get; set; }
            public string Password { get; set; }
        }

        public AuthorizationController(
            IEob_Service eob_Service,
            IUser_Service user_Service,
            IConfiguration configuration,
            IHttpContextAccessor http_Context_Accessor,
            UserManager<ApplicationUser> user_Manager)
        {
            _eob_Service = eob_Service;
            _user_Service = user_Service;
            _configuration = configuration;
            _user_Manager = user_Manager;
            _http_Context_Accessor = http_Context_Accessor;

            _key = _configuration.GetValue<string>($"JWT:SigningKey");
        }

        // Identity authentication
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Authenticate(Login login)
        {
            var application_User = await _user_Manager.FindByEmailAsync(login.Email_Address);
            if (application_User == null)
                return Unauthorized();

            if (!await _user_Manager.CheckPasswordAsync(application_User, login.Password))
                return Unauthorized();

            var user = await _user_Service.Get_By_Id(application_User.UserId);
            if (user.Role_Name == User_Roles.New_User || !user.Verified)
                return Forbid();

            if (!user.Company_Id.HasValue && user.Role_Name != User_Roles.Admin)
                return BadRequest(new { Error = "User has no company ID" });

            var token = Create_Token(
                user.Email_Address,
                user.Role_Name,
                user.Id,
                eob_Id: 0,
                DateTime.UtcNow.AddDays(7));

            return Ok(new Auth_Result { User = user, Token = token });
        }

        // Token (remember login) authentication
        // Only requires the token to be in the header
        [HttpGet("get_user")]
        public async Task<ActionResult> Get_User()
        {
            int user_Id;
            bool res = int.TryParse(_http_Context_Accessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "User_Id").Value, out user_Id);

            var user = await _user_Service.Get_By_Id(user_Id);

            if (user.Role_Name == User_Roles.New_User)
                return Forbid();

            if (!user.Company_Id.HasValue && user.Role_Name != User_Roles.Admin)
                return BadRequest(new { Error = "User has no company ID" });

            return Ok(user);
        }

        // EOB authentication
        [AllowAnonymous]
        [HttpGet("eob/{key}")]
        public async Task<ActionResult> Eob(string key)
        {
            var eob = await _eob_Service.Get_By_Node_Id(key);
            if (eob == null)
                return Unauthorized();

            var token = Create_Token(
                eob.Serial_Number,
                User_Roles.Eob,
                user_Id: 0,
                eob.Id,
                DateTime.UtcNow.AddDays(7));

            return Ok(new ReturnMessage
            {
                status = "success",
                message = token
            });
        }

        private string Create_Token(
            string name,
            string user_Role,
            int user_Id,
            int eob_Id,
            DateTime expires)
        {
            var token_Handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);
            var token_Descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                (
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, name),
                        new Claim(ClaimTypes.Role, user_Role),
                        new Claim("User_Id", user_Id.ToString()),
                        new Claim("Eob_Id", eob_Id.ToString()),
                    }
                ),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = token_Handler.CreateToken(token_Descriptor);
            return token_Handler.WriteToken(token);
        }
    }
}
