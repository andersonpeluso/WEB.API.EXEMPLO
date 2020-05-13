using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Modelo.API.Extensions;
using Modelo.API.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Modelo.API.Controllers
{
    [ApiController]
    [Route("api/identidade")]
    [Authorize]
    public class AuthController : BaseController
    {
        private readonly AppSettings _appSettings;

        public AuthController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        [HttpPost("nova-conta")]
        public ActionResult Registrar(UsuarioRegistro registro)
        {
            // é async

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            // Criar Usuario.
            //var result = Metodo();

            //if (result.Succeeded)
            //{
            //    return Ok();
            //}

            //foreach (var erro in AcceptedAtActionResult.Erros)
            //{
            //    AdicionarErroProcessamento(erro.Description);
            //}
            //return CustomResponse();

            return BadRequest();
        }


        [HttpPost]
        [Route("autenticar")]
        [AllowAnonymous]
        public ActionResult Login(UsuarioLogin login)
        {
            // é async

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            // Verifica usuario no banco de dados.

            //if (result.Succeeded)
            //{
            //    return CustomResponse(GeraJWT("nobody@gmail.com"));
            //}

            // return BadRequest();

            return CustomResponse(GerarToken("nobody@gmail.com"));

            //if (result.IsLockedOut)
            //{
            //    AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
            //    return CustomResponse();
            //}

            // AdicionarErroProcessamento("Usuário ou Senha incorretos");
            //    return CustomResponse();
        }

        private UsuarioLogin GerarToken(string email)
        {
            // Obter o usuario.
            // Verifica quais módulos tem acesso.
            // Verificar o nivel de acesso.

            var QuandoVaiExpirar = ToUnixEpochDate(DateTime.UtcNow).ToString();
            var QuandoFoiEmitido = ToUnixEpochDate(DateTime.UtcNow).ToString();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Segredo);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                // Subject = "",
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), algorithm: SecurityAlgorithms.HmacSha256Signature)
            });
            var encodedToken = tokenHandler.WriteToken(token);

            var response = new UsuarioLogin
            {
                // Pegar informações do usuário.


                Acesso = new Token
                {
                    AccessToken = encodedToken,
                    ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                }
            };
            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
         => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}