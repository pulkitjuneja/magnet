using System;
using UnityEngine;
using System.Collections.Generic;

public class SignalData {
  Dictionary<string, System.Object> data = new Dictionary<string, System.Object>();


  public void set(string key, System.Object value) {
    if (!data.ContainsKey(key)) {
      data.Add(key, value);
    } else {
      Debug.Log(String.Format("Cannot Add key {0} already exists", key));
    }
  }

  public T get<T>(string key) {
    if (data.ContainsKey(key) && data[key] is T) {
      return (T)data[key];
    }
    return default(T);
  }
}