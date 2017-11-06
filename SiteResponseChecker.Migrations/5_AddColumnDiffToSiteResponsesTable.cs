using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace SiteResponseChecker.Migrations
{
    [Migration(5)]
    public class AddColumnDiffToSiteResponsesTable : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("SiteResponses")
                 .AddColumn("Diff").AsString(4096).Nullable();
        }
    }
}
