// <auto-generated />

using System.CodeDom.Compiler;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace FinancialPlannerApplication.Migrations
{
    [GeneratedCode("EntityFramework.Migrations", "6.1.1-30610")]
    public sealed partial class AddBalanceToBudgetItemAndRemoveIsBudgetTransactionFromTransaction : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddBalanceToBudgetItemAndRemoveIsBudgetTransactionFromTransaction));
        
        string IMigrationMetadata.Id
        {
            get { return "201501252051307_AddBalanceToBudgetItemAndRemoveIsBudgetTransactionFromTransaction"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
