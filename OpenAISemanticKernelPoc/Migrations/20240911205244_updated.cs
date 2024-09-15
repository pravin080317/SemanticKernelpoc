using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAISemanticKernelPoc.Migrations
{
    /// <inheritdoc />
    public partial class updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PID = table.Column<long>(type: "bigint", nullable: false),
                    Varietyy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VID = table.Column<long>(type: "bigint", nullable: true),
                    Rate = table.Column<int>(type: "int", nullable: false),
                    Preview = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End = table.Column<DateTime>(type: "datetime2", nullable: true),
                    perma = table.Column<bool>(type: "bit", nullable: false),
                    Last = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpBY = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CrBY = table.Column<int>(type: "int", nullable: false),
                    regulat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProdG = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Varietys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Varietyy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Varietys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Varietys_products_productId",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Varietys_productId",
                table: "Varietys",
                column: "productId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exports");

            migrationBuilder.DropTable(
                name: "Varietys");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
