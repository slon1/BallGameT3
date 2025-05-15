using System;
using System.Collections.Generic;

public class GameLogic {
	private readonly SlotManager slotManager;

	public event Action<BallType> OnChainRemoved;
	public event Action OnSlotsFull;

	public GameLogic(int slotCount) {
		
		slotManager = new SlotManager(slotCount);
	}

	public bool TryAddBall(BallType ballType) {
		if (!slotManager.AddBall(ballType))
			return false;

		BallType matched = slotManager.CheckLastThree();
		if (matched != BallType.Empty) {
			slotManager.ClearLast3();
			OnChainRemoved?.Invoke(matched);
		}
		else if (slotManager.IsFull()) {
			OnSlotsFull?.Invoke();
		}

		return true;
	}

	public IReadOnlyList<BallType> GetSlots() => slotManager.GetSlotsState();

	
	public void ClearAll() => slotManager.ClearAllSlots();
}