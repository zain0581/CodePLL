using codepulse.API.Modells.DTO;
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

        public AuthController(UserManager<IdentityUser> userManager)
        {
            UserManager = userManager;
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
