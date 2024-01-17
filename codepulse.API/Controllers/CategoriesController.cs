using codepulse.API.Data;
using codepulse.API.Modells.Domain;
using codepulse.API.Modells.DTO;
using codepulse.API.Repositories.Implementation;
using codepulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var cat = await categoryrepo.GetAllAsync();

            var response = new List<CategoryDTO>();
            foreach (var category in cat)
            {
                response.Add(new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle,
                });
            }
            return Ok(response);
        }

        //Get Method BY id 
        [HttpGet]
        [Route("{id:Guid}")]
        //https://localhost:7074/api/Categories/355d2aad-9aae-4bcd-db1a-08dbdf90af7B
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory = await categoryrepo.GetById(id);
            if (existingCategory is null)
            {
                return NotFound();
            }
            var response = new CategoryDTO
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle,

            };

            return Ok(response);
        }

        // update category
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditCategory([FromRoute]Guid id, UpdateCategoryDto Request)
        {
            //convert dto to domain model
            var category = new Category
            {
                Id = id,
                Name = Request.Name,
                UrlHandle = Request.UrlHandle,

            };
            await categoryrepo.EditCategory(category);
            if(category == null)
            {
                return NotFound();
            }
            //convert Domain model to Dto
            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };
            return Ok(response);

        }


        [HttpGet("Name")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var exsistname = await categoryrepo.GetByName(name);
            if (exsistname is null)
            {

                return NotFound();
            }
          
            var response = new CategoryDTO
            {
                Id = exsistname.Id,
                Name = exsistname.Name,
                UrlHandle = exsistname.UrlHandle,
            };
            return Ok(response);
        }

        [HttpDelete]
        
        public async Task<IActionResult> DeletCetgoryById([FromRoute] Guid id)
        {
            var category =await categoryrepo.DeleteCategoryById(id);
            if(category is null)
            {
                return NotFound();
            }
            //convert Domain mode to dto 
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
