using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Buttons : MonoBehaviour {


	void Start () {

        Button[] buttons = GetComponentsInChildren<Button>();
        foreach(Button butt in buttons)
        {
            switch(butt.gameObject.name)
            {
                case "NewGame": butt.onClick.AddListener(() => NewGameListener()); break;
                case "Exit": butt.onClick.AddListener(() => ExitListener()); break;
                case "Pause": butt.onClick.AddListener(() => PauseListener()); break;
                case "Resume": butt.onClick.AddListener(() => ResumeListener()); break;
                case "ExitToMenu": butt.onClick.AddListener(() => ExitToMenu()); break;
                 
            }
        }
	}

    void NewGameListener()
    {
        MainStateMachine.instance.SetState(typeof(GamePlay), false, new object[]{MainStateMachine.instance});
    }

    void ExitListener()
    {
        Application.Quit();
    }

    void PauseListener()
    {
        (MainStateMachine.instance.Current as GamePlay).onPause();
    }

    void ResumeListener()
    {
        (MainStateMachine.instance.Current as GamePlay).onResume();
    }

    void ExitToMenu()
    {
        MainStateMachine.instance.SetState(typeof(MenuState), false, new object[]{MainStateMachine.instance});
    }

}
