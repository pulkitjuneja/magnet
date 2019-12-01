using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "My Assets/Game Signal")]

public class Signal : ScriptableObject {

  [SerializeField]
  List<Action<SignalData>> Listeners = new List<Action<SignalData>>();

  public void addListener(Action<SignalData> listener) {
    if (!Listeners.Contains(listener)) {
      Listeners.Add(listener);
    }
  }

  public void removeListener(Action<SignalData> listener) {
    if (Listeners.Contains(listener)) {
      Listeners.Remove(listener);
    }
  }

  public void fire(SignalData data) {
    foreach (Action<SignalData> listener in Listeners) {
      listener(data);
    }
  }

}
