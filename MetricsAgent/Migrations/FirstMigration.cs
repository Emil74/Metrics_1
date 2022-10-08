using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgent.Migrations
{
    [Migration(0)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {
            Create.Table("cpumetrics").WithColumn("Id").AsInt64().PrimaryKey().Identity()
                            .WithColumn("Value").AsInt32()
                            .WithColumn("Time").AsInt64();
        }

        public override void Down()
        {
            Delete.Table("cpumetrics");
        }


    }
}
