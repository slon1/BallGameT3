using System;
using System.Collections.Generic;
using UnityEngine;

public enum BallType {
	Empty,
	A,
	B,
	C,
	D,
	E
}

public class SlotManager {
	private readonly BallType[] slots;
	private int currentSlotIndex;

	public SlotManager(int slotCount) {
		
		slots = new BallType[slotCount];
		for (int i = 0; i < slots.Length; i++)
			slots[i] = BallType.Empty;
	}

	public bool AddBall(BallType ballType) {
		if (ballType == BallType.Empty || IsFull()) {
			Debug.Log("Cannot add Empty ball type or slots are full.");
			return false;
		}

		slots[currentSlotIndex++] = ballType;
		return true;
	}

	public void ClearSlot(int index) {
		if (index < 0 || index >= currentSlotIndex)
			return;

		Array.Copy(slots, index + 1, slots, index, currentSlotIndex - index - 1);
		slots[--currentSlotIndex] = BallType.Empty;
	}

	public void ClearLast3() {
		if (currentSlotIndex < 3) {
			Debug.Log("Not enough filled slots to clear last 3.");
			return;
		}

		for (int i = 0; i < 3; i++)
			ClearSlot(currentSlotIndex - 1);
	}

	public void ClearAllSlots() {
		Array.Clear(slots, 0, slots.Length);
		currentSlotIndex = 0;
	}

	public IReadOnlyList<BallType> GetSlotsState() => Array.AsReadOnly(slots);

	
	public bool IsFull() => currentSlotIndex >= slots.Length;

	public BallType CheckLastThree() {
		if (currentSlotIndex < 3)
			return BallType.Empty;

		BallType lastType = slots[currentSlotIndex - 1];
		if (slots[currentSlotIndex - 2] == lastType &&
			slots[currentSlotIndex - 3] == lastType)
			return lastType;

		return BallType.Empty;
	}
}