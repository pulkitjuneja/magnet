using UnityEngine;

public abstract class SharedVariable<T> : ScriptableObject {
  public T value;

  public T GetValue() {
    return value;
  }

  public void SetValue(T input) {
    value = input;
  }
}

[CreateAssetMenu(fileName = "new float", menuName = "SharedVariables/float")]
public class FloatValue : SharedVariable<float> {
}