using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class GooglePlayManager : MonoBehaviour
{
    public string Token;
    public string Error;
 
    void Awake()
    {
        DontDestroyOnLoad(this);
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        Debug.Log(status);
        if (status == SignInStatus.Success)
        {
            Debug.Log("Login with Google Play games successful.");
            PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
            {
                Debug.Log("Authorization code: " + code);
                Token = code;
            });
        }
        else
        {
            Error = "Failed to retrieve Google play games authorization code";
            Debug.Log("Login Unsuccessful");
        }
    }
}