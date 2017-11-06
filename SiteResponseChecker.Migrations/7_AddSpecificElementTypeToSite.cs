using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace SiteResponseChecker.Migrations
{
    [Migration(7)]
    public class _7_AddSpecificElementTypeToSite : Migration
    {
        public override void Up()
        {
            Alter.Table("Sites")
                 .AddColumn("SpecificElementType").AsString(50).Nullable();

            Update.Table("Sites").Set(new { SpecificElementType = "CssSelector" }).AllRows();

            Alter.Table("Sites")
                 .AlterColumn("SpecificElementType").AsString(50).NotNullable();

        }

        public override void Down()
        {
            Delete.Column("SpecificElementType").FromTable("Sites");
        }
    }
}
