﻿using codepulse.API.Modells.Domain;
using codepulse.API.Modells.DTO;
using codepulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}

