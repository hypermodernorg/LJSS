using Microsoft.EntityFrameworkCore;
using LJSS.Models;

namespace LJSS.Data
{
    public class WordContext : DbContext
    {
        public WordContext(DbContextOptions<WordContext> options)
            : base(options)
        {
            
        }

        public DbSet<WordModel> WordModel { get; set; }
     
    }
}
