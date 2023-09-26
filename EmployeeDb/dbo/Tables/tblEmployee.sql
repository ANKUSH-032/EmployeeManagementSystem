﻿CREATE TABLE [dbo].[tblEmployee] (
    [EmployeeId]        VARCHAR (50)  NOT NULL,
    [FirstName]         VARCHAR (50)  NOT NULL,
    [MiddleName]        VARCHAR (50)  NULL,
    [LastName]          VARCHAR (50)  NOT NULL,
    [DayofBirth]        DATETIME      NOT NULL,
    [EmailId]           VARCHAR (50)  NOT NULL,
    [District]          BIGINT        NOT NULL,
    [State]             BIGINT        NOT NULL,
    [PinCode]           BIGINT        NOT NULL,
    [Address]           VARCHAR (50)  NOT NULL,
    [Qualification]     BIGINT        NOT NULL,
    [CurrentExperience] BIGINT        NOT NULL,
    [JoinDate]          DATETIME      NOT NULL,
    [DesignationName]   BIGINT        NOT NULL,
    [Gender]            VARCHAR (10)  NOT NULL,
    [MaritalStatus]     VARCHAR (20)  NOT NULL,
    [CompanyName]       VARCHAR (50)  NOT NULL,
    [CompanyAddress]    VARCHAR (50)  NOT NULL,
    [FatherName]        VARCHAR (50)  NOT NULL,
    [FatherOccupation]  VARCHAR (50)  NOT NULL,
    [MontherName]       VARCHAR (50)  NOT NULL,
    [MontherOcupation]  VARCHAR (50)  NOT NULL,
    [IsDeleted]         BIT           NULL,
    [CreatedBy]         VARCHAR (50)  NULL,
    [CreatedOn]         DATETIME      NULL,
    [UpdatedBy]         VARCHAR (50)  NULL,
    [UpdatedOn]         DATETIME      NULL,
    [DeletedOn]         DATETIME      NULL,
    [DeletedBy]         VARCHAR (50)  NULL,
    [PhoneNumber]       BIGINT        DEFAULT ('0') NOT NULL,
    [HomePhoneNumber]   BIGINT        DEFAULT ('0') NOT NULL,
    [SignaturePath]     VARCHAR (500) NULL,
    [Photopath]         VARCHAR (500) NULL,
    CONSTRAINT [PK_tblEmployee] PRIMARY KEY CLUSTERED ([EmployeeId] ASC)
);





