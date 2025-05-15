using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SpriteInfo {
	public BallType type;
	public Sprite sprite;
	public SpriteInfo(BallType type, Sprite sprite) {
		this.type = type;
		this.sprite = sprite;
	}
}

public class DataStorage : MonoBehaviour {
	[SerializeField] private List<SpriteInfo> spriteStorage;
	private Dictionary<BallType, Sprite> dict;

	private void Start() {
		dict = spriteStorage.ToDictionary(x => x.type, x => x.sprite);
	}

	public Sprite GetSprite(BallType type) =>
		dict.TryGetValue(type, out var sprite) ? sprite : null;
}
