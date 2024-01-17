using codepulse.API.Data;
using codepulse.API.Modells.Domain;
using codepulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace codepulse.API.Repositories.Implementation
{
    public class BlogPostRepo : IBlogPostRepo
    {
        private readonly dbContext dbContext;
        public BlogPostRepo(dbContext dbContext)
        {
            this.dbContext = dbContext; 
                
        }

        public async Task<BlogPost> CreateBlogPost(BlogPost post)
        {
           await dbContext.BlogPosts.AddAsync(post);
            await dbContext.SaveChangesAsync(); 
            return post;
        }

       

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {

          return  await dbContext.BlogPosts.Include(x=> x.categories).ToListAsync();
            
        }

        public async Task<BlogPost?> GetById(Guid id)
        {
            return await dbContext.BlogPosts.Include(x => x.categories).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<BlogPost?> GetByTitle(string title)
        {
            return await dbContext.BlogPosts.Include(x => x.categories).FirstOrDefaultAsync(x => x.Title == title);
        }

        public async Task<BlogPost?> EditBlogPost(BlogPost post)
        {
          var cat = await dbContext.BlogPosts.Include(x=>x.categories). FirstOrDefaultAsync(x=>x.Id==post.Id);   
            if(cat!=null)
            {
                //update blogpost here
                dbContext.Entry(cat).CurrentValues.SetValues(post);
                // Update categories her
                cat.categories = post.categories;
                // Save changes in db
                await dbContext.SaveChangesAsync();
                return post;    
            }
            return null;
        }

        public async Task<BlogPost?> DeletBlogPostById(Guid id)
        {
          var cat = await dbContext.BlogPosts.FirstOrDefaultAsync(x=>x.Id==id);
            if(cat!=null)
            {

                dbContext.BlogPosts.Remove(cat);
                
                    //cat.categories = cat.categories;

                await dbContext.SaveChangesAsync();
                return cat;

            }
            return null;
        }
    }
}
