using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PersonnelManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePersonnels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePersonnels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_RoleAccounts_RoleAccountId",
                        column: x => x.RoleAccountId,
                        principalTable: "RoleAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_RoleAccounts_RoleAccountId",
                        column: x => x.RoleAccountId,
                        principalTable: "RoleAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Personnels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreatedCccd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RolePersonnel = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personnels_RolePersonnels_RolePersonnel",
                        column: x => x.RolePersonnel,
                        principalTable: "RolePersonnels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccountId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tokens_Accounts_AccountId1",
                        column: x => x.AccountId1,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PersonnelHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonnelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonnelHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonnelHistories_Personnels_PersonnelId",
                        column: x => x.PersonnelId,
                        principalTable: "Personnels",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Code", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("13a7e164-4265-426e-aff8-cda304df4eb5"), "EDIT_PERSONNEl_GROUP", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8815), "Cho phép sửa phân nhóm nhân viên", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8816) },
                    { new Guid("1778ff61-a19e-4f58-abe4-407e42e5445b"), "EDIT_ACCOUNT", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8826), "Cho phép sửa tài khoản đăng nhập", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8826) },
                    { new Guid("25722ca8-cca1-4a8b-aa78-14d3cd5dc0e3"), "ADD_PERSONNEl_GROUP", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8812), "Cho phép thêm phân nhóm nhân viên", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8813) },
                    { new Guid("3620b076-ad7d-425b-a22a-20f97bf14821"), "DELETE_ROLE_ACCOUNT", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8842), "Cho phép xóa nhóm phân quyền", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8842) },
                    { new Guid("3d75129e-ac23-48e3-964c-a0b4f1c9791a"), "VIEW_HISTORY", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8821), "Cho phép xem lịch sử", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8821) },
                    { new Guid("54b9a0c6-ecae-4144-9417-e41c9b1db7e2"), "EDIT_PERSONNEl", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8807), "Cho phép sửa thông tin nhân viên", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8808) },
                    { new Guid("584a32a5-236e-4d41-9f00-6bf709d92e87"), "DELETE_PERSONNEl_GROUP", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8818), "Cho phép xóa phân nhóm nhân viên", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8818) },
                    { new Guid("58c58baa-bc00-4ec1-9d93-6fd53f9a93ec"), "ADD_PERSONNEl", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8776), "Cho phép thêm nhân viên", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8777) },
                    { new Guid("85e46dc7-69e0-4cae-a68a-79de683e0f61"), "ADD_ACCOUNT", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8823), "Cho phép thêm tài khoản đăng nhập", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8824) },
                    { new Guid("a357f6d3-ed02-48fd-be06-e34f7605c505"), "ADD_ROLE_ACCOUNT", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8830), "Cho phép thêm nhóm phân quyền", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8831) },
                    { new Guid("a7463264-74a1-42a3-bc4d-b281833f5d0d"), "DELETE_PERSONNEl", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8810), "Cho phép xóa nhân viên", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8811) },
                    { new Guid("c83e7f07-564b-41e1-8de4-cabf39013ef9"), "DELETE_ACCOUNT", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8828), "Cho phép xóa tài khoản đăng nhập", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8829) },
                    { new Guid("c98f3e02-7535-4998-bdcd-8560dad8c35e"), "EDIT_ROLE_ACCOUNT", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8832), "Cho phép sửa nhóm phân quyền", new DateTime(2025, 4, 1, 15, 9, 24, 805, DateTimeKind.Utc).AddTicks(8833) }
                });

            migrationBuilder.InsertData(
                table: "RoleAccounts",
                columns: new[] { "Id", "CreatedAt", "RoleName", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("6ce51904-32a9-4dc6-ac57-3f3704d65a68"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8608), "NhanVien", new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8608) },
                    { new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8597), "Admin", new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8599) }
                });

            migrationBuilder.InsertData(
                table: "RolePersonnels",
                columns: new[] { "Id", "CreatedAt", "RoleName", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("91d1e478-813c-4618-b0f6-3247f1703576"), new DateTime(2025, 4, 1, 8, 9, 24, 388, DateTimeKind.Utc).AddTicks(7974), "Khach", new DateTime(2025, 4, 1, 8, 9, 24, 388, DateTimeKind.Utc).AddTicks(7974) },
                    { new Guid("dd911a5e-010c-449a-a143-6ce5e23e221c"), new DateTime(2025, 4, 1, 8, 9, 24, 388, DateTimeKind.Utc).AddTicks(7970), "CanBo", new DateTime(2025, 4, 1, 8, 9, 24, 388, DateTimeKind.Utc).AddTicks(7971) },
                    { new Guid("f1137868-e530-423a-a732-8dde59a2d77b"), new DateTime(2025, 4, 1, 8, 9, 24, 388, DateTimeKind.Utc).AddTicks(7957), "HocVien", new DateTime(2025, 4, 1, 8, 9, 24, 388, DateTimeKind.Utc).AddTicks(7958) }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "RoleAccountId", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { new Guid("0a036be5-2081-40ee-aa81-2b0994e25fb1"), new DateTime(2025, 4, 1, 15, 9, 24, 388, DateTimeKind.Utc).AddTicks(8155), "$2a$11$hLrT52QVJYTkuoAw5QWT8uwjPD5sZOJtJFNGAjsfZWmQT9cGCXHVm", new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 15, 9, 24, 388, DateTimeKind.Utc).AddTicks(8155), "admin" },
                    { new Guid("2aa5a408-9056-43a0-a881-c4375243a5d6"), new DateTime(2025, 4, 1, 15, 9, 24, 597, DateTimeKind.Utc).AddTicks(5183), "$2a$11$NJVZDwwHIXI/tKohiWA9s.22tcyhRZYmKdYeZ1muBzEQYPeKxYY8O", new Guid("6ce51904-32a9-4dc6-ac57-3f3704d65a68"), new DateTime(2025, 4, 1, 15, 9, 24, 597, DateTimeKind.Utc).AddTicks(5187), "nguyenhau" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedAt", "PermissionId", "RoleAccountId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0e4cbf40-c21c-49ef-a8bd-b404b7d99033"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8949), new Guid("58c58baa-bc00-4ec1-9d93-6fd53f9a93ec"), new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8950) },
                    { new Guid("1015d3a5-d9ec-473c-bde3-d98c36b9fc46"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8994), new Guid("3620b076-ad7d-425b-a22a-20f97bf14821"), new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8995) },
                    { new Guid("3791ebdb-bbae-447f-9164-e9470a719dde"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8956), new Guid("54b9a0c6-ecae-4144-9417-e41c9b1db7e2"), new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8957) },
                    { new Guid("4ef3446b-a17e-40d1-bb15-96be21ec5985"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(9021), new Guid("58c58baa-bc00-4ec1-9d93-6fd53f9a93ec"), new Guid("6ce51904-32a9-4dc6-ac57-3f3704d65a68"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(9021) },
                    { new Guid("56d220bd-06bc-44d2-be78-7a0df0bd0733"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8981), new Guid("a357f6d3-ed02-48fd-be06-e34f7605c505"), new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8981) },
                    { new Guid("6c4216d6-c7d4-4dbe-8067-89f8f897a2c5"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8967), new Guid("25722ca8-cca1-4a8b-aa78-14d3cd5dc0e3"), new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8967) },
                    { new Guid("7b2fccd4-2256-487a-916c-c272b40d1934"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8999), new Guid("3d75129e-ac23-48e3-964c-a0b4f1c9791a"), new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(9000) },
                    { new Guid("9f380009-8942-4e3c-8e86-fe3fa514e204"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(9008), new Guid("1778ff61-a19e-4f58-abe4-407e42e5445b"), new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(9009) },
                    { new Guid("acd50988-6b52-42e5-82e2-ef1b431988db"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8971), new Guid("13a7e164-4265-426e-aff8-cda304df4eb5"), new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8972) },
                    { new Guid("b90df6e1-f160-43d2-8773-9f6d39b1ff21"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(9016), new Guid("3d75129e-ac23-48e3-964c-a0b4f1c9791a"), new Guid("6ce51904-32a9-4dc6-ac57-3f3704d65a68"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(9017) },
                    { new Guid("ce4335e4-c933-4f09-bbe1-7f51b3f45e83"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8962), new Guid("a7463264-74a1-42a3-bc4d-b281833f5d0d"), new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8963) },
                    { new Guid("d9e49fee-dda4-4f66-90b0-e833788a1317"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8976), new Guid("584a32a5-236e-4d41-9f00-6bf709d92e87"), new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8977) },
                    { new Guid("e83d1905-626c-4b64-b099-cc7101148542"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(9004), new Guid("85e46dc7-69e0-4cae-a68a-79de683e0f61"), new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(9004) },
                    { new Guid("eeb2f829-c954-4358-bc67-d78217366a64"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8989), new Guid("c98f3e02-7535-4998-bdcd-8560dad8c35e"), new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(8990) },
                    { new Guid("f643755a-d89c-4140-9b56-589bef76b834"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(9012), new Guid("c83e7f07-564b-41e1-8de4-cabf39013ef9"), new Guid("88958239-c499-4d74-92e0-74caeb4ff03d"), new DateTime(2025, 4, 1, 8, 9, 24, 805, DateTimeKind.Utc).AddTicks(9013) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_RoleAccountId",
                table: "Accounts",
                column: "RoleAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Username",
                table: "Accounts",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelHistories_PersonnelId",
                table: "PersonnelHistories",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_Personnels_NumberId",
                table: "Personnels",
                column: "NumberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personnels_RolePersonnel",
                table: "Personnels",
                column: "RolePersonnel");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleAccountId",
                table: "RolePermissions",
                column: "RoleAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_AccountId",
                table: "Tokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_AccountId1",
                table: "Tokens",
                column: "AccountId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonnelHistories");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Personnels");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "RolePersonnels");

            migrationBuilder.DropTable(
                name: "RoleAccounts");
        }
    }
}
