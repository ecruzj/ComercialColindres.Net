CREATE TABLE [dbo].[UsuariosOpciones] (
    [UsuarioOpcionId] INT IDENTITY (1, 1) NOT NULL,
    [UsuarioId]       INT NOT NULL,
	[SucursalId] int NOT NULL,
    [OpcionId]        INT NOT NULL,
    CONSTRAINT [PK_UsuariosOpciones] PRIMARY KEY CLUSTERED ([UsuarioOpcionId] ASC),
    CONSTRAINT [FK_UsuariosOpciones_Opciones] FOREIGN KEY ([OpcionId]) REFERENCES [dbo].[Opciones] ([OpcionId]),
    CONSTRAINT [FK_UsuariosOpciones_Usuarios] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios] ([UsuarioId]), 
    CONSTRAINT [FK_UsuariosOpciones_Sucursales] FOREIGN KEY ([SucursalId]) REFERENCES [dbo].Sucursales(SucursalId)
);