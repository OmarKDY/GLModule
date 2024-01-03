using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GLModule.Migrations
{
    /// <inheritdoc />
    public partial class Launching : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountCode = table.Column<int>(type: "int", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitialBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDebit = table.Column<bool>(type: "bit", nullable: false),
                    IsCredit = table.Column<bool>(type: "bit", nullable: false),
                    IsParent = table.Column<bool>(type: "bit", nullable: false),
                    ParentAccountCode = table.Column<int>(type: "int", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    AccountTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountCode);
                    table.ForeignKey(
                        name: "FK_Accounts_Accounts_ParentAccountCode",
                        column: x => x.ParentAccountCode,
                        principalTable: "Accounts",
                        principalColumn: "AccountCode");
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                    table.ForeignKey(
                        name: "FK_Banks_Accounts_AccountCode",
                        column: x => x.AccountCode,
                        principalTable: "Accounts",
                        principalColumn: "AccountCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashBankReceipts",
                columns: table => new
                {
                    CashBankReceiptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountCode = table.Column<int>(type: "int", nullable: false),
                    DailyJournalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashBankReceipts", x => x.CashBankReceiptId);
                    table.ForeignKey(
                        name: "FK_CashBankReceipts_Accounts_AccountCode",
                        column: x => x.AccountCode,
                        principalTable: "Accounts",
                        principalColumn: "AccountCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashBankReceives",
                columns: table => new
                {
                    CashBankReceiveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountCode = table.Column<int>(type: "int", nullable: false),
                    DailyJournalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashBankReceives", x => x.CashBankReceiveId);
                    table.ForeignKey(
                        name: "FK_CashBankReceives_Accounts_AccountCode",
                        column: x => x.AccountCode,
                        principalTable: "Accounts",
                        principalColumn: "AccountCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cashes",
                columns: table => new
                {
                    CashId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cashes", x => x.CashId);
                    table.ForeignKey(
                        name: "FK_Cashes_Accounts_AccountCode",
                        column: x => x.AccountCode,
                        principalTable: "Accounts",
                        principalColumn: "AccountCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyJournals",
                columns: table => new
                {
                    DailyJournalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountCode = table.Column<int>(type: "int", nullable: false),
                    CashBankReceiptId = table.Column<int>(type: "int", nullable: true),
                    CashBankReceiveId = table.Column<int>(type: "int", nullable: true),
                    ChequeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyJournals", x => x.DailyJournalId);
                    table.ForeignKey(
                        name: "FK_DailyJournals_Accounts_AccountCode",
                        column: x => x.AccountCode,
                        principalTable: "Accounts",
                        principalColumn: "AccountCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyJournals_CashBankReceipts_CashBankReceiptId",
                        column: x => x.CashBankReceiptId,
                        principalTable: "CashBankReceipts",
                        principalColumn: "CashBankReceiptId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DailyJournals_CashBankReceives_CashBankReceiveId",
                        column: x => x.CashBankReceiveId,
                        principalTable: "CashBankReceives",
                        principalColumn: "CashBankReceiveId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cheques",
                columns: table => new
                {
                    ChequeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDueCheque = table.Column<bool>(type: "bit", nullable: false),
                    DueChequeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccountCode = table.Column<int>(type: "int", nullable: false),
                    DailyJournalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cheques", x => x.ChequeId);
                    table.ForeignKey(
                        name: "FK_Cheques_Accounts_AccountCode",
                        column: x => x.AccountCode,
                        principalTable: "Accounts",
                        principalColumn: "AccountCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cheques_DailyJournals_DailyJournalId",
                        column: x => x.DailyJournalId,
                        principalTable: "DailyJournals",
                        principalColumn: "DailyJournalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountTransactions",
                columns: table => new
                {
                    AccountTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocSerial = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsInitialBalance = table.Column<bool>(type: "bit", nullable: false),
                    AccountCode = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CashBankReceiptId = table.Column<int>(type: "int", nullable: false),
                    CashBankReceiveId = table.Column<int>(type: "int", nullable: false),
                    ChequeId = table.Column<int>(type: "int", nullable: false),
                    DailyJournalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTransactions", x => x.AccountTransactionId);
                    table.ForeignKey(
                        name: "FK_AccountTransactions_CashBankReceipts_CashBankReceiptId",
                        column: x => x.CashBankReceiptId,
                        principalTable: "CashBankReceipts",
                        principalColumn: "CashBankReceiptId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountTransactions_CashBankReceives_CashBankReceiveId",
                        column: x => x.CashBankReceiveId,
                        principalTable: "CashBankReceives",
                        principalColumn: "CashBankReceiveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountTransactions_Cheques_ChequeId",
                        column: x => x.ChequeId,
                        principalTable: "Cheques",
                        principalColumn: "ChequeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountTransactions_DailyJournals_DailyJournalId",
                        column: x => x.DailyJournalId,
                        principalTable: "DailyJournals",
                        principalColumn: "DailyJournalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountName",
                table: "Accounts",
                column: "AccountName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountTransactionId",
                table: "Accounts",
                column: "AccountTransactionId",
                unique: true,
                filter: "[AccountTransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ParentAccountCode",
                table: "Accounts",
                column: "ParentAccountCode");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_CashBankReceiptId",
                table: "AccountTransactions",
                column: "CashBankReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_CashBankReceiveId",
                table: "AccountTransactions",
                column: "CashBankReceiveId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_ChequeId",
                table: "AccountTransactions",
                column: "ChequeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_DailyJournalId",
                table: "AccountTransactions",
                column: "DailyJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_DocSerial",
                table: "AccountTransactions",
                column: "DocSerial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banks_AccountCode",
                table: "Banks",
                column: "AccountCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banks_BankName",
                table: "Banks",
                column: "BankName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashBankReceipts_AccountCode",
                table: "CashBankReceipts",
                column: "AccountCode");

            migrationBuilder.CreateIndex(
                name: "IX_CashBankReceives_AccountCode",
                table: "CashBankReceives",
                column: "AccountCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cashes_AccountCode",
                table: "Cashes",
                column: "AccountCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cashes_CashName",
                table: "Cashes",
                column: "CashName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cheques_AccountCode",
                table: "Cheques",
                column: "AccountCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cheques_DailyJournalId",
                table: "Cheques",
                column: "DailyJournalId",
                unique: true,
                filter: "[DailyJournalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DailyJournals_AccountCode",
                table: "DailyJournals",
                column: "AccountCode");

            migrationBuilder.CreateIndex(
                name: "IX_DailyJournals_CashBankReceiptId",
                table: "DailyJournals",
                column: "CashBankReceiptId",
                unique: true,
                filter: "[CashBankReceiptId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DailyJournals_CashBankReceiveId",
                table: "DailyJournals",
                column: "CashBankReceiveId",
                unique: true,
                filter: "[CashBankReceiveId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AccountTransactions_AccountTransactionId",
                table: "Accounts",
                column: "AccountTransactionId",
                principalTable: "AccountTransactions",
                principalColumn: "AccountTransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AccountTransactions_AccountTransactionId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "Cashes");

            migrationBuilder.DropTable(
                name: "AccountTransactions");

            migrationBuilder.DropTable(
                name: "Cheques");

            migrationBuilder.DropTable(
                name: "DailyJournals");

            migrationBuilder.DropTable(
                name: "CashBankReceipts");

            migrationBuilder.DropTable(
                name: "CashBankReceives");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
