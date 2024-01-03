using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GLModule.Migrations
{
    /// <inheritdoc />
    public partial class AccountTransactionStringDocSerial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountTransactions_DocSerial",
                table: "AccountTransactions");

            migrationBuilder.AlterColumn<string>(
                name: "DocSerial",
                table: "AccountTransactions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsDailyJournal",
                table: "AccountTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_DocSerial",
                table: "AccountTransactions",
                column: "DocSerial",
                unique: true,
                filter: "[DocSerial] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountTransactions_DocSerial",
                table: "AccountTransactions");

            migrationBuilder.DropColumn(
                name: "IsDailyJournal",
                table: "AccountTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "DocSerial",
                table: "AccountTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_DocSerial",
                table: "AccountTransactions",
                column: "DocSerial",
                unique: true);
        }
    }
}
