using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KevinBlogApi.Web.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace KevinBlogApi.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IConfiguration _config;
        private UserManager<IdentityUser> _userManager;
        private ILogger<UsersController> _logger;
        public UsersController(UserManager<IdentityUser> userManager,
            IConfiguration config,
            ILogger<UsersController> logger)
        {
            _config = config;
            _userManager = userManager;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        //[HttpPost]
        //public async Task<ActionResult<string>> Login([FromBody] LoginViewModel value)
        //{
        //    var u = await _userManager.FindByNameAsync(value.UserName);
        //    if(u != null)
        //    {
        //        var lu = await _userManager.CheckPasswordAsync(u, value.Password);
        //        if (lu)
        //        {
        //            await _signInManager.SignInAsync(u, true);
        //            return Ok(new { result = true, Msg = "" });
        //        }
        //        else
        //        {
        //            return Ok(new { result = false, Msg = "密码错误"});
        //        }
        //    }
        //    else
        //    {
        //        return Ok(new { result = false, Msg = "用户不存在" });
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult<string>> Login([FromBody] LoginViewModel value)
        {
            try
            {
                var u = await _userManager.FindByNameAsync(value.UserName);
                if (u != null)
                {
                    var lu = await _userManager.CheckPasswordAsync(u, value.Password);
                    if (lu)
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.UTF8.GetBytes(_config.GetSection("AppSettings")["Key"]);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.Name, u.Id.ToString())
                            }),
                            Expires = DateTime.Now.AddHours(1),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var tokenstr = tokenHandler.WriteToken(token);
                        return Ok(new { result = true, Msg = tokenstr });
                    }
                    else
                    {
                        return Ok(new { result = false, Msg = "密码错误" });
                    }
                }
                else
                {
                    return Ok(new { result = false, Msg = "用户不存在" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new { result = false, Msg = "Internal error" });
            }
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<ActionResult<string>> Register([FromBody] RegisterViewModel value)
        {
            try
            {
                var u = new IdentityUser()
                {
                    UserName = value.UserName
                };
                var lu = await _userManager.CreateAsync(u, value.Password);
                return Ok(new { result = lu.Succeeded, Msg = "" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new { result = "false", Msg = "Interal error" });
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
