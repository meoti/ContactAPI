using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContactAPI.Migrations
{
    public partial class initDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Expertise = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 256, nullable: false),
                    PasswordSalt = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    Address = table.Column<string>(maxLength: 100, nullable: false),
                    Mobile = table.Column<string>(unicode: false, maxLength: 14, nullable: false),
                    Zip = table.Column<string>(unicode: false, maxLength: 5, nullable: false),
                    Sex = table.Column<string>(unicode: false, maxLength: 1, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contact_User",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contact_Skills",
                columns: table => new
                {
                    Contact_Id = table.Column<Guid>(nullable: false),
                    Skill_id = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactSkill", x => new { x.Contact_Id, x.Skill_id });
                    table.ForeignKey(
                        name: "FK_ContactSkill_Contact",
                        column: x => x.Contact_Id,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactSkill_Skill",
                        column: x => x.Skill_id,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contact_Skills_Skill_id",
                table: "Contact_Skills",
                column: "Skill_id");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CreatedBy",
                table: "Contacts",
                column: "CreatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact_Skills");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
