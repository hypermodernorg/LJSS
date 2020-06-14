using Microsoft.EntityFrameworkCore;
using LJSS.Models;

namespace LJSS.Data
{
    public class VocabularyContext : DbContext
    {
        public VocabularyContext(DbContextOptions<VocabularyContext> options)
            : base(options)
        {
            
        }

        public DbSet<WordModel> WordModel { get; set; }
     
    }
}
