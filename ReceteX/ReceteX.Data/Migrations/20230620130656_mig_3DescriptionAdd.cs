using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceteX.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig_3DescriptionAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Descriptions_Prescriptions_PrescriptionId",
                table: "Descriptions");

            migrationBuilder.AlterColumn<Guid>(
                name: "PrescriptionId",
                table: "Descriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Descriptions_Prescriptions_PrescriptionId",
                table: "Descriptions",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Descriptions_Prescriptions_PrescriptionId",
                table: "Descriptions");

            migrationBuilder.AlterColumn<Guid>(
                name: "PrescriptionId",
                table: "Descriptions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Descriptions_Prescriptions_PrescriptionId",
                table: "Descriptions",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id");
        }
    }
}
