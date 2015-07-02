using FluentMigrator;
using Rabbit.UserInterface.Models;
using System.Data;
using MigrationBase = Rabbit.Components.Data.Migrators.MigrationBase;

namespace Rabbit.UserInterface
{
    [Migration(20150702180530)]
    public class Migrators : MigrationBase
    {
        #region Overrides of MigrationBase

        public override void Up()
        {
            Create
                .Table(TableName<AccountRecord>())
                .WithColumn("Id").AsAnsiString(32).PrimaryKey()
                .WithColumn("Account").AsString(20)
                .WithColumn("Password").AsString(50);

            Create
                .Table(TableName<UserRecord>())
                .WithColumn("Id").AsAnsiString(32).PrimaryKey()
                .WithColumn("Name").AsString(20)
                .WithColumn("Account_Id").AsAnsiString(32).ForeignKey(TableName<AccountRecord>(), "Id").OnDelete(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table(TableName("UserRecord"));
            Delete.Table(TableName("AccountRecord"));
        }

        #endregion Overrides of MigrationBase
    }
}