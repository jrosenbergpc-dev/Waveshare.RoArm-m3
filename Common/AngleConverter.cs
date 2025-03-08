using System;

public static class AngleConverter
{
	public static double DegreesToRadians(double degrees)
	{
		return degrees * (Math.PI / 180);
	}
}