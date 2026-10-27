IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    CREATE TABLE [AccessPoints] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(250) NULL,
        [IsEnabled] bit NOT NULL,
        [IsRestrictedArea] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_AccessPoints] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    CREATE TABLE [Employees] (
        [Id] int NOT NULL IDENTITY,
        [DocumentNumber] nvarchar(20) NOT NULL,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(100) NOT NULL,
        [Email] nvarchar(150) NOT NULL,
        [Position] nvarchar(100) NOT NULL,
        [Department] nvarchar(100) NOT NULL,
        [Status] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    CREATE TABLE [AccessLogs] (
        [Id] int NOT NULL IDENTITY,
        [CredentialCode] nvarchar(50) NOT NULL,
        [EmployeeId] int NULL,
        [AccessPointId] int NOT NULL,
        [MovementType] int NOT NULL,
        [Timestamp] datetime2 NOT NULL,
        [IsGranted] bit NOT NULL,
        [RejectionReason] nvarchar(250) NULL,
        CONSTRAINT [PK_AccessLogs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AccessLogs_AccessPoints_AccessPointId] FOREIGN KEY ([AccessPointId]) REFERENCES [AccessPoints] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AccessLogs_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE SET NULL
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    CREATE TABLE [Credentials] (
        [Id] int NOT NULL IDENTITY,
        [Code] nvarchar(50) NOT NULL,
        [EmployeeId] int NOT NULL,
        [Status] int NOT NULL,
        [AssignedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [Notes] nvarchar(250) NULL,
        CONSTRAINT [PK_Credentials] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Credentials_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'IsEnabled', N'IsRestrictedArea', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[AccessPoints]'))
        SET IDENTITY_INSERT [AccessPoints] ON;
    EXEC(N'INSERT INTO [AccessPoints] ([Id], [CreatedAt], [Description], [IsEnabled], [IsRestrictedArea], [Name], [UpdatedAt])
    VALUES (1, ''2024-01-01T00:00:00.0000000Z'', N''Torniquete de acceso peatonal principal a instalaciones administrativas'', CAST(1 AS bit), CAST(0 AS bit), N''Recepción Principal'', NULL),
    (2, ''2024-01-01T00:00:00.0000000Z'', N''Puerta industrial con sensor de proximidad de zona de almacenamiento'', CAST(1 AS bit), CAST(1 AS bit), N''Bodega Central de Almacenamiento'', NULL),
    (3, ''2024-01-01T00:00:00.0000000Z'', N''Talancón automatizado de ingreso vehicular logístico'', CAST(1 AS bit), CAST(0 AS bit), N''Parqueadero de Carga Pesada'', NULL),
    (4, ''2024-01-01T00:00:00.0000000Z'', N''Puerta blindada con cerradura biométrica y tarjeta para personal IT'', CAST(1 AS bit), CAST(1 AS bit), N''Área Restringida - Servidores y CCTV'', NULL),
    (5, ''2024-01-01T00:00:00.0000000Z'', N''Punto de acceso inhabilitado por obras de modernización'', CAST(0 AS bit), CAST(0 AS bit), N''Muelle 4 (En Mantenimiento)'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'IsEnabled', N'IsRestrictedArea', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[AccessPoints]'))
        SET IDENTITY_INSERT [AccessPoints] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Department', N'DocumentNumber', N'Email', N'FirstName', N'LastName', N'Position', N'Status', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Employees]'))
        SET IDENTITY_INSERT [Employees] ON;
    EXEC(N'INSERT INTO [Employees] ([Id], [CreatedAt], [Department], [DocumentNumber], [Email], [FirstName], [LastName], [Position], [Status], [UpdatedAt])
    VALUES (1, ''2024-01-15T08:00:00.0000000Z'', N''Operaciones Logísticas'', N''1001001001'', N''carlos.gomez@servicioslogisticos.com'', N''Carlos'', N''Gómez Pérez'', N''Supervisor de Almacén'', 1, NULL),
    (2, ''2024-02-01T08:00:00.0000000Z'', N''Bodega Central'', N''1002002002'', N''maria.rodriguez@servicioslogisticos.com'', N''María'', N''Rodríguez Silva'', N''Operadora de Montacargas'', 1, NULL),
    (3, ''2024-03-10T08:00:00.0000000Z'', N''Distribución y Transporte'', N''1003003003'', N''andres.martinez@servicioslogisticos.com'', N''Andrés'', N''Martínez Ruiz'', N''Auxiliar de Despacho'', 2, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Department', N'DocumentNumber', N'Email', N'FirstName', N'LastName', N'Position', N'Status', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Employees]'))
        SET IDENTITY_INSERT [Employees] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AssignedAt', N'Code', N'EmployeeId', N'Notes', N'Status', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Credentials]'))
        SET IDENTITY_INSERT [Credentials] ON;
    EXEC(N'INSERT INTO [Credentials] ([Id], [AssignedAt], [Code], [EmployeeId], [Notes], [Status], [UpdatedAt])
    VALUES (1, ''2024-01-15T08:30:00.0000000Z'', N''CRD-2024-001'', 1, N''Tarjeta RFID Principal asignada al Supervisor'', 1, NULL),
    (2, ''2024-02-01T08:30:00.0000000Z'', N''CRD-2024-002'', 2, N''Badge de proximidad para Bodega'', 1, NULL),
    (3, ''2024-02-20T10:00:00.0000000Z'', N''CRD-2024-003'', 1, N''Tarjeta temporal reportada extraviada'', 2, NULL),
    (4, ''2024-03-10T08:30:00.0000000Z'', N''CRD-2024-004'', 3, N''Asignada a empleado inactivo para validación de RF-03'', 1, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AssignedAt', N'Code', N'EmployeeId', N'Notes', N'Status', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Credentials]'))
        SET IDENTITY_INSERT [Credentials] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AccessLogs_AccessPointId] ON [AccessLogs] ([AccessPointId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AccessLogs_CredentialCode] ON [AccessLogs] ([CredentialCode]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AccessLogs_EmployeeId] ON [AccessLogs] ([EmployeeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AccessLogs_IsGranted] ON [AccessLogs] ([IsGranted]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_AccessLogs_Timestamp] ON [AccessLogs] ([Timestamp]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_AccessPoints_Name] ON [AccessPoints] ([Name]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Credentials_Code] ON [Credentials] ([Code]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Credentials_EmployeeId] ON [Credentials] ([EmployeeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Employees_DocumentNumber] ON [Employees] ([DocumentNumber]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261027013604_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261027013604_InitialCreate', N'8.0.8');
END;
GO

COMMIT;
GO

