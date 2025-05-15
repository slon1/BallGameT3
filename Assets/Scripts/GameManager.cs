using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	[SerializeField] private GameObject prefab;
	[SerializeField] private Button refresh;
	private Pool<ViewComponent> pool;
	private GameLogic gameLogic;
	private SlotView slotView;
	private BallSpawner ballSpawner;

	private void Awake() {
		if (prefab == null || prefab.GetComponent<ViewComponent>() == null) {
			Debug.LogError("Prefab or ViewComponent is missing.");
			return;
		}
		pool = new Pool<ViewComponent>(prefab.GetComponent<ViewComponent>());
	}

	private void Start() {
		slotView = Installer.GetService<SlotView>();
		var storage = Installer.GetService<DataStorage>();
		gameLogic = Installer.GetService<GameLogic>();

		if (slotView == null || storage == null || gameLogic == null) {
			Debug.LogError("Failed to resolve dependencies.");
			return;
		}

		slotView.Init(gameLogic, storage);
		gameLogic.OnChainRemoved += GameLogic_OnChainRemoved;
		gameLogic.OnSlotsFull += GameLogic_OnSlotsFull;

		ballSpawner = new BallSpawner(pool, storage);
		ballSpawner.OnDestroy += Item_OnClick;
		ballSpawner.SpawnBalls(30, 0.1f).AttachExternalCancellation(destroyCancellationToken).Forget();
		refresh.onClick.AddListener(()=>Restart());
	}

	

	private void GameLogic_OnSlotsFull() {
		Restart().Forget(); 
	}

	private void GameLogic_OnChainRemoved(BallType type) {		
		Debug.Log($"Chain of {type} removed.");
	}

	public void Item_OnClick(ViewComponent obj) {
		if (obj == null) return;

		bool added = slotView.TryDisplayBall(obj);
		if (added) {
			obj.OnClick -= Item_OnClick;
			pool.ReturnObject(obj);
			
		}
	}

	private async UniTask Restart() {
		await UniTask.Delay(TimeSpan.FromSeconds(1));
		slotView.Clear();

		foreach (var obj in pool.GetActiveObjects().ToList()) {
			obj.OnClick -= Item_OnClick;
			pool.ReturnObject(obj);
		}
		gameLogic.ClearAll();
		await ballSpawner.SpawnBalls(30, 0.1f);
	}

	private void OnDestroy() {
		gameLogic.OnChainRemoved -= GameLogic_OnChainRemoved;
		gameLogic.OnSlotsFull -= GameLogic_OnSlotsFull;
		
		ballSpawner.OnDestroy -= Item_OnClick;
		foreach (var obj in pool.GetActiveObjects()) {
			obj.OnClick -= Item_OnClick;
		}		
		
		pool?.Dispose();
	}
}