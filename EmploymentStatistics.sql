USE [Company]
GO

/****** Object:  StoredProcedure [dbo].[EmploymentStatistics]    Script Date: 03.05.2022 9:28:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create Procedure [dbo].[EmploymentStatistics]
@startDate		DateTime2 = Null,
@endDate		DateTime2 = Null
as
Declare @script NVARCHAR(MAX) = '
Select Count(Distinct persons.id) as CountPersons, Persons.DateEmploy
From Persons
Where Persons.StatusId = 1 and Persons.DateEmploy Between @startDate AND @endDate
Group by Persons.DateEmploy'

Declare @params NVARCHAR(MAX) = '
@startDate		DateTime2,
@endDate		DateTime2'

EXEC sp_executesql @script, @params,
@startDate,
@endDate
GO

