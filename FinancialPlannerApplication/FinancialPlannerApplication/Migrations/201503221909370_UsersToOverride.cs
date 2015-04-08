namespace FinancialPlannerApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersToOverride : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.AspNetUsers", "Id", "dbo.IdentityUsers");
            //DropForeignKey("dbo.AspNetUserClaims", "IdentityUser_Id", "dbo.IdentityUsers");
            //DropForeignKey("dbo.AspNetUserLogins", "IdentityUser_Id", "dbo.IdentityUsers");
            //DropForeignKey("dbo.AspNetUserRoles", "IdentityUser_Id", "dbo.IdentityUsers");
            //DropIndex("dbo.AspNetUserRoles", new[] { "IdentityUser_Id" });
            //DropIndex("dbo.IdentityUsers", "UserNameIndex");
            //DropIndex("dbo.AspNetUserClaims", new[] { "IdentityUser_Id" });
            //DropIndex("dbo.AspNetUserLogins", new[] { "IdentityUser_Id" });
            //DropIndex("dbo.AspNetUsers", new[] { "Id" });
            //DropColumn("dbo.AspNetUserRoles", "UserId");
            //DropColumn("dbo.AspNetUserClaims", "UserId");
            //DropColumn("dbo.AspNetUserLogins", "UserId");
            //RenameColumn(table: "dbo.AspNetUserClaims", name: "IdentityUser_Id", newName: "UserId");
            //RenameColumn(table: "dbo.AspNetUserLogins", name: "IdentityUser_Id", newName: "UserId");
            //RenameColumn(table: "dbo.AspNetUserRoles", name: "IdentityUser_Id", newName: "UserId");
            //DropPrimaryKey("dbo.AspNetUserRoles");
            //DropPrimaryKey("dbo.AspNetUserLogins");
            //AddColumn("dbo.AspNetUsers", "Email", c => c.String(maxLength: 256));
            //AddColumn("dbo.AspNetUsers", "EmailConfirmed", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AspNetUsers", "PasswordHash", c => c.String());
            //AddColumn("dbo.AspNetUsers", "SecurityStamp", c => c.String());
            //AddColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String());
            //AddColumn("dbo.AspNetUsers", "PhoneNumberConfirmed", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AspNetUsers", "TwoFactorEnabled", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AspNetUsers", "LockoutEndDateUtc", c => c.DateTime());
            //AddColumn("dbo.AspNetUsers", "LockoutEnabled", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AspNetUsers", "AccessFailedCount", c => c.Int(nullable: false));
            //AddColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false, maxLength: 256));
            //AlterColumn("dbo.AspNetUserRoles", "UserId", c => c.String(nullable: false, maxLength: 128));
            //AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.String(nullable: false, maxLength: 128));
            //AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.String(nullable: false, maxLength: 128));
            //AlterColumn("dbo.AspNetUserLogins", "UserId", c => c.String(nullable: false, maxLength: 128));
            //AddPrimaryKey("dbo.AspNetUserRoles", new[] { "UserId", "RoleId" });
            //AddPrimaryKey("dbo.AspNetUserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });
            //CreateIndex("dbo.AspNetUserRoles", "UserId");
            //CreateIndex("dbo.AspNetUsers", "UserName", unique: true, name: "UserNameIndex");
            //CreateIndex("dbo.AspNetUserClaims", "UserId");
            //CreateIndex("dbo.AspNetUserLogins", "UserId");
            //AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            //DropTable("dbo.IdentityUsers");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.IdentityUsers",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Email = c.String(maxLength: 256),
            //            EmailConfirmed = c.Boolean(nullable: false),
            //            PasswordHash = c.String(),
            //            SecurityStamp = c.String(),
            //            PhoneNumber = c.String(),
            //            PhoneNumberConfirmed = c.Boolean(nullable: false),
            //            TwoFactorEnabled = c.Boolean(nullable: false),
            //            LockoutEndDateUtc = c.DateTime(),
            //            LockoutEnabled = c.Boolean(nullable: false),
            //            AccessFailedCount = c.Int(nullable: false),
            //            UserName = c.String(nullable: false, maxLength: 256),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            //DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            //DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            //DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            //DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            //DropIndex("dbo.AspNetUsers", "UserNameIndex");
            //DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            //DropPrimaryKey("dbo.AspNetUserLogins");
            //DropPrimaryKey("dbo.AspNetUserRoles");
            //AlterColumn("dbo.AspNetUserLogins", "UserId", c => c.String(maxLength: 128));
            //AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.String(maxLength: 128));
            //AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.String());
            //AlterColumn("dbo.AspNetUserRoles", "UserId", c => c.String(maxLength: 128));
            //DropColumn("dbo.AspNetUsers", "UserName");
            //DropColumn("dbo.AspNetUsers", "AccessFailedCount");
            //DropColumn("dbo.AspNetUsers", "LockoutEnabled");
            //DropColumn("dbo.AspNetUsers", "LockoutEndDateUtc");
            //DropColumn("dbo.AspNetUsers", "TwoFactorEnabled");
            //DropColumn("dbo.AspNetUsers", "PhoneNumberConfirmed");
            //DropColumn("dbo.AspNetUsers", "PhoneNumber");
            //DropColumn("dbo.AspNetUsers", "SecurityStamp");
            //DropColumn("dbo.AspNetUsers", "PasswordHash");
            //DropColumn("dbo.AspNetUsers", "EmailConfirmed");
            //DropColumn("dbo.AspNetUsers", "Email");
            //AddPrimaryKey("dbo.AspNetUserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });
            //AddPrimaryKey("dbo.AspNetUserRoles", new[] { "UserId", "RoleId" });
            //RenameColumn(table: "dbo.AspNetUserRoles", name: "UserId", newName: "IdentityUser_Id");
            //RenameColumn(table: "dbo.AspNetUserLogins", name: "UserId", newName: "IdentityUser_Id");
            //RenameColumn(table: "dbo.AspNetUserClaims", name: "UserId", newName: "IdentityUser_Id");
            //AddColumn("dbo.AspNetUserLogins", "UserId", c => c.String(nullable: false, maxLength: 128));
            //AddColumn("dbo.AspNetUserClaims", "UserId", c => c.String());
            //AddColumn("dbo.AspNetUserRoles", "UserId", c => c.String(nullable: false, maxLength: 128));
            //CreateIndex("dbo.AspNetUsers", "Id");
            //CreateIndex("dbo.AspNetUserLogins", "IdentityUser_Id");
            //CreateIndex("dbo.AspNetUserClaims", "IdentityUser_Id");
            //CreateIndex("dbo.IdentityUsers", "UserName", unique: true, name: "UserNameIndex");
            //CreateIndex("dbo.AspNetUserRoles", "IdentityUser_Id");
            //AddForeignKey("dbo.AspNetUserRoles", "IdentityUser_Id", "dbo.IdentityUsers", "Id");
            //AddForeignKey("dbo.AspNetUserLogins", "IdentityUser_Id", "dbo.IdentityUsers", "Id");
            //AddForeignKey("dbo.AspNetUserClaims", "IdentityUser_Id", "dbo.IdentityUsers", "Id");
            //AddForeignKey("dbo.AspNetUsers", "Id", "dbo.IdentityUsers", "Id");
        }
    }
}
