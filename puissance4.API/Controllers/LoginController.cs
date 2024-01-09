using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using puissance4.API.Models;
using pussance4.Security;

namespace puissance4.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]

    public class LoginController : ControllerBase
    {
        private readonly JwtManager _jwtManager;
        public LoginController(JwtManager jwtManager)
        {
            _jwtManager = jwtManager;
        }


        [HttpGet("api/login")]
        public IActionResult Login([FromBody]LoginDTO dto)
        {
            //Non ho DB, x semplificare ho detto che tutte le pws saranno "1234"
            if (dto.password != "1234") { return Unauthorized(); }
            return Ok(
                new
                {
                    //cosi' quello che recupero nello status é solo l'username
                    Token = _jwtManager.CreateToken(dto.username, dto.username, dto.username),
                });

            //TODO controllare in DB che pws esiste e se si, se é al corrispondente User
            //--> v altro project (BLL con SecurotyServices che ha method 'login' che importa repository di DAL e controlla che ci sia

        }
    }
}
