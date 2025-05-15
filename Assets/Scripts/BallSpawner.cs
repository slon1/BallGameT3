using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class BallSpawner {
	private readonly Pool<ViewComponent> pool;
	private readonly DataStorage storage;
	public event Action<ViewComponent> OnDestroy;	
	public BallSpawner(Pool<ViewComponent> pool, DataStorage storage) {
		this.pool = pool ?? throw new ArgumentNullException(nameof(pool));
		this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
	}

	public async UniTask SpawnBalls(int count, float delaySeconds) {
		for (int i = 0; i < count; i++) {
			var item = pool.GetObject();
			BallType type = BallTypeUtils.GetRandomBallType();
			item.Init(new SpriteInfo(type, storage.GetSprite(type)));
			item.MoveTo(new Vector3(UnityEngine.Random.Range(-1f, 1f), 3f, 0));
			item.OnClick += obj => OnDestroy?.Invoke(obj);
			await UniTask.Delay(TimeSpan.FromSeconds(delaySeconds));
		}
	}
}