﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskEngAmr.Data.Migrations
{
    /// <inheritdoc />
    public partial class seedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table:"Roles",
                columns: new[] {"Id" , "Name" , "NormalizedName" , "ConcurrencyStamp" } ,
                values: new object[] {Guid .NewGuid().ToString(), "User","User".ToUpper(), Guid.NewGuid().ToString()} ,
                schema: "security"
                );
            migrationBuilder.InsertData(
               table: "Roles",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { Guid.NewGuid().ToString(), "Admin", "Admin".ToUpper(), Guid.NewGuid().ToString() },
               schema: "security"
               );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ELETE FROM  [security].[Roles]");
        }
    }
}
