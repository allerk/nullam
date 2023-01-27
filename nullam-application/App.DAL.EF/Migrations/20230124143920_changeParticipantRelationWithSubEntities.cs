using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class changeParticipantRelationWithSubEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Participants_EnterpriseId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_PersonId",
                table: "Participants");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_EnterpriseId",
                table: "Participants",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_PersonId",
                table: "Participants",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Participants_EnterpriseId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_PersonId",
                table: "Participants");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_EnterpriseId",
                table: "Participants",
                column: "EnterpriseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_PersonId",
                table: "Participants",
                column: "PersonId",
                unique: true);
        }
    }
}
