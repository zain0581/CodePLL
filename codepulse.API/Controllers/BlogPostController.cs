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
        public BlogPostController(IBlogPostRepo irepo)

        {
           this.blogPost = irepo;
                
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
                };

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
                   Id=blogs.Id, 
                   Author = blogs.Author,   
                   Content = blogs.Content, 
                   Title = blogs.Title, 
                   PublishedDate = blogs.PublishedDate, 
                   FeatureImageUrl = blogs.FeatureImageUrl, 
                   ShortDescription = blogs.ShortDescription,   
                   IsVisible = blogs.IsVisible, 
                   UrlHandle = blogs.UrlHandle, 
                });
            }
            return Ok(response);

        }

    }
}
