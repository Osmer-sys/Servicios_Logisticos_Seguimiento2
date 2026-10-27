using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Servicios_Logisticos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsRestrictedArea = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CredentialCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    AccessPointId = table.Column<int>(type: "int", nullable: false),
                    MovementType = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsGranted = table.Column<bool>(type: "bit", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessLogs_AccessPoints_AccessPointId",
                        column: x => x.AccessPointId,
                        principalTable: "AccessPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credentials_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AccessPoints",
                columns: new[] { "Id", "CreatedAt", "Description", "IsEnabled", "IsRestrictedArea", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Torniquete de acceso peatonal principal a instalaciones administrativas", true, false, "Recepción Principal", null },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Puerta industrial con sensor de proximidad de zona de almacenamiento", true, true, "Bodega Central de Almacenamiento", null },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Talancón automatizado de ingreso vehicular logístico", true, false, "Parqueadero de Carga Pesada", null },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Puerta blindada con cerradura biométrica y tarjeta para personal IT", true, true, "Área Restringida - Servidores y CCTV", null },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Punto de acceso inhabilitado por obras de modernización", false, false, "Muelle 4 (En Mantenimiento)", null }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CreatedAt", "Department", "DocumentNumber", "Email", "FirstName", "LastName", "Position", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), "Operaciones Logísticas", "1001001001", "carlos.gomez@servicioslogisticos.com", "Carlos", "Gómez Pérez", "Supervisor de Almacén", 1, null },
                    { 2, new DateTime(2024, 2, 1, 8, 0, 0, 0, DateTimeKind.Utc), "Bodega Central", "1002002002", "maria.rodriguez@servicioslogisticos.com", "María", "Rodríguez Silva", "Operadora de Montacargas", 1, null },
                    { 3, new DateTime(2024, 3, 10, 8, 0, 0, 0, DateTimeKind.Utc), "Distribución y Transporte", "1003003003", "andres.martinez@servicioslogisticos.com", "Andrés", "Martínez Ruiz", "Auxiliar de Despacho", 2, null }
                });

            migrationBuilder.InsertData(
                table: "Credentials",
                columns: new[] { "Id", "AssignedAt", "Code", "EmployeeId", "Notes", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 15, 8, 30, 0, 0, DateTimeKind.Utc), "CRD-2024-001", 1, "Tarjeta RFID Principal asignada al Supervisor", 1, null },
                    { 2, new DateTime(2024, 2, 1, 8, 30, 0, 0, DateTimeKind.Utc), "CRD-2024-002", 2, "Badge de proximidad para Bodega", 1, null },
                    { 3, new DateTime(2024, 2, 20, 10, 0, 0, 0, DateTimeKind.Utc), "CRD-2024-003", 1, "Tarjeta temporal reportada extraviada", 2, null },
                    { 4, new DateTime(2024, 3, 10, 8, 30, 0, 0, DateTimeKind.Utc), "CRD-2024-004", 3, "Asignada a empleado inactivo para validación de RF-03", 1, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_AccessPointId",
                table: "AccessLogs",
                column: "AccessPointId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_CredentialCode",
                table: "AccessLogs",
                column: "CredentialCode");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_EmployeeId",
                table: "AccessLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_IsGranted",
                table: "AccessLogs",
                column: "IsGranted");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_Timestamp",
                table: "AccessLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoints_Name",
                table: "AccessPoints",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_Code",
                table: "Credentials",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_EmployeeId",
                table: "Credentials",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DocumentNumber",
                table: "Employees",
                column: "DocumentNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessLogs");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "AccessPoints");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
