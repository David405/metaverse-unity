using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Unity;
using Moralis;
using Assets;
using MoralisWeb3ApiSdk;
using Moralis.WebGL.Hex.HexTypes;
using System.Numerics;
using System.Threading.Tasks;
using Moralis.Platform;
using Moralis.Platform.Objects;
using Moralis.Web3Api.Models;
public class MainMenuScript : MonoBehaviour
{
    public MoralisController moralisController;
    public GameObject AuthenticationButton;
    public WalletConnect walletConnect;
    public GameObject qrMenu;
    public GameObject androidMenu;
    public GameObject iosMenu;
    public GameObject joystick;

    Image menuBackground;

    void OnApplicationQuit()
    {
        walletConnect.CLearSession();
    }
    async void Start()
    {
        menuBackground = (Image)gameObject.GetComponent(typeof(Image));

        qrMenu.SetActive(false);
        androidMenu.SetActive(false);
        iosMenu.SetActive(false);

#if UNITY_ANDROID || UNITY_IOS
        joystick.SetActive(true);
#endif

        // await MoralisInterface.Initialize(MoralisApplicationId, MoralisServerURI, hostManifestData);
        if (moralisController != null && moralisController)
        {
            await moralisController.Initialize();
        }
        else
        {
          Debug.LogError("The MoralisInterface has not been set up, please check you MoralisController in the scene.");
        }

         if (!MoralisInterface.IsLoggedIn())
        {
            AuthenticationButtonOn();
        }
    }
    public async void Play()
    {
        AuthenticationButtonOff();

        if (MoralisInterface.IsLoggedIn())
        {
            Debug.Log("User is already logged in to Moralis.");
        }
         else
        {
            Debug.Log("User is not logged in.");
            //mainMenu.SetActive(false);

#if UNITY_ANDROID
            // Use Wallet Connect.
            androidMenu.SetActive(true);

#elif UNITY_IOS
            // Use Wallet Connect.
            iosMenu.SetActive(true);

            //await LoginViaConnectionPage();
#else
            qrMenu.SetActive(true);
#endif
        }
    }

    public async void WalletConnectHandler(WCSessionData data)
    {
        Debug.Log("Wallet connection received");
        // Extract wallet address from the Wallet Connect Session data object.
        string address = data.accounts[0].ToLower();
        string appId = MoralisInterface.GetClient().ApplicationId;
        long serverTime = 0;

        // Retrieve server time from Moralis Server for message signature
        Dictionary<string, object> serverTimeResponse = await MoralisInterface.GetClient().Cloud.RunAsync<Dictionary<string, object>>("getServerTime", new Dictionary<string, object>());

        if (serverTimeResponse == null || !serverTimeResponse.ContainsKey("dateTime") ||
            !long.TryParse(serverTimeResponse["dateTime"].ToString(), out serverTime))
        {
            Debug.Log("Failed to retrieve server time from Moralis Server!");
        }

        string signMessage = $"Moralis Authentication\n\nId: {appId}:{serverTime}";

        Debug.Log($"Sending sign request for {address} ...");

        string response = await walletConnect.Session.EthPersonalSign(address, signMessage);

        Debug.Log($"Signature {response} for {address} was returned.");

        // Create moralis auth data from message signing response.
        Dictionary<string, object> authData = new Dictionary<string, object> { { "id", address }, { "signature", response }, { "data", signMessage } }; 

        Debug.Log("Logging in user.");

        // Attempt to login user.
        MoralisUser user = await MoralisInterface.LogInAsync(authData);

        if (user != null)
        {
            Debug.Log($"User {user.username} logged in successfully. ");
        }
        else
        {
            Debug.Log("User login failed.");
        }

        HideWalletSelection();
    }

    //Logout or quit game
    public async void Quit()
    {
        Debug.Log("QUIT");
         await walletConnect.Session.Disconnect();
         walletConnect.CLearSession();
        await MoralisInterface.LogOutAsync();
         Application.Quit();
    }

    public void HideWalletSelection()
    {
#if UNITY_ANDROID
        androidMenu.SetActive(false);
#elif UNITY_IOS
        iosMenu.SetActive(false);
#endif
    }

    private void AuthenticationButtonOff()
    {
        AuthenticationButton.SetActive(false);
        Color color = menuBackground.color;
        color = new Color(0f, 0f, 0f, 0f);
        menuBackground.color = color;
    }

    private void AuthenticationButtonOn()
    {
        AuthenticationButton.SetActive(true);
        Color color = menuBackground.color;
        color = new Color(0f, 0f, 0f, 0.7f);
        menuBackground.color = color;
    }
}
