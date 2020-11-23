CREATE TABLE [dbo].[RegistrosPontos]
(
    [Id] INT NOT NULL identity,
    [Data] DATE NULL,
    [Hora] DATETIME NULL,
    [NomeUsuario] NVARCHAR (50) NULL,
    [Tipo] NCHAR (10) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

