CREATE FUNCTION HaversineDistance(@Latitude1 float,@Longitude1 float,@Latitude2 float,@Longitude2 float, @DistanceUnit smallint)
	RETURNS FLOAT
	AS 
	BEGIN
	-- CONSTANTS
	DECLARE @EarthRadiusInMiles FLOAT=(CASE @DistanceUnit WHEN 1 THEN 3959 WHEN 0 THEN 6371 ELSE 0 END);
	DECLARE @PI FLOAT=PI();
	DECLARE @lat1Radians FLOAT=@Latitude1 * @PI / 180
	DECLARE @long1Radians FLOAT=@Longitude1 * @PI / 180;
	DECLARE @lat2Radians FLOAT=@Latitude2 * @PI / 180;
	DECLARE @long2Radians FLOAT=@Longitude2 * @PI / 180;
	RETURN Acos(
	Cos(@lat1Radians)*Cos(@long1Radians)*Cos(@lat2Radians)*Cos(@long2Radians)+
	Cos(@lat1Radians)*Sin(@long1Radians)*Cos(@lat2Radians)*Sin(@long2Radians)+
	Sin(@lat1Radians)*Sin(@lat2Radians)
	) * @EarthRadiusInMiles;
END