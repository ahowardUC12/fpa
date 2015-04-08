using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinancialPlannerApplication.Models.DataAccess
{
    public class FinancialPlannerDbContext : IdentityDbContext<ApplicationUser>
    {
        public FinancialPlannerDbContext()
            : base("FinancialPlannerConnection")
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public override IDbSet<ApplicationUser> Users { get; set; }
    }
}