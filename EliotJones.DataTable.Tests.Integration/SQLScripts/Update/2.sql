CREATE PROCEDURE uspGetAllAreas
AS 
    SET NOCOUNT ON;
    SELECT	AreaId,
			Area_Name,
			IsActive,
			CreatedDate
	FROM	Area
GO