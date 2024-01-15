
--- Unit is (K) Kilometers or (M) Miles
CREATE FUNCTION [dbo].[udf_Haversine](@lat1 float, @long1 float, @lat2 float, @long2 float, @Unit char) RETURNS float 
BEGIN
    DECLARE @dlon float, @dlat float,
            @rlat1 float, @rlat2 float, @rlong1 float, @rlong2 float,
            @a float, @c float, @R float, @d float, @DtoR float

    SELECT
        @DtoR = PI() / 180, -- Degrees to radians const
        @R = 6371 -- Radius of Earth in KM

    SELECT 
        @rlat1 = @lat1 * @DtoR,
        @rlong1 = @long1 * @DtoR,
        @rlat2 = @lat2 * @DtoR,
        @rlong2 = @long2 * @DtoR

    SELECT 
        @dlat = @rlat1 - @rlat2,
        @dlon = @rlong1 - @rlong2

    SELECT @a = SIN(@dlat / 2) * SIN(@dlat / 2) +
                SIN(@dlon / 2) * SIN(@dlon / 2) * COS(@rlat2) * COS(@rlat1)

    SELECT @c = 2 * atn2(sqrt(@a), sqrt(1 - @a))

    IF @Unit = 'K'
        SELECT @d = @R * @c -- Final distance in KM
    ELSE
        SELECT @d = @d * 0.621371192 -- Final distance in miles

RETURN @d 
END