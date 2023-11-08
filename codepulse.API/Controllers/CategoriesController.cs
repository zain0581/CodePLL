using codepulse.API.Data;
using codepulse.API.Modells.Domain;
using codepulse.API.Modells.DTO;
using codepulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace codepulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategory categoryrepo;
        public CategoriesController(ICategory category)
        {
            
            this.categoryrepo = category;
        }

        [HttpPost]
        public async Task<IActionResult> Createcategorie(CreateCategories request)

        {
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

            await categoryrepo.CreateAsync(category);


             var response = new CategoryDTO
             {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
             };

            return Ok(response);

        }

    }
}
