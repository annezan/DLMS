using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

namespace API_DLMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationJwtController
    {
        // Variables locales.
        JwtSecurityTokenHandler tokenHandler;
        SecurityTokenDescriptor tokenDescriptor;
        SecurityToken token;
        byte[] key;
        [HttpGet]
        public string AuthentificationJwt()
        {
            tokenHandler = new JwtSecurityTokenHandler();
            key = Encoding.ASCII.GetBytes(GlobalVariable.Cle);
            tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "ASC_CONTROLE_FACTURE")
            }),
                
                Expires = DateTime.MaxValue,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenstring= tokenHandler.WriteToken(token);
            return tokenstring;
        }

    }
}
