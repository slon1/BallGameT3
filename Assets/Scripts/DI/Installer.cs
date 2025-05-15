using System;
using System.Collections;
using UnityEngine;
public class Installer : MonoBehaviour {

	private static DIContainer container;


	void Awake() {
		container = new DIContainer();
		container.Register(GetComponent<DataStorage>());		
		container.Register(GetComponent<SlotView>());
		container.Register(new GameLogic(7));
	}

	public static T GetService<T>() => container.Resolve<T>();
	private void OnDestroy() {
		container.Dispose();
	}
}
