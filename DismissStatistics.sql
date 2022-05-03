USE [Company]
GO

/****** Object:  StoredProcedure [dbo].[DismissStatistics]    Script Date: 03.05.2022 9:27:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[DismissStatistics]
@startDate		DateTime2 = Null,
@endDate		DateTime2 = Null
as
Declare @script NVARCHAR(MAX) = '
Select Count(Distinct persons.id) as CountPersons, Persons.DateUnemploy
From Persons
Where Persons.StatusId = 2 and Persons.DateUnemploy Between @startDate AND @endDate
Group by Persons.DateUnemploy'

Declare @params NVARCHAR(MAX) = '
@startDate		DateTime2,
@endDate		DateTime2'

EXEC sp_executesql @script, @params,
@startDate,
@endDate
GO

