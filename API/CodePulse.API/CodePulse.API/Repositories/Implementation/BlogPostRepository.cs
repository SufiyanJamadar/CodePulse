using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbcontext;

        public BlogPostRepository(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
           await dbcontext.BlogPosts.AddAsync(blogPost);
            await dbcontext.SaveChangesAsync();
            return blogPost;
        }

        public async  Task<BlogPost?> DeleteAsync(Guid id)
        {
          var existingBlogPost=await dbcontext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if(existingBlogPost != null)
            {
                dbcontext.BlogPosts.Remove(existingBlogPost);
                await dbcontext.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
          return await dbcontext.BlogPosts.Include(x=>x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
          return await  dbcontext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await dbcontext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
          var existingBlogPost=await dbcontext.BlogPosts.Include(x=>x.Categories)
                .FirstOrDefaultAsync(x=>x.Id == blogPost.Id);

              if(existingBlogPost is null)
              {
                return null;
              }
              
              // Update BlogPost
              dbcontext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

              // Update Categories
               existingBlogPost.Categories=blogPost.Categories;

               await dbcontext.SaveChangesAsync();

            return blogPost;
        }
    }
}
