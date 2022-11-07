using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalRegistrationSystem.Infrastructure.Migrations;

public partial class InitialMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Clients",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clients", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Doctors",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                Specialty = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Doctors", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Appointments",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                VisitTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsVisited = table.Column<bool>(type: "bit", nullable: false),
                DoctorId = table.Column<int>(type: "int", nullable: false),
                ClientId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Appointments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Appointments_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Appointments_Doctors_DoctorId",
                    column: x => x.DoctorId,
                    principalTable: "Doctors",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Appointments_ClientId",
            table: "Appointments",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_Appointments_DoctorId",
            table: "Appointments",
            column: "DoctorId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Appointments");

        migrationBuilder.DropTable(
            name: "Clients");

        migrationBuilder.DropTable(
            name: "Doctors");
    }
}
