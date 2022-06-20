using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;

public class PlayfabAuthController : MonoBehaviour
{
    #region ----Fields----
    [SerializeField] private TMP_InputField userName;
    [SerializeField] private TMP_InputField password;
    #endregion ----Fields----

    #region ----Methods----	
    public void Awake()
    {
        MobileAuthentication();
    }

    #region 1. Mobile anonym auth
    public void MobileAuthentication()
    {
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, (lol) => { }, (lol2) => { });
    }

    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }
    #endregion 1. Mobile anonym auth

    #region 2. Link Account
    public void LinkAccount()
    {
        var LinkAccountRequest = new AddUsernamePasswordRequest
        {
            Username = userName.text,
            Password = password.text
        };
        PlayFabClientAPI.AddUsernamePassword(LinkAccountRequest, (lol) => { }, (lol) => { });
    }
    #endregion 2. Link Account
    #endregion ----Methods----	
}
