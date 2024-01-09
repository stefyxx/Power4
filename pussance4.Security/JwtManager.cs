using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pussance4.Security
{
    public class JwtManager(JwtSecurityTokenHandler _handler, JwtManager.JwtConfig _config)
    {
        public class JwtConfig
        {
            public required string Issuer { get; set; } // 1- who created tokens
            public string Audience { get; set; }        // 2- who the token is built for
            public required string Signature { get; set; }
            public required int DurationSeconds { get; set; }
        }
        public string CreateToken(string username, string identifier, string email, params string[] roles)
        {
            JwtSecurityToken t = new JwtSecurityToken(
                _config.Issuer,
                _config.Audience,
                CreatePayload(username, identifier, email, roles),
                DateTime.Now,
                (_config.DurationSeconds == 0) ? null : DateTime.Now.AddSeconds(_config.DurationSeconds),
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Signature)), SecurityAlgorithms.HmacSha256)

                );
            return _handler.WriteToken(t);
        }

        private IEnumerable<Claim> CreatePayload(string Username, string Identifier, string Email, string[] Roles)
        {
            // return new List<Claim> //oppure

            yield return new Claim(ClaimTypes.Name, Username);
            yield return new Claim(ClaimTypes.NameIdentifier, Identifier);
            yield return new Claim(ClaimTypes.Email, Email);
            //yield return new Claim(ClaimTypes.Role, Role);
            foreach (string role in Roles)
            {
                yield return new Claim(ClaimTypes.Role, role);
            }
        }
    }
}
