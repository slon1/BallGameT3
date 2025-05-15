using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool<T> where T : Component {
	private readonly ObjectPool<T> pool;
	private readonly List<T> activeObjects = new();

	public Pool(T prefab, Transform parent = null, int capacity = 10, int maxSize = 100) {
		if (prefab == null) throw new ArgumentNullException(nameof(prefab));

		var poolParent = parent ?? new GameObject($"{typeof(T).Name}Pool").transform;

		pool = new ObjectPool<T>(
			() => UnityEngine.Object.Instantiate(prefab, poolParent),
			obj => {
				obj.gameObject.SetActive(true);
				activeObjects.Add(obj);
			},
			obj => {
				obj.gameObject.SetActive(false);
				activeObjects.Remove(obj);
			},
			obj => UnityEngine.Object.Destroy(obj.gameObject),
			true, capacity, maxSize
		);
	}

	public T GetObject() => pool.Get();

	public void ReturnObject(T obj) { 
		pool.Release(obj);
		activeObjects.Remove(obj);
	}

	public void ReturnAllObjects() {
		foreach (var obj in activeObjects.ToArray())
			pool.Release(obj);

	}

	public IReadOnlyList<T> GetActiveObjects() => activeObjects.AsReadOnly();

	public void Dispose() {

		//pool.Dispose();
		activeObjects.Clear();
	}
}
