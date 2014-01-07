using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace UmbracoExtension.FM
{
    public static class DataBaseParm
    {
        public const string scriptPath = @"..\..\..\..\..\..\Database\";
    }


    [Migration(2014010701)]
    public class FMCore2014010601 : Migration
    {

        public override void Up()
        {
            Execute.Script(DataBaseParm.scriptPath + "CreateLog.sql");
        }

        public override void Down()
        {

        }
    }
}
