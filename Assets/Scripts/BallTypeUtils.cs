using System;

public static class BallTypeUtils {
	
	private static readonly BallType[] validBallTypes = {
		BallType.A, BallType.B, BallType.C, BallType.D, BallType.E
	};

	public static BallType GetRandomBallType() {
		int index = new Random ().Next(0, validBallTypes.Length);
		return validBallTypes[index];
	}
}