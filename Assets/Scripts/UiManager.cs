using UnityEngine;
using System.Collections;
using UnityEngine.UI;

class UiManager : MonoBehaviour {
    bool isSfxToggled = true;
    bool isMusicToggled = true;

    public Sprite sfxOn, sfxOff, musicOn, musicOff;
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
                case "toggleMusic": butt.onClick.AddListener(()=> toggleMusic(butt)); break;
                case "toggleSfx": butt.onClick.AddListener(()=> toggleSfx(butt)); break;
            }
        }
	}

    public void NewGameListener()
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
        Time.timeScale = 1.0f;
        MainStateMachine.instance.SetState(typeof(MenuState), false, new object[]{MainStateMachine.instance});
    }

    void toggleMusic(Button butt) {
        isMusicToggled = !isMusicToggled;
        AudioManager.toggleMusic(isMusicToggled);
        if(isMusicToggled){
            butt.gameObject.GetComponent<Image>().overrideSprite = musicOn;
        } else {
             butt.gameObject.GetComponent<Image>().overrideSprite = musicOff;
        }
    }

    void toggleSfx(Button butt) {
         isSfxToggled = !isSfxToggled;
        AudioManager.toggleSFX(isSfxToggled);
        if(isSfxToggled){
            butt.gameObject.GetComponent<Image>().overrideSprite = sfxOn;
        } else {
             butt.gameObject.GetComponent<Image>().overrideSprite = sfxOff;
        }
    }

}
