using codepulse.API.Modells.Domain;
using codepulse.API.Modells.DTO;
using codepulse.API.Repositories.Implementation;
using codepulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace codepulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepo blogPost;
        private readonly ICategory category;

        public BlogPostController(IBlogPostRepo irepo, ICategory categoryrepo)

        {
            this.blogPost = irepo;
            this.category = categoryrepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPost request)
        {


            // Convert dto to domain 
            var blogPostEntity = new BlogPost
            {
                Author = request.Author,
                UrlHandle = request.UrlHandle,
                Content = request.Content,
                Title = request.Title,
                IsVisible = request.IsVisible,
                FeatureImageUrl = request.FeatureImageUrl,
                ShortDescription = request.ShortDescription,
                PublishedDate = request.PublishedDate,
                categories = new List<Category>()
            };
            foreach (var categoriesGuid in request.Categories)
            {
                var excisting = await category.GetById(categoriesGuid);
                if (excisting is not null)
                {

                    blogPostEntity.categories.Add(excisting);
                }
                else
                {
                    return BadRequest();
                }

            }

            // Use your blogPost repository to create the blog post
            var createdBlogPost = await blogPost.CreateBlogPost(blogPostEntity);

            var response = new BlogPostDto
            {
                Id = createdBlogPost.Id,
                Author = createdBlogPost.Author,
                Content = createdBlogPost.Content,
                Title = createdBlogPost.Title,
                PublishedDate = createdBlogPost.PublishedDate,
                FeatureImageUrl = createdBlogPost.FeatureImageUrl,
                ShortDescription = createdBlogPost.ShortDescription,
                IsVisible = createdBlogPost.IsVisible,
                UrlHandle = createdBlogPost.UrlHandle,
                categories = blogPostEntity.categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,

                }).ToList()
            };

            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var Blogpos = await blogPost.GetAllAsync();

            var response = new List<BlogPostDto>();
            foreach (var blogs in Blogpos)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogs.Id,
                    Author = blogs.Author,
                    Content = blogs.Content,
                    Title = blogs.Title,
                    PublishedDate = blogs.PublishedDate,
                    FeatureImageUrl = blogs.FeatureImageUrl,
                    ShortDescription = blogs.ShortDescription,
                    IsVisible = blogs.IsVisible,
                    UrlHandle = blogs.UrlHandle,
                    categories = blogs.categories.Select(x => new CategoryDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle,

                    }).ToList()


                });
            }
            return Ok(response);

        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var blogs = await blogPost.GetById(id);
            if (blogs == null)
            {
                return BadRequest();
            }

            // convert domain to dto
            var response = new BlogPostDto
            {
                Id = blogs.Id,
                Author = blogs.Author,
                Content = blogs.Content,
                Title = blogs.Title,
                PublishedDate = blogs.PublishedDate,
                FeatureImageUrl = blogs.FeatureImageUrl,
                ShortDescription = blogs.ShortDescription,
                IsVisible = blogs.IsVisible,
                UrlHandle = blogs.UrlHandle,
                categories = blogs.categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }


        [HttpGet("Title")]
        public async Task<IActionResult> GetBlogByTitle(string Title)
        {

            var title = await blogPost.GetByTitle(Title);
            if (title == null)
            {
                return BadRequest();
            }
            var response = new BlogPostDto
            {
                Id = title.Id,
                Author = title.Author,
                Content = title.Content,
                Title = title.Title,
                PublishedDate = title.PublishedDate,
                FeatureImageUrl = title.FeatureImageUrl,
                ShortDescription = title.ShortDescription,
                IsVisible = title.IsVisible,
                UrlHandle = title.UrlHandle,
                categories = title.categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }


        [HttpGet]
        [Route("{UrlHandle}")]
        public async Task<IActionResult> GetBlogByUrl([FromRoute] string UrlHandle)
        {
           var blogpost =  await blogPost.GetByUrlHandle(UrlHandle);
            if (blogpost == null)
            {
                return BadRequest();
            }
            var resposne = new BlogPostDto
            {
                Id = blogpost.Id,
                Author = blogpost.Author,
                Content = blogpost.Content,
                Title = blogpost.Title,
                PublishedDate = blogpost.PublishedDate,
                FeatureImageUrl = blogpost.FeatureImageUrl,
                ShortDescription = blogpost.ShortDescription,
                IsVisible = blogpost.IsVisible,
                UrlHandle = blogpost.UrlHandle,
                categories = blogpost.categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,

                }).ToList()

            };
            return Ok(resposne);

        }


        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditBlogPost([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            //convert dto to Domain Nodel 
            var blogpost = new BlogPost
            {
                Id = id,
                Author = request.Author,
                Content = request.Content,
                Title = request.Title,
                PublishedDate = request.PublishedDate,
                FeatureImageUrl = request.FeatureImageUrl,
                ShortDescription = request.ShortDescription,
                IsVisible = request.IsVisible,
                UrlHandle = request.UrlHandle,
                categories = new List<Category>()
            };

            foreach (var cat in request.Categories)
            {
                var existing = await category.GetById(cat);
                if (existing != null)
                {
                    blogpost.categories.Add(existing);
                }
            }

            // calling the repository 
            var title = await blogPost.EditBlogPost(blogpost);
            if (title == null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = title.Id,
                Author = title.Author,
                Content = title.Content,
                Title = title.Title,
                PublishedDate = title.PublishedDate,
                FeatureImageUrl = title.FeatureImageUrl,
                ShortDescription = title.ShortDescription,
                IsVisible = title.IsVisible,
                UrlHandle = title.UrlHandle,
                categories = title.categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task <IActionResult>DeleteBlogpost([FromRoute] Guid id)
        {
            var cat = await blogPost.DeletBlogPostById(id);
            if (cat == null)
            {
                return BadRequest();
            }
            var response = new BlogPostDto
            {
                Title = cat.Title,
                PublishedDate = cat.PublishedDate,
                FeatureImageUrl = cat.FeatureImageUrl,
                ShortDescription = cat.ShortDescription,
                IsVisible = cat.IsVisible,
                UrlHandle = cat.UrlHandle,
                Author = cat.Author,
                Content = cat.Content,
                //categories = cat.categories.Select(x => new CategoryDTO
                //{
                //    Id = x.Id,
                //    Name = x.Name,
                //    UrlHandle = x.UrlHandle
                //}).ToList()

            };
            return( Ok(response));  
        }





    }
}




















