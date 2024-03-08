using codepulse.API.Modells.DTO;
using codepulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace codepulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase

    {
        private readonly UserManager<IdentityUser> UserManager;
        private readonly ITokenRepo Tokenrepo;

        public AuthController(UserManager<IdentityUser> userManager , ITokenRepo tokenrepo)
        {
            UserManager = userManager;
            this.Tokenrepo = tokenrepo;
        }
        //Password for admin user 
        //Admin@123

        [HttpPost]
        [Route("login")]
       public async Task<IActionResult> login([FromBody]LoginRequestDto request)

       {
            //Check Email
            var identityuser = await UserManager.FindByEmailAsync(request.Email);

            if (identityuser is not null)
            {
                // cHECK pASSSWORD
               var cheackpassword = await UserManager.CheckPasswordAsync(identityuser,request.Password);


                if(cheackpassword )
                {
                    var roles = await UserManager.GetRolesAsync(identityuser);

                    //creat a tokken response
                    var jwttoken = Tokenrepo.CreateJwtToken(identityuser,roles.ToList());
                    var response = new LoginResponseDto()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwttoken
                    };
                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Email or password incoorect");
            return ValidationProblem(ModelState);

       }

       

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            //Create IdenttityUser object

            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };

            // Create user her
           var IdentityResult = await UserManager.CreateAsync(user, request.Password);

            if (IdentityResult.Succeeded)
            {
                //add role to user (reader)
                await UserManager.AddToRoleAsync(user, "Reader");

                if (IdentityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (IdentityResult.Errors.Any())
                    {
                        foreach (var error in IdentityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
               if (IdentityResult.Errors.Any())
               {
                    foreach (var errors in IdentityResult.Errors)
                    {
                        ModelState.AddModelError("", errors.Description);

                    }
               }
            }
            return ValidationProblem(ModelState);
        }
    }
}
