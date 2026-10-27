# 🏢 Servicios_Logisticos_Seguimiento2
### Sistema de Gestión y Control de Acceso de Empleados (SGCAE)

Backend empresarial desarrollado en **.NET 8 (C#)** bajo **Clean Architecture**, **Domain-Driven Design (DDD)** y **CQRS**, con persistencia en **SQL Server** mediante **Entity Framework Core 8** y exposición de servicios a través de **ASP.NET Core Web API**.

Desarrollado para el **Seguimiento 2 de Programación Distribuida** - ITM.

---

## 📋 Contexto del Negocio y Requerimientos Funcionales

Una empresa dedicada a la prestación de servicios logísticos y de almacenamiento cuenta con una sede principal y diversas áreas de acceso restringido (bodegas, patios de carga pesada, cuartos de servidores y CCTV). El presente sistema reemplaza los registros en planillas físicas y hojas de cálculo por una plataforma centralizada y trazable.

### Requerimientos Funcionales Implementados:
- **RF-01. Gestión de Empleados:** Registrar, consultar (por ID, documento y listado general), actualizar y desactivar empleados con identificación única, nombres, apellidos, correo electrónico corporativo, cargo, área organizacional y estado laboral.
- **RF-02. Gestión de Credenciales:** Asignar credenciales físicas/RFID a los empleados, consultar su estado, bloquearlas (por extravío o temporal) y revocarlas definitivamente.
- **RF-03. Registro de Entradas y Salidas:** Procesar solicitudes de acceso con código de credencial, punto de acceso y tipo de movimiento (Entrada o Salida). Valida reglas de negocio (credencial vigente, empleado con vínculo laboral activo, punto de acceso habilitado) y registra el resultado trazable tanto de accesos autorizados como de accesos rechazados.
- **RF-04. Consulta del Historial de Accesos:** Consultar movimientos históricos filtrando por empleado, rango de fechas, punto de acceso físico y resultado de validación (autorizado/rechazado), implementando **paginación** (`pageNumber`, `pageSize`, `totalCount`, `totalPages`).
- **RF-06. Administración de Puntos de Acceso:** Registrar y consultar puntos de acceso físicos de la organización (recepción, bodegas, parqueadero logístico, áreas restringidas) identificando cuáles se encuentran habilitados.

---

## 🏛️ Arquitectura del Sistema

El proyecto sigue estrictamente los principios de **Clean Architecture** (Arquitectura Limpia) con inversión de dependencias:

```
Servicios_Logisticos_Seguimiento2/
├── src/
│   ├── Servicios_Logisticos.Domain/                  # Capa de Dominio (Sin dependencias externas)
│   │   ├── Entities/                                 # Entidades y Agregados (Employee, Credential, AccessPoint, AccessLog)
│   │   ├── ValueObjects/                             # Objetos de Valor inmutables (Email, CredentialCode)
│   │   ├── Enums/                                    # Enumeradores (EmploymentStatus, CredentialStatus, AccessMovementType)
│   │   ├── Exceptions/                               # Excepciones de Dominio (BusinessRuleException, EntityNotFoundException)
│   │   └── Interfaces/                               # Contratos de Repositorios y Unit of Work
│   │
│   ├── Servicios_Logisticos.Application/             # Capa de Aplicación (CQRS - Comandos y Consultas)
│   │   ├── Commands/                                 # Operaciones de Escritura
│   │   │   ├── Employees/                            # CreateEmployee, UpdateEmployee, DeactivateEmployee
│   │   │   ├── Credentials/                          # AssignCredential, BlockCredential, RevokeCredential
│   │   │   ├── AccessPoints/                         # CreateAccessPoint, UpdateAccessPoint
│   │   │   └── AccessLogs/                           # RegisterAccessMovement (Valida y audita RF-03)
│   │   ├── Queries/                                  # Operaciones de Lectura
│   │   │   ├── Employees/                            # GetAllEmployees, GetEmployeeById, GetEmployeeByDocument
│   │   │   ├── Credentials/                          # GetAllCredentials, GetCredentialByCode, GetCredentialsByEmployeeId
│   │   │   ├── AccessPoints/                         # GetAllAccessPoints, GetAccessPointById
│   │   │   └── AccessLogs/                           # GetAccessLogsPaged (Historial con paginación RF-04)
│   │   ├── DTOs/                                     # Objetos de Transferencia de Datos y PagedResult<T>
│   │   ├── Mappers/                                  # Extensiones de mapeo Entidad <-> DTO
│   │   ├── Utilities/Mediator/                       # Implementación desacoplada del patrón Mediador
│   │   └── Extensions/                               # Inyección de dependencias de Aplicación
│   │
│   ├── Servicios_Logisticos.Infrastructure/          # Capa de Infraestructura (Persistencia SQL Server)
│   │   ├── Persistence/
│   │   │   ├── DbContext/                            # ApplicationDbContext e IDesignTimeDbContextFactory
│   │   │   ├── Configurations/                       # Mapeos Fluent API (Entity Configurations)
│   │   │   ├── Repositories/                         # Implementación de Repositorios (EF Core)
│   │   │   ├── Migrations/                           # Migraciones generadas por EF Core
│   │   │   └── Extensions/                           # Inyección de dependencias de Persistencia
│   │
│   └── Servicios_Logisticos.Presentation/             # Capa de Presentación (ASP.NET Core Web API)
│       ├── Controllers/                              # Controladores RESTful
│       ├── Common/                                   # Filtro global unificado de excepciones (ApiExceptionFilter)
│       ├── Program.cs                                # Configuración de pipeline HTTP y Swagger
│       └── appsettings.json                          # Configuración y plantilla de cadena de conexión
│
├── scripts/
│   └── database.sql                                  # Script SQL idempotente DDL y Seed de Base de Datos
├── run.bat                                           # Script de ejecución automatizada
├── README.md                                         # Documentación del proyecto
└── Servicios_Logisticos_Seguimiento2.sln             # Solución de Visual Studio
```

---

## 🗄️ Persistencia y Base de Datos (SQL Server)

La base de datos utilizada es **SQL Server** mediante **Entity Framework Core 8**.

### Configuración de la Cadena de Conexión
La cadena de conexión se configura en `src/Servicios_Logisticos.Presentation/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ServiciosLogisticosDB;User Id=tu_usuario;Password=tu_contraseña;TrustServerCertificate=True;"
  }
}
```

> **Nota de Seguridad:** En el repositorio las credenciales se mantienen como marcadores de posición genéricos (`tu_usuario`, `tu_contraseña`). Para entornos locales o de desarrollo, configure sus credenciales de SQL Server o utilice variables de entorno (`ConnectionStrings__DefaultConnection`).

---

## 🚀 Instrucciones de Ejecución

### Requisitos Previos
1. **.NET SDK 8.0** o superior (verificar con `dotnet --version`).
2. **SQL Server** (LocalDB, Express o SQL Server Developer/Enterprise en ejecución).
3. Herramienta global de EF Core (opcional para el script `.bat` si se compila con migraciones):
   ```bash
   dotnet tool install --global dotnet-ef
   ```

### Opción 1: Ejecución con el script automatizado `run.bat`
En la raíz de la solución, ejecute en PowerShell o CMD:
```cmd
.\run.bat
```
El script realiza automáticamente los 4 pasos:
1. Verifica la presencia del SDK de .NET.
2. Restaura las dependencias NuGet de la solución.
3. Compila la solución completa en modo `Release`.
4. Aplica las migraciones pendientes en SQL Server.
5. Inicia el servidor Web API en el navegador.

### Opción 2: Ejecución desde Visual Studio 2022
1. Abrir `Servicios_Logisticos_Seguimiento2.sln` con **Visual Studio 2022**.
2. Establecer `Servicios_Logisticos.Presentation` como proyecto de inicio (*Set as Startup Project*).
3. Verificar o ajustar su cadena de conexión en `appsettings.json`.
4. Presionar `F5` o `Ctrl + F5` para compilar y ejecutar.
5. El explorador abrirá automáticamente la interfaz de **Swagger UI** en la raíz: `https://localhost:{puerto}/`.

### Opción 3: Ejecución desde Terminal (.NET CLI)
```bash
# Restaurar dependencias
dotnet restore

# Compilar
dotnet build --configuration Release

# Aplicar migraciones a SQL Server
dotnet ef database update --project src/Servicios_Logisticos.Infrastructure --startup-project src/Servicios_Logisticos.Presentation

# Ejecutar el API
dotnet run --project src/Servicios_Logisticos.Presentation
```

---

## 🌐 Endpoints de la API REST

### 1. Gestión de Empleados (`/api/employees`) - RF-01
| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/api/employees` | Obtiene el listado completo de empleados. |
| `GET` | `/api/employees/{id}` | Obtiene los datos detallados de un empleado por ID. |
| `GET` | `/api/employees/document/{documentNumber}` | Busca un empleado por su cédula / documento único. |
| `POST` | `/api/employees` | Registra un nuevo empleado (valida documento único y formato de correo). |
| `PUT` | `/api/employees/{id}` | Actualiza datos personales, cargo y departamento del empleado. |
| `PATCH` | `/api/employees/{id}/deactivate` | Desactiva a un empleado (cambia estado a Inactivo). |

### 2. Gestión de Credenciales (`/api/credentials`) - RF-02
| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/api/credentials` | Obtiene el listado de todas las credenciales registradas. |
| `GET` | `/api/credentials/{code}` | Consulta el estado y detalle de una credencial por su código único. |
| `GET` | `/api/credentials/employee/{employeeId}` | Obtiene las credenciales asignadas a un empleado. |
| `POST` | `/api/credentials` | Asigna una nueva credencial a un empleado activo. |
| `PATCH` | `/api/credentials/{code}/block` | Bloquea una credencial (impide accesos futuros). |
| `PATCH` | `/api/credentials/{code}/revoke` | Revoca definitivamente una credencial. |

### 3. Puntos de Acceso Físicos (`/api/access-points`) - RF-06
| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/api/access-points` | Lista los puntos de acceso (soporta filtro opcional `?enabledOnly=true`). |
| `GET` | `/api/access-points/{id}` | Obtiene el detalle de un punto de acceso físico. |
| `POST` | `/api/access-points` | Registra un nuevo punto de acceso físico. |
| `PUT` | `/api/access-points/{id}` | Actualiza nombre, descripción o habilita/inhabilita el punto. |

### 4. Registro y Consulta de Accesos (`/api/access-logs`) - RF-03 y RF-04
| Método | Ruta | Descripción |
|---|---|---|
| `POST` | `/api/access-logs/register` | **RF-03:** Procesa una validación de acceso (entrada/salida). Valida estado de credencial, empleado y punto de acceso; persiste en el historial el resultado (autorizado o rechazado con motivo). |
| `GET` | `/api/access-logs` | **RF-04:** Consulta el historial con paginación (`pageNumber`, `pageSize`) y filtros por `employeeId`, `startDate`, `endDate`, `accessPointId`, `isGranted`. |

---

## 🧪 Casos de Prueba y Verificación de Reglas de Negocio

El sistema incluye datos semilla (*Seed Data*) para comprobar de inmediato los diferentes escenarios:

1. **Acceso Autorizado:**
   - Credencial: `CRD-2024-001` (Carlos Gómez - Empleado Activo)
   - Punto de Acceso: `1` (Recepción Principal - Habilitado)
   - Movimiento: `Entrada`
   - *Resultado esperado:* `isGranted: true`, acceso concedido.

2. **Acceso Denegado por Empleado Inactivo:**
   - Credencial: `CRD-2024-004` (Andrés Martínez - Empleado Inactivo)
   - Punto de Acceso: `1` (Recepción Principal)
   - Movimiento: `Entrada`
   - *Resultado esperado:* `isGranted: false`, motivo: `"El empleado asociado a la credencial tiene el vínculo laboral inactivo."`.

3. **Acceso Denegado por Credencial Bloqueada:**
   - Credencial: `CRD-2024-003` (Credencial Bloqueada)
   - Punto de Acceso: `2` (Bodega Central)
   - Movimiento: `Entrada`
   - *Resultado esperado:* `isGranted: false`, motivo: `"La credencial se encuentra bloqueada."`.

4. **Acceso Denegado por Punto de Acceso Inhabilitado:**
   - Credencial: `CRD-2024-001` (Empleado Activo)
   - Punto de Acceso: `5` (Muelle 4 - En Mantenimiento, Inhabilitado)
   - Movimiento: `Entrada`
   - *Resultado esperado:* `isGranted: false`, motivo: `"El punto de acceso 'Muelle 4 (En Mantenimiento)' se encuentra inhabilitado."`.

---

## 🛠️ Tecnologías y Librerías Utilizadas
- **Lenguaje:** C# 12 / .NET 8 LTS
- **Persistencia:** Entity Framework Core 8.0.8 (SQL Server Provider)
- **Documentación:** OpenAPI / Swagger (Swashbuckle 6.7.3)
- **Patrones de Diseño:** Clean Architecture, Domain-Driven Design (DDD), Command Query Responsibility Segregation (CQRS), Repository Pattern, Unit of Work, Mediator Pattern.
