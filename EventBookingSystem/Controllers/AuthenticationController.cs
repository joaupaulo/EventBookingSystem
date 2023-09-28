using System.IdentityModel.Tokens.Jwt;
   using System.Security.Claims;
   using System.Text;
   using EventBookingSystem.Configurations;
   using EventBookingSystem.Configurations.Identity;
   using EventBookingSystem.Model;
   using EventBookingSystem.Model.DTOs;
   using Microsoft.AspNetCore.Identity;
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.IdentityModel.Tokens;

   namespace EventBookingSystem.Controllers;

   [ApiController]
   [Route("/auth")]
   public class AuthenticationController : ControllerBase
   {
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly JwtConfig _jwtConfig;
   private readonly IConfiguration _configuration;

   public AuthenticationController(UserManager<ApplicationUser> userManager,
      IConfiguration configuration)
   {
      _userManager = userManager;
      _configuration = configuration;
   }

   [HttpPost("register")]
   public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto userRegistrationRequestDto)
   {
      if (ModelState.IsValid)
      {

         var verifyUser = await _userManager.FindByEmailAsync(userRegistrationRequestDto.Email);

         if (verifyUser != null)
         {
            return BadRequest(new AuthResult
            {
               Result = false,
               Errors = new List<string>()
               {
                  "Email already exist!"
               }
            });
         }

         ApplicationUser newUser = new()
         {
            UserName = userRegistrationRequestDto.Name,
            Email = userRegistrationRequestDto.Email
         };

         IdentityResult identityResult = await _userManager.CreateAsync(newUser, userRegistrationRequestDto.Password);

         if (identityResult.Succeeded)
         {
            var token = GenerateJwtToken(newUser);

            return Ok(new AuthResult()
            {
               Result = true,
               Token = token
            });

         }

         return BadRequest(new AuthResult
         {
            Result = false,
            Errors = new List<string>()
            {
               "Server Error"
            }
         });

      }

      return BadRequest();
   }
[HttpGet("/Login")]
   public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginRequestDto)
   {
      if (ModelState.IsValid)
      {
        
         var existing_user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

         if (existing_user == null)
         {
            return BadRequest(new AuthResult
            {
               Result = false,
               Errors = new List<string>()
               {
                  "Server Error"
               }
            });
         }
         
         var isCorrect = await _userManager.CheckPasswordAsync(existing_user, loginRequestDto.Senha);

         if (!isCorrect)
         {
            return BadRequest(new AuthResult
            {
               Result = false,
               Errors = new List<string>()
               {
                  "Error Credentials"
               }
            });
         }

         var jwtToken = GenerateJwtToken(existing_user);
         
         return Ok(new AuthResult()
         {
            Result = true,
            Token = jwtToken
         });  
      }
      return BadRequest(new AuthResult
      {
         Result = false,
         Errors = new List<string>()
         {
            "Error Credentials"
         }
      });  
   }
   private string GenerateJwtToken(ApplicationUser user)
   {
      var jwtTokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes( _configuration.GetSection("JwtConfig:Secret").Value);

      var tokenDescriptor = new SecurityTokenDescriptor()
      {
         Subject = new ClaimsIdentity(new[]
         {
            new Claim("Id", user.UserName),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
         }),
         Expires = DateTime.Now.AddDays(2),
         SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
      };
      var token = jwtTokenHandler.CreateToken(tokenDescriptor);
      var jwtToken = jwtTokenHandler.WriteToken(token);

      return jwtToken;
   }
}