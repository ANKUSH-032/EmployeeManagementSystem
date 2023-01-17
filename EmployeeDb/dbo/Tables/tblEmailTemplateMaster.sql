CREATE TABLE [dbo].[tblEmailTemplateMaster] (
    [TemplateID]   INT           IDENTITY (1, 1) NOT NULL,
    [TemplateName] VARCHAR (50)  NOT NULL,
    [Subject]      VARCHAR (MAX) NOT NULL,
    [Content]      VARCHAR (MAX) NOT NULL,
    [CreatedOn]    DATETIME      CONSTRAINT [DF_tblEmailTemplateMaster_CreatedOn] DEFAULT (getutcdate()) NULL,
    [CreatedBy]    VARCHAR (50)  NULL,
    [UpdatedOn]    DATETIME      CONSTRAINT [DF_tblEmailTemplateMaster_UpdatedOn] DEFAULT (getutcdate()) NULL,
    [UpdatedBy]    VARCHAR (50)  NULL,
    [IsActive]     BIT           CONSTRAINT [DF_tblEmailTemplateMaster_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_tblEmailTemplateMaster] PRIMARY KEY CLUSTERED ([TemplateID] ASC)
);

