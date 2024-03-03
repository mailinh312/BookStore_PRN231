using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    public partial class ver5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportDetails_Imports_ImportId",
                table: "ImportDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImportDetails",
                table: "ImportDetails");

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "ImportId",
                table: "ImportDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ImportDetailId",
                table: "ImportDetails",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                column: "OrderDetailId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImportDetails",
                table: "ImportDetails",
                column: "ImportDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportDetails_ImportId",
                table: "ImportDetails",
                column: "ImportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportDetails_Imports_ImportId",
                table: "ImportDetails",
                column: "ImportId",
                principalTable: "Imports",
                principalColumn: "ImportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportDetails_Imports_ImportId",
                table: "ImportDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImportDetails",
                table: "ImportDetails");

            migrationBuilder.DropIndex(
                name: "IX_ImportDetails_ImportId",
                table: "ImportDetails");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ImportDetailId",
                table: "ImportDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ImportId",
                table: "ImportDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                columns: new[] { "OrderId", "BookId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImportDetails",
                table: "ImportDetails",
                columns: new[] { "ImportId", "BookId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ImportDetails_Imports_ImportId",
                table: "ImportDetails",
                column: "ImportId",
                principalTable: "Imports",
                principalColumn: "ImportId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
