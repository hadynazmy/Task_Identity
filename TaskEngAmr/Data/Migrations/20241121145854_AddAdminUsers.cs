using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskEngAmr.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        INSERT INTO [security].[Users] 
        ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfilePicture]) 
        VALUES 
        (N'34881e5e-31dc-44dc-bf68-f0ddea73d97f', 
         N'admin', 
         N'ADMIN', 
         N'admin@test.com', 
         N'ADMIN@TEST.COM', 
         0, 
         N'AQAAAAIAAYagAAAAEAlSBw/eKL0tEkyi4tCl/7tpZOkyGBmR7TCArUuYY6nHgp8NDmVqzYMfjLgQWtkXWA==', 
         N'C6ZLH34KCNGM77RDCZILQC46NHDEH3GI', 
         N'555bca89-0ca8-4700-ae60-8025c2be99e0', 
         NULL, 
         0, 
         0, 
         NULL, 
         1, 
         0, 
         N'hady', 
         N'nazmy', 
         CONVERT(varbinary(max), 0)); -- إدخال قيمة فارغة (Binary)
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[Users] WHERE Id = '34881e5e-31dc-44dc-bf68-f0ddea73d97f'");
        }

    }
}
