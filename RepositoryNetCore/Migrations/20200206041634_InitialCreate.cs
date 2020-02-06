using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreSqliteDB.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Table1",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BatchId = table.Column<int>(nullable: false),
                    RowNo = table.Column<string>(nullable: true),
                    P3 = table.Column<string>(nullable: true),
                    P4 = table.Column<double>(nullable: false),
                    P5 = table.Column<double>(nullable: false),
                    P6 = table.Column<double>(nullable: false),
                    P7 = table.Column<DateTime>(nullable: false),
                    P8 = table.Column<DateTime>(nullable: false),
                    P9 = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table2",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BatchId = table.Column<int>(nullable: false),
                    RowNo = table.Column<string>(nullable: true),
                    P3 = table.Column<string>(nullable: true),
                    P4 = table.Column<double>(nullable: false),
                    P5 = table.Column<double>(nullable: false),
                    P6 = table.Column<double>(nullable: false),
                    P7 = table.Column<DateTime>(nullable: false),
                    P8 = table.Column<DateTime>(nullable: false),
                    P9 = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table3",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BatchId = table.Column<int>(nullable: false),
                    RowNo = table.Column<string>(nullable: true),
                    P3 = table.Column<string>(nullable: true),
                    P4 = table.Column<double>(nullable: false),
                    P5 = table.Column<double>(nullable: false),
                    P6 = table.Column<double>(nullable: false),
                    P7 = table.Column<DateTime>(nullable: false),
                    P8 = table.Column<DateTime>(nullable: false),
                    P9 = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table4",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BatchId = table.Column<int>(nullable: false),
                    RowNo = table.Column<string>(nullable: true),
                    P3 = table.Column<string>(nullable: true),
                    P4 = table.Column<double>(nullable: false),
                    P5 = table.Column<double>(nullable: false),
                    P6 = table.Column<double>(nullable: false),
                    P7 = table.Column<DateTime>(nullable: false),
                    P8 = table.Column<DateTime>(nullable: false),
                    P9 = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table4", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table5",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BatchId = table.Column<int>(nullable: false),
                    RowNo = table.Column<string>(nullable: true),
                    P3 = table.Column<string>(nullable: true),
                    P4 = table.Column<double>(nullable: false),
                    P5 = table.Column<double>(nullable: false),
                    P6 = table.Column<double>(nullable: false),
                    P7 = table.Column<DateTime>(nullable: false),
                    P8 = table.Column<DateTime>(nullable: false),
                    P9 = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table5", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Table1");

            migrationBuilder.DropTable(
                name: "Table2");

            migrationBuilder.DropTable(
                name: "Table3");

            migrationBuilder.DropTable(
                name: "Table4");

            migrationBuilder.DropTable(
                name: "Table5");
        }
    }
}
