using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverState : State<MainStateMachine> {

    Animator EndMenu;
    Text FinalScore,
    PersonalBest,
    PersonalBestText;

    Button ShareButton;

    int currentScore,
    personalBestScore;
    public GameOverState (MainStateMachine m, int Score) : base (m) {
        EndMenu = GameObject.Find ("Canvas").transform.Find ("End Menu").gameObject.GetComponent<Animator> ();
        FinalScore = EndMenu.transform.Find ("FinalScore").GetComponent<Text> ();
        PersonalBest = EndMenu.transform.Find ("PersonalBest").GetComponent<Text> ();
        PersonalBestText = EndMenu.transform.Find ("PersonalBestText").GetComponent<Text> ();
        ShareButton = EndMenu.transform.Find ("PersonalBest").GetComponent<Button> ();
        currentScore = Score;
    }

    void PrepareScreenForNewPersonalBest () {
        PersonalBestText.text = "New Personal Best";
        PersonalBest.text = "Share";
#if UNITY_ANDROID && !UNITY_EDITOR
        ShareButton.onClick.AddListener (shareScoreAndroid);
#endif
    }

    void PrepareScreenForNormalScore (int personalBestScore) {
        PersonalBestText.text = "Personal Best";
        PersonalBest.text = personalBestScore.ToString () + "m";
        ShareButton.onClick.RemoveAllListeners ();
    }

    void FetchScoreAndPrepareUI () {
        personalBestScore = PlayerPrefs.GetInt (FSMgenerator.SCORE_KEY);
        if (currentScore > personalBestScore) {
            PlayerPrefs.SetInt (FSMgenerator.SCORE_KEY, currentScore);
            GooglePlayServiceHelper.instance.AddScoreToLeaderboard (GPGSIds.leaderboard_magnetic, (long) currentScore);
            PlayerPrefs.Save ();
            PrepareScreenForNewPersonalBest ();
        } else {
            PrepareScreenForNormalScore (personalBestScore);
        }
        FinalScore.text = currentScore.ToString () + "m";
    }

    public override IEnumerator run () {
        EndMenu.SetBool ("visible", true);
        FetchScoreAndPrepareUI ();
        while (ParentMachine.Current.GetType () == GetType ()) {
            yield return null;
        }
        EndMenu.SetBool ("visible", false);
    }

    void shareScoreAndroid () {
        string score = currentScore.ToString ();
        AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
        intentObject.Call ("setAction", intentClass.GetStatic<String> ("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
        intentObject.Call ("putExtra", intentClass.GetStatic<String> ("EXTRA_SUBJECT"), "Magnetic");
        intentObject.Call ("putExtra", intentClass.GetStatic<String> ("EXTRA_TITLE"), "New Best Score");
        intentObject.Call ("putExtra", intentClass.GetStatic<String> ("EXTRA_TEXT"),
            "I just fell " + score + " meters in magnetic, Think you can beat it ? https://play.google.com/apps/testing/com.pipedreams.Magnet");
        AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
        currentActivity.Call ("startActivity", intentObject);
    }

}