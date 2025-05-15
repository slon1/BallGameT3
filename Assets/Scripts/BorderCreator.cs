using UnityEngine;

public class BorderCreator : MonoBehaviour {
	[SerializeField] private float thickness = 1f;
	[SerializeField] private float cornerHeightRatio = 0.5f;

	private void Start() {
		CreateBorders();
	}

	private void CreateBorders() {
		Camera cam = Camera.main;
		if (cam == null) {
			Debug.LogError("Main Camera not found.");
			return;
		}

		Vector2 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0));
		Vector2 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1));

		float width = topRight.x - bottomLeft.x;
		float height = topRight.y - bottomLeft.y;

		float centerX = (topRight.x + bottomLeft.x) / 2;

		
		CreateBorder(
			new Vector2(centerX, bottomLeft.y - thickness / 2),
			new Vector2(width, thickness),
			"BottomBorder"
		);

		float cornerHeight = height * cornerHeightRatio;

		
		CreateBorder(
			new Vector2(bottomLeft.x - thickness / 2, bottomLeft.y + cornerHeight / 2),
			new Vector2(thickness, cornerHeight),
			"LeftCornerBorder"
		);

		
		CreateBorder(
			new Vector2(topRight.x + thickness / 2, bottomLeft.y + cornerHeight / 2),
			new Vector2(thickness, cornerHeight),
			"RightCornerBorder"
		);
	}

	private void CreateBorder(Vector2 position, Vector2 size, string name) {
		GameObject border = new GameObject(name);
		border.transform.position = position;
		BoxCollider2D collider = border.AddComponent<BoxCollider2D>();
		collider.size = size;
	}
}
