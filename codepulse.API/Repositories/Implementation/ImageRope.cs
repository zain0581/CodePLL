using codepulse.API.Data;
using codepulse.API.Modells.Domain;
using codepulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace codepulse.API.Repositories.Implementation
{
    public class ImageRope : IImageRepo
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly dbContext _dbContext;
        public ImageRope(IWebHostEnvironment webhostenviroment, IHttpContextAccessor httpContextAccessor, dbContext dbcontext)
        {
            _webHostEnvironment = webhostenviroment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbcontext;

        }



        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogimage)
        {
            //1_ Upload image tp api/Images
            var localpath = Path.Combine(_webHostEnvironment.ContentRootPath,"Images",$"{blogimage.FileNmae}{blogimage.FileExtension}");
            using var stream = new FileStream(localpath, FileMode.Create);
            await file.CopyToAsync(stream);

            //2_ Update the database
            //https://codepulse.com/images/somefilename.jpg
            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogimage.FileNmae}{blogimage.FileExtension}";


            blogimage.Url = urlPath;
            await _dbContext.BlogImages.AddAsync(blogimage);
            await _dbContext.SaveChangesAsync();
            return blogimage;
        }


        public async Task<IEnumerable<BlogImage>> GetAllImages()
        {
            return await _dbContext.BlogImages.ToListAsync();
        }


        public async Task<BlogImage?> GetById(Guid id)
        {
           return await _dbContext.BlogImages.FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<BlogImage?> GetByTitle(string Title)
        {
           return await _dbContext.BlogImages.FirstOrDefaultAsync(x=>x.Title==Title);   
        }
    }
    
}
