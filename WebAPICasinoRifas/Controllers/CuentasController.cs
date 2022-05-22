﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPICasinoRifas.DTOs;

namespace WebAPICasinoRifas.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario)
        {
            var usuario = new IdentityUser { UserName = credencialesUsuario.Email, Email = credencialesUsuario.Email };
            var resultado = await userManager.CreateAsync(usuario, credencialesUsuario.Password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }

        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Email, credencialesUsuario.Password, isPersistent: false, lockoutOnFailure: false);

            if( resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest("Inicio de sesion incorrecto");
            }

        }

        [HttpGet("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaAutenticacion>> Renovar()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var credencialesUsuario = new CredencialesUsuario()
            {
                Email = email
            };
            return await ConstruirToken(credencialesUsuario);
        }

        private async  Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuario.Email)
            };
            var usuario = await userManager.FindByEmailAsync(credencialesUsuario.Email);
            var claimsDb = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDb);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddMinutes(15);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdministrador")]
        [HttpPost("HacerAdministrador")]
        public async Task<ActionResult> HacerAdmin(EditarAdministradorDTO editarAdministradorDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdministradorDTO.Email);
            await userManager.AddClaimAsync(usuario, new Claim("esAdministrador", "1"));
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdministrador")]
        [HttpPost("QuitarAdministrador")]
        public async Task<ActionResult> QuitarAdmin(EditarAdministradorDTO editarAdministradorDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdministradorDTO.Email);
            await userManager.RemoveClaimAsync(usuario, new Claim("esAdministrador", "1"));
            return NoContent();
        }

    }
}
