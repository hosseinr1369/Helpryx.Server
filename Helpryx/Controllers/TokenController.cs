using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        [HttpGet("get-token")]
        public IActionResult GetToken([FromHeader] string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey) || apiKey != "kmr5VohCNYeLHaZTntlOy26P9jgFqjlTyn19Se9eu+I=")
            {
                return Unauthorized(new { message = "Invalid API Key...." });
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("8ieHTsEqkQIwtSX81hMLY4Q6jNfjP50O01/5iPtqJNo="));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials);

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = tokenStr });
        }
    }
}
