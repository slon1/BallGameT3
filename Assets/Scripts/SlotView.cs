using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SlotView : MonoBehaviour {
	[SerializeField] private Image[] slotImages;

	private GameLogic gameLogic;
	private DataStorage dataStorage;

	public void Init(GameLogic logic, DataStorage storage) {
		if (slotImages == null || slotImages.Length == 0) {
			Debug.LogError("SlotImages is not configured.");
			return;
		}
		gameLogic = logic ?? throw new ArgumentNullException(nameof(logic));
		dataStorage = storage ?? throw new ArgumentNullException(nameof(storage));
		UpdateView();
	}

	public bool TryDisplayBall(ViewComponent component) {
		if (component == null) {
			Debug.LogWarning("ViewComponent is null.");
			return false;
		}

		bool success = gameLogic.TryAddBall(component.BallType);
		if (success) {
			UpdateView();
		}
		return success;
	}

	private void UpdateView() {
		var slots = gameLogic.GetSlots();
		for (int i = 0; i < slotImages.Length; i++) {
			Sprite sprite = i < slots.Count && slots[i] != BallType.Empty
				? dataStorage.GetSprite(slots[i])
				: dataStorage.GetSprite(BallType.Empty);
			if (sprite == null) {
				Debug.LogError($"Sprite for {slots[i]} is missing.");
				continue;
			}
			slotImages[i].sprite = sprite;
		}
	}

	public void Clear() {
		gameLogic.ClearAll();
		UpdateView();
	}
}