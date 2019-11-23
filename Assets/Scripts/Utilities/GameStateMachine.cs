using UnityEngine;

public class GameStateMachine : StateMachine<object, GameStateMachine, GameStateManager> {
  public static GameStateMachine instance = null;
  private GameStateMachine(object parent, GameStateManager mono) : base(parent, mono) {
    AddState(typeof(MenuState));
    AddState(typeof(GamePlay));
    AddState(typeof(GameOverState));
    Current = new MenuState(this);
  }

  public static GameStateMachine GetNewInstance(object parent, GameStateManager mono) {
    instance = new GameStateMachine(parent, mono);
    return instance;
  }

  public override void TriggerExit2D(Collider2D other) {
    Current.TriggerExit2D(other);
  }

  public void OnGUI() {
    Current.OnGUI();
  }
}