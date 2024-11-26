using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskEngAmr.Data.Migrations
{
    /// <inheritdoc />
    public partial class AssignAdminUserToAllRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("iNSERT INTO [security].[UserRoles] (UserId , RoleId) select '34881e5e-31dc-44dc-bf68-f0ddea73d97f' , Id From [security].[Roles]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from [security].[UserRoles] where UserId ='34881e5e-31dc-44dc-bf68-f0ddea73d97f'");
        }
    }
}
