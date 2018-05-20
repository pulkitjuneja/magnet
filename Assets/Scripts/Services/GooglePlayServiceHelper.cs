using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GooglePlayServiceHelper {

    private static GooglePlayServiceHelper _instance;
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
        Social.localUser.Authenticate ((success) => { });
    }

    public void AddScoreToLeaderboard (string leaderBoardID, long score) {
        Social.ReportScore (score, leaderBoardID, (success) => { });
    }

    public void ShowLeaderboard () {
        Social.ShowLeaderboardUI ();
    }
}