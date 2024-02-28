using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StargateAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                    //table.UniqueConstraint("Name", x => x.Name); //Uncomment if unique index implementation is not desired
                });

            migrationBuilder.CreateTable(
                name: "AstronautDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentRank = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentDutyTitle = table.Column<string>(type: "TEXT", nullable: false),
                    CareerStartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CareerEndDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AstronautDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AstronautDetail_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AstronautDuty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rank = table.Column<string>(type: "TEXT", nullable: false),
                    DutyTitle = table.Column<string>(type: "TEXT", nullable: false),
                    DutyStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DutyEndDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AstronautDuty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AstronautDuty_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            //This can be generated via the EF CLI in a new migration, for simplicity it is just added here
            migrationBuilder.CreateTable(
                name: "Error",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Error", x => x.Id);
                    //table.UniqueConstraint("Name", x => x.Name); //Uncomment if unique index implementation is not desired
                });

            migrationBuilder.CreateIndex(
                name: "IX_AstronautDetail_PersonId",
                table: "AstronautDetail",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AstronautDuty_PersonId",
                table: "AstronautDuty",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AstronautDetail");

            migrationBuilder.DropTable(
                name: "AstronautDuty");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
