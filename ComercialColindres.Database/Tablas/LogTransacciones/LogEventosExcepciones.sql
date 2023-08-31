CREATE TABLE [dbo].[LogEventosExcepciones]
(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[EventType] [varchar](50) NOT NULL,
	[Server] [varchar](50) NULL,
	[ModuleName] [varchar](50) NOT NULL,
	[TypeFullName] [varchar](max) NULL,
	[MethodName] [varchar](max) NULL,
	[LineNumber] [int] NOT NULL CONSTRAINT [DF_LogEventosExcepciones_FileLineNumber]  DEFAULT ((0)),
	[ExceptionType] [varchar](max) NULL,
	[ExceptionMessage] [varchar](max) NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Date] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
