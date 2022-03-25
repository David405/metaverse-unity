using UnityEngine;
using UnityEngine.UI;
using MoralisWeb3ApiSdk;
using Moralis.Platform.Objects;
using Moralis.Web3Api.Models;
public class WalletBalanceController : MonoBehaviour
{
    public Text addressText;
    public Text balanceText;
    public int ChainId;

    // Update is called once per frame
    public async void PopulateBalanceValues()
    {
       MoralisUser user = await MoralisInterface.GetUserAsync();

        if (user != null)
        {
            string addr = user.authData["moralisEth"]["id"].ToString();

            addressText.text = addr;

            // Retrieve account balanace.
            NativeBalance bal =
                await MoralisInterface.GetClient().Web3Api.Account.GetNativeBalance(addr.ToLower(),
                                            (ChainList)ChainId);

            double balance = 0.0;
            
            if (bal != null && !string.IsNullOrWhiteSpace(bal.Balance))
            {
                double.TryParse(bal.Balance, out balance);
            }

         balanceText.text = string.Format("{0:0.##} ETH", balance / (double)Mathf.Pow(10.0f, 18.0f));
        }
        else
        {
            balanceText.text = "0";
        }
    }
}
