using System;
using UnityEngine;

public class ViewComponent : MonoBehaviour {
	public event Action<ViewComponent> OnClick;

	private BallType ballType;
	public BallType BallType => ballType;

	public void Init(SpriteInfo info) {		
		ballType = info.type;
		var renderer = GetComponent<SpriteRenderer>();		
		renderer.sprite = info.sprite;
	}
	private void OnDisable() {
		OnClick = null;
	}
	public void MoveTo(Vector3 targetPos) {
		transform.position = targetPos;
	}

	public void Reset() {
		OnClick = null;
	}

	private void OnMouseDown() {
		OnClick?.Invoke(this);
	}
}