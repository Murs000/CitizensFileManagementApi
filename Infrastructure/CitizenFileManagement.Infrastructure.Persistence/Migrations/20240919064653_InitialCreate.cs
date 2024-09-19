using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CitizenFileManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatorId = table.Column<int>(type: "integer", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifierId = table.Column<int>(type: "integer", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OTP = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    PasswordSalt = table.Column<string>(type: "text", nullable: false),
                    IsActivated = table.Column<bool>(type: "boolean", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    CreatorId = table.Column<int>(type: "integer", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifierId = table.Column<int>(type: "integer", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Users_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId1 = table.Column<int>(type: "integer", nullable: false),
                    CreatorId = table.Column<int>(type: "integer", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifierId = table.Column<int>(type: "integer", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Customers_CustomerId1",
                        column: x => x.CustomerId1,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Documents_Users_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatorId",
                table: "Customers",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ModifierId",
                table: "Customers",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CreatorId",
                table: "Documents",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CustomerId",
                table: "Documents",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CustomerId1",
                table: "Documents",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ModifierId",
                table: "Documents",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatorId",
                table: "Users",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CustomerId",
                table: "Users",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ModifierId",
                table: "Users",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_CreatorId",
                table: "Customers",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_ModifierId",
                table: "Customers",
                column: "ModifierId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_CreatorId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_ModifierId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
