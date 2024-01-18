using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Individual.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    STFSTUDID = table.Column<long>(type: "bigint", nullable: false),
                    STFSTUDPICPATH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STFSTUDLNAME = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    STFSTUDFNAME = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    STFSTUDMNAME = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    STFSTUDCOURSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    STFSTUDYEAR = table.Column<int>(type: "int", nullable: false),
                    STFSTUDREMARKS = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    STFSTUDSTATUS = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.STFSTUDID);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    SUBJCODE = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    SUBJCOURSECODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SUBJPREQ = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    SUBJCODEPREQ = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    SUBJCATEGORY = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    SUBJDESC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SUBJUNITS = table.Column<int>(type: "int", nullable: false),
                    SUBJREGOFRNG = table.Column<int>(type: "int", nullable: false),
                    SUBJSTATUS = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    SUBJCURRCODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => new { x.SUBJCODE, x.SUBJCOURSECODE, x.SUBJCATEGORY, x.SUBJCODEPREQ });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}
