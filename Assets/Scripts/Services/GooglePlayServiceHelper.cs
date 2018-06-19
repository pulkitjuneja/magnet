using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GooglePlayServiceHelper {

    private static GooglePlayServiceHelper _instance;

    public bool isAuthenticated;

    public static GooglePlayServiceHelper instance {

        get {
            if (_instance == null) {
                _instance = new GooglePlayServiceHelper ();
            }
            return _instance;
        }

    }

    private GooglePlayServiceHelper () {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ().Build ();
        PlayGamesPlatform.InitializeInstance (config);
        PlayGamesPlatform.Activate ();
    }

    public void SignIn () {
        Social.localUser.Authenticate ((success) => {
            if (success) {
                isAuthenticated = true;
                setLatestScores ();
            }
        });
    }

    void setLatestScores () {
        PlayGamesPlatform.Instance.LoadScores (GPGSIds.leaderboard_magnetic,
            LeaderboardStart.PlayerCentered,
            1,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
            (LeaderboardScoreData data) => {
                int storedScore = PlayerPrefs.HasKey (FSMgenerator.SCORE_KEY) ? PlayerPrefs.GetInt (FSMgenerator.SCORE_KEY) : 0;
                if (data.PlayerScore.value > storedScore) {
                    PlayerPrefs.SetInt (FSMgenerator.SCORE_KEY, (int) data.PlayerScore.value);
                } else {
                    AddScoreToLeaderboard (GPGSIds.leaderboard_magnetic, storedScore);
                }
            });
    }

    public void AddScoreToLeaderboard (string leaderBoardID, long score) {
        Social.ReportScore (score, leaderBoardID, (success) => { });
    }

    public void ShowLeaderboard () {
        checkAuthentication ();
        Social.ShowLeaderboardUI ();
    }

    public void checkAuthentication () {
        if (!isAuthenticated) {
            SignIn ();
        }
    }
}