using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace isgasoir.Migrations
{
    /// <inheritdoc />
    public partial class AddActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chapitres_modules_ModuleId",
                table: "chapitres");

            migrationBuilder.DropForeignKey(
                name: "FK_modules_semestrees_SemId",
                table: "modules");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleStudant_modules_ModulesId",
                table: "ModuleStudant");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleStudant_studants_StudantsId",
                table: "ModuleStudant");

            migrationBuilder.DropForeignKey(
                name: "FK_semestrees_filieres_FiliereId",
                table: "semestrees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_studants",
                table: "studants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_semestrees",
                table: "semestrees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_modules",
                table: "modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_filieres",
                table: "filieres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chapitres",
                table: "chapitres");

            migrationBuilder.RenameTable(
                name: "studants",
                newName: "Studants");

            migrationBuilder.RenameTable(
                name: "semestrees",
                newName: "Semestrees");

            migrationBuilder.RenameTable(
                name: "products",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "modules",
                newName: "Modules");

            migrationBuilder.RenameTable(
                name: "filieres",
                newName: "Filieres");

            migrationBuilder.RenameTable(
                name: "chapitres",
                newName: "Chapitres");

            migrationBuilder.RenameIndex(
                name: "IX_semestrees_FiliereId",
                table: "Semestrees",
                newName: "IX_Semestrees_FiliereId");

            migrationBuilder.RenameIndex(
                name: "IX_modules_SemId",
                table: "Modules",
                newName: "IX_Modules_SemId");

            migrationBuilder.RenameIndex(
                name: "IX_chapitres_ModuleId",
                table: "Chapitres",
                newName: "IX_Chapitres_ModuleId");

            migrationBuilder.AlterColumn<long>(
                name: "ModuleId",
                table: "Chapitres",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Studants",
                table: "Studants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Semestrees",
                table: "Semestrees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Modules",
                table: "Modules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Filieres",
                table: "Filieres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chapitres",
                table: "Chapitres",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChapitreId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Chapitres_ChapitreId",
                        column: x => x.ChapitreId,
                        principalTable: "Chapitres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ChapitreId",
                table: "Activities",
                column: "ChapitreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapitres_Modules_ModuleId",
                table: "Chapitres",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Semestrees_SemId",
                table: "Modules",
                column: "SemId",
                principalTable: "Semestrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleStudant_Modules_ModulesId",
                table: "ModuleStudant",
                column: "ModulesId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleStudant_Studants_StudantsId",
                table: "ModuleStudant",
                column: "StudantsId",
                principalTable: "Studants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Semestrees_Filieres_FiliereId",
                table: "Semestrees",
                column: "FiliereId",
                principalTable: "Filieres",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapitres_Modules_ModuleId",
                table: "Chapitres");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Semestrees_SemId",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleStudant_Modules_ModulesId",
                table: "ModuleStudant");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleStudant_Studants_StudantsId",
                table: "ModuleStudant");

            migrationBuilder.DropForeignKey(
                name: "FK_Semestrees_Filieres_FiliereId",
                table: "Semestrees");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Studants",
                table: "Studants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Semestrees",
                table: "Semestrees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Modules",
                table: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Filieres",
                table: "Filieres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chapitres",
                table: "Chapitres");

            migrationBuilder.RenameTable(
                name: "Studants",
                newName: "studants");

            migrationBuilder.RenameTable(
                name: "Semestrees",
                newName: "semestrees");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "products");

            migrationBuilder.RenameTable(
                name: "Modules",
                newName: "modules");

            migrationBuilder.RenameTable(
                name: "Filieres",
                newName: "filieres");

            migrationBuilder.RenameTable(
                name: "Chapitres",
                newName: "chapitres");

            migrationBuilder.RenameIndex(
                name: "IX_Semestrees_FiliereId",
                table: "semestrees",
                newName: "IX_semestrees_FiliereId");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_SemId",
                table: "modules",
                newName: "IX_modules_SemId");

            migrationBuilder.RenameIndex(
                name: "IX_Chapitres_ModuleId",
                table: "chapitres",
                newName: "IX_chapitres_ModuleId");

            migrationBuilder.AlterColumn<long>(
                name: "ModuleId",
                table: "chapitres",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_studants",
                table: "studants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_semestrees",
                table: "semestrees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                table: "products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_modules",
                table: "modules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_filieres",
                table: "filieres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chapitres",
                table: "chapitres",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_chapitres_modules_ModuleId",
                table: "chapitres",
                column: "ModuleId",
                principalTable: "modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_modules_semestrees_SemId",
                table: "modules",
                column: "SemId",
                principalTable: "semestrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleStudant_modules_ModulesId",
                table: "ModuleStudant",
                column: "ModulesId",
                principalTable: "modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleStudant_studants_StudantsId",
                table: "ModuleStudant",
                column: "StudantsId",
                principalTable: "studants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_semestrees_filieres_FiliereId",
                table: "semestrees",
                column: "FiliereId",
                principalTable: "filieres",
                principalColumn: "Id");
        }
    }
}
