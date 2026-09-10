USE [Escape]
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'alumno')
BEGIN
    CREATE USER [alumno] FOR LOGIN [alumno] WITH DEFAULT_SCHEMA=[dbo]
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- Salas: guarda el código correcto de cada nivel (OrdenSecuencial = 1..5)
-- ============================================================
CREATE TABLE [dbo].[Salas](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo_Sala] [varchar](50) NULL,
	[OrdenSecuencial] [int] NULL,
	[Nombre_Usuario] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- ============================================================
-- Partidas: una fila por jugador. Ojo: Estado es VARCHAR porque
-- BD.cs guarda los textos "En curso" / "Finalizado", no un int.
-- ============================================================
CREATE TABLE [dbo].[Partidas](
	[Nombre_Usuario] [varchar](50) NOT NULL,
	[Fecha] [date] NULL,
	[Hora] [time](0) NULL,
	[ID_Jugador] [int] NULL,
	[Estado] [varchar](20) NULL,
	[Puntuacion] [decimal](18, 5) NULL,
	[Sala_Actual] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Nombre_Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- ============================================================
-- Respuestas: log de cada intento (correcto o no) por sala
-- ============================================================
CREATE TABLE [dbo].[Respuestas](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre_Usuario] [varchar](50) NULL,
	[Id_Sala] [int] NULL,
	[ValorRespuesta] [bit] NULL,
	[FechaHora] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- ============================================================
-- Foreign keys: solo las que corresponden a columnas que existen de verdad.
-- (Se sacaron FK_Respuestas_Partidas y FK_Salas_Partidas: apuntaban a
-- columnas que nunca se crearon - ID_Partida, Partidas.ID, Salas.PartidaId)
-- ============================================================
ALTER TABLE [dbo].[Respuestas]  WITH CHECK ADD  CONSTRAINT [FK_Respuestas_Partidas] FOREIGN KEY([Nombre_Usuario])
REFERENCES [dbo].[Partidas] ([Nombre_Usuario])
GO
ALTER TABLE [dbo].[Respuestas] CHECK CONSTRAINT [FK_Respuestas_Partidas]
GO
ALTER TABLE [dbo].[Respuestas]  WITH CHECK ADD  CONSTRAINT [FK_Respuestas_Salas] FOREIGN KEY([Id_Sala])
REFERENCES [dbo].[Salas] ([ID])
GO
ALTER TABLE [dbo].[Respuestas] CHECK CONSTRAINT [FK_Respuestas_Salas]
GO

-- ============================================================
-- Códigos de cada sala. Cambiá el valor cuando definas la respuesta
-- real del iframe de educaplay (sala 3) y del juego de monedas (sala 5).
-- ============================================================
INSERT INTO [dbo].[Salas] (Codigo_Sala, OrdenSecuencial) VALUES
('DORADO',   1),  -- Nivel 1: nombre de la ciudad perdida
('KILDARE',  2),  -- Nivel 2: letras del cartel del surf shop
('CAMBIAR',  3),  -- Nivel 3: respuesta del juego de educaplay -> reemplazar
('POGUE',    4),  -- Nivel 4: se revela al superar el rosco
('CAMBIAR',  5)   -- Nivel 5: respuesta del juego de monedas -> reemplazar
GO