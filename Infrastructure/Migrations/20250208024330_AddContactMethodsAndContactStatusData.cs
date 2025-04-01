using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddContactMethodsAndContactStatusData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                insert into  ContactStatuses select  'any';
                                insert into  ContactMethods select 'phone';
                                insert into ContactMethods select  'whatsapp';
                                ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                delete  ContactStatuses where ContactStatusId = 1;
                                delete  ContactMethods where ContactMethodId in (1 ,2);
                                ");

        }
    }
}
