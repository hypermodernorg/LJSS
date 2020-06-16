using Microsoft.EntityFrameworkCore;
using LJSS.Models;

namespace LJSS.Data
{
    public class TranslateContext : DbContext
    {
        public TranslateContext(DbContextOptions<TranslateContext> options)
            : base(options)
        {
            
        }

        public DbSet<WordModelTrans> WordModelTrans { get; set; }
    }
}
