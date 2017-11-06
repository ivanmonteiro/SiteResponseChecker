using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace SiteResponseChecker.Migrations
{
    [Migration(1)]
    public class CreateUsersTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("UserId").AsInt32().NotNullable().PrimaryKey().Identity().Indexed()
                .WithColumn("UserName").AsString(20).NotNullable()
                .WithColumn("Password").AsString(20).NotNullable()
                .WithColumn("CreateDate").AsDateTime().NotNullable()
                .WithColumn("Email").AsString(50).NotNullable()
                .WithColumn("SnapshotInterval").AsInt32().NotNullable()
                .WithColumn("LastSnapshotDate").AsDateTime().Nullable();

            Insert.IntoTable("Users").Row(
                new
                    {
                        UserName = "admin",
                        Password = "123456",
                        CreateDate = DateTime.Now,
                        Email = "test@testdomain.com",
                        SnapshotInterval = 7
                    });

        }
    }
}
