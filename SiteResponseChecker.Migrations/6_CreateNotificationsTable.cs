using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace SiteResponseChecker.Migrations
{
    [Migration(6)]
    public class CreateNotificationsTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Notifications")
                  .WithColumn("NotificationsId").AsInt32().NotNullable().PrimaryKey().Identity().Indexed()
                  .WithColumn("SiteId").AsInt32().NotNullable().ForeignKey("Sites", "SiteId").Indexed()
                  .WithColumn("NotificationDate").AsDateTime().NotNullable()
                  .WithColumn("IsSent").AsBoolean().NotNullable()
                  .WithColumn("SendDate").AsDateTime().Nullable()
                  .WithColumn("SendError").AsString(4096).Nullable()
                  .WithColumn("Subject").AsString(512).NotNullable()
                  .WithColumn("Contents").AsString(4096).NotNullable();
        }
    }
}
