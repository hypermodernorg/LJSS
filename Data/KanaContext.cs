using Microsoft.EntityFrameworkCore;
using LJSS.Models;

namespace LJSS.Data
{
    public class KanaContext : DbContext
    {
        public KanaContext(DbContextOptions<KanaContext> options)
            : base(options)
        {
            
        }

        public DbSet<Kana> Kana { get; set; }
        public DbSet<KanaQuizH> KanaQuizH { get; set; }
        public DbSet<KanaQuizK> KanaQuizK { get; set; }

    }
}
