USE [Company]
GO

/****** Object:  StoredProcedure [dbo].[EmployeesSearch]    Script Date: 03.05.2022 9:28:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[EmployeesSearch]
@statusNumbers     INT = NULL,
@departmentNumbers INT = NULL,
@postNumber        INT = NULL,
@lastName          NVARCHAR(MAX) = NULL
AS
DECLARE @script NVARCHAR(MAX) = '
SELECT  Persons.Id,
        Persons.LastName,
        Persons.FirstName,
        Persons.SecondName,
        Statuses.Id,
        Statuses.Name AS Status,
        Deps.Id,
        Deps.Name AS Department,
        Posts.Id,
        Posts.Name AS Post,
        Persons.DateEmploy AS Employ,
        Persons.DateUnemploy AS Unemploy
FROM dbo.Persons
JOIN dbo.Statuses ON Statuses.Id = Persons.StatusId
JOIN dbo.Deps ON Deps.Id = Persons.DepId
JOIN dbo.Posts ON Posts.Id = Persons.PostId 
WHERE 1 = 1'

+ CASE WHEN @statusNumbers       IS NOT NULL THEN ' AND Persons.StatusId = @statusNumbers' ELSE '' END
+ CASE WHEN @departmentNumbers   IS NOT NULL THEN ' AND Persons.DepId = @departmentNumbers' ELSE '' END
+ CASE WHEN @postNumber          IS NOT NULL THEN ' AND Persons.PostId = @postNumber' ELSE '' END
+ CASE WHEN @lastName            IS NOT NULL THEN ' AND Persons.LastName LIKE @lastName' ELSE '' END

DECLARE @params NVARCHAR(MAX) = '
@statusNumbers     INT,
@departmentNumbers INT,
@postNumber        INT,
@lastName          NVARCHAR(100)'

EXEC sp_executesql @script, @params,
@statusNumbers,
@departmentNumbers,
@postNumber,
@lastName
GO

