using Microsoft.EntityFrameworkCore;
using ZiraatBankDbParser.Models;

namespace ZiraatBankDbParser.Data
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {

        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<SettlementCategory> SettlementCategories { get; set; }      
    }
}
