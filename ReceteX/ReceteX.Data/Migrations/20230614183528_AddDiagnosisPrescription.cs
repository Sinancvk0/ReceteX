using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceteX.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDiagnosisPrescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_Prescriptions_PrescriptionId",
                table: "Diagnoses");

            migrationBuilder.DropIndex(
                name: "IX_Diagnoses_PrescriptionId",
                table: "Diagnoses");

            migrationBuilder.DropColumn(
                name: "PrescriptionId",
                table: "Diagnoses");

            migrationBuilder.CreateTable(
                name: "DiagnosisPrescription",
                columns: table => new
                {
                    DiagnosesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrescriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagnosisPrescription", x => new { x.DiagnosesId, x.PrescriptionId });
                    table.ForeignKey(
                        name: "FK_DiagnosisPrescription_Diagnoses_DiagnosesId",
                        column: x => x.DiagnosesId,
                        principalTable: "Diagnoses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiagnosisPrescription_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosisPrescription_PrescriptionId",
                table: "DiagnosisPrescription",
                column: "PrescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiagnosisPrescription");

            migrationBuilder.AddColumn<Guid>(
                name: "PrescriptionId",
                table: "Diagnoses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_PrescriptionId",
                table: "Diagnoses",
                column: "PrescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_Prescriptions_PrescriptionId",
                table: "Diagnoses",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id");
        }
    }
}
