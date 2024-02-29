using codepulse.API.Modells.Domain;
using codepulse.API.Modells.DTO;
using codepulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace codepulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private IImageRepo Imagerepo;
        public ImagesController(IImageRepo imagerepo)
        {
            Imagerepo = imagerepo;
            // Constructor; you can add any initialization logic here if needed.
        }

      

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            // Validate the uploaded file.
            validateFileUpload(file);

            if (ModelState.IsValid)
            {
                // If the ModelState is valid, proceed with file upload and other logic.

                // Create a BlogImage object to store information about the uploaded image.
                var blogimage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileNmae = fileName, // Note: There's a typo here, should be "FileName" instead of "FileNmae".
                    Title = title,
                    DateCreated = DateTime.Now
                };
                await Imagerepo.Upload(file, blogimage);
                //convert domain mode to dto 
                var response = new BlogImageDto
                {
                    Id = blogimage.Id,
                    Title = blogimage.Title,
                    DateCreated = blogimage.DateCreated,
                    FileExtension = blogimage.FileExtension,
                    FileNmae = blogimage.FileNmae,
                    Url = blogimage.Url,

                };
                return Ok(blogimage);
            }
            return BadRequest(ModelState);
        }


        // the validation for Image 
        private void validateFileUpload(IFormFile file)
        {
            // Define allowed file extensions.
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            // Check if the uploaded file's extension is allowed.
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            
            // Check if the file size is within the allowed limit (10MB).
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }

            // Check if the file is not empty.
            if (file.Length == 0)
            {
                ModelState.AddModelError("file", "The uploaded file is empty.");
            }
        }

        // Get method for alll Pic 
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await Imagerepo.GetAllImages();
            var resposne = new List<BlogImageDto>();
            foreach (var image in images)
            {
                resposne.Add(new BlogImageDto
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = DateTime.Now,
                    FileExtension = image.FileExtension,
                    FileNmae = image.FileNmae,
                    Url = image.Url,

                });

            }
            return Ok(resposne);    
           
        }




        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetImagesById(Guid id)
        {
           var IMG =  await Imagerepo.GetById(id);
            if (IMG == null)
            {
                return BadRequest();
            }
            var response = new BlogImageDto 
            {
                Id = IMG.Id,
                Title = IMG.Title,
                DateCreated = DateTime.Now,
                FileExtension = IMG.FileExtension,
                Url = IMG.Url,
                FileNmae= IMG.FileNmae,

            };
            return Ok(response);
        }

        [HttpGet]
        [Route("{Title}")]
        public async Task<IActionResult> GetImagebyTitle(string Title)
        {
            var img= await Imagerepo.GetByTitle(Title);
            if (img == null)
            {
                return BadRequest();
            }
            var response = new BlogImageDto 
            { 
                Id = img.Id, 
                FileExtension= img.FileExtension,
                Url = img.Url,
                FileNmae = img.FileNmae,
                DateCreated= DateTime.Now,
                Title = img.Title 
            };
            return Ok(response);


        }

    }
}


