using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalRegistrationSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddServicePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HospitalFeePercent",
                table: "Hospitals",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VisitCost",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HospitalFeePercent",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "VisitCost",
                table: "AspNetUsers");
        }
    }
}
