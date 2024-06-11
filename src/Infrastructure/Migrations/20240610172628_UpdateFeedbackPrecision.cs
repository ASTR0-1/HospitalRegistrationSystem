using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalRegistrationSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFeedbackPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Feedbacks",
                type: "decimal(6,2)",
                precision: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Feedbacks",
                type: "decimal(5,2)",
                precision: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6);
        }
    }
}
