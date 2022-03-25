using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

using UnityEngine.Networking;
using System.Text;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using MoralisWeb3ApiSdk;

public class TokenListController : MonoBehaviour
{
    public GameObject ListItemPrefab;
    public Transform TokenListTransform;
    public int ChainId;
    private bool tokensLoaded;
    public async void PopulateWallet()
    {
        if (!tokensLoaded)
        {
            tokensLoaded = true;

            MoralisUser user = await MoralisInterface.GetUserAsync();
            List<Erc20TokenBalance> tokens = await LoadTokens(user);

            StartCoroutine( BuildTokenList(tokens));
            
        }
    }
    private async Task<List<Erc20TokenBalance>> LoadTokens(MoralisUser user)
    {
        List<Erc20TokenBalance> tokens = new List<Erc20TokenBalance>();

        if (user != null)
        {
            string addr = user.authData["moralisEth"]["id"].ToString();

            tokens = await MoralisInterface.GetClient().Web3Api.Account.GetTokenBalances(addr.ToLower(),
                                            (ChainList)ChainId);
        }

        return tokens;
    }

    IEnumerator BuildTokenList(List<Erc20TokenBalance> tokens)
    {
        if (tokens.Count > 0)
        {
            foreach (Erc20TokenBalance token in tokens)
            {
                if (string.IsNullOrWhiteSpace(token.Symbol))
                {
                    continue;
                }

                var tokenObj = Instantiate(ListItemPrefab, TokenListTransform);
                var tokenSymbol = tokenObj.GetFirstChildComponentByName<Text>("TokenSymbolText", false);
                var tokenBalanace = tokenObj.GetFirstChildComponentByName<Text>("TokenCountText", false);
                var tokenImage = tokenObj.GetFirstChildComponentByName<Image>("TokenThumbNail", false);
                var tokenButton = tokenObj.GetComponent<Button>();
                var rectTransform = tokenObj.GetComponent<RectTransform>();

                var parentTransform = TokenListTransform.GetComponent<RectTransform>();
                double balance = 0.0;
                float tokenDecimals = 18.0f;

                if (token != null && !string.IsNullOrWhiteSpace(token.Balance))
                {
                    double.TryParse(token.Balance, out balance);
                    float.TryParse(token.Decimals, out tokenDecimals);
                }

                tokenSymbol.text = token.Symbol;
                tokenBalanace.text = string.Format("{0:0.##} ", balance / (double)Mathf.Pow(10.0f, tokenDecimals));

                tokenButton.onClick.AddListener(delegate
                {
                  Application.OpenURL($"https://coinmarketcap.com/currencies/{token.Name}");
                });

                using (UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(token.Thumbnail))
                {
                    yield return imageRequest.SendWebRequest();

                    if (imageRequest.isNetworkError)
                    {
                        Debug.Log("Error Getting Nft Image: " + imageRequest.error);
                    }
                    else
                    {
                        Texture2D tokenTexture = ((DownloadHandlerTexture)imageRequest.downloadHandler).texture;

                        var sprite = Sprite.Create(tokenTexture,
                                    new Rect(0.0f, 0.0f, tokenTexture.width, tokenTexture.height),
                                    new Vector2(0.75f, 0.75f), 100.0f);

                        tokenImage.sprite = sprite;
                    }
                }
            }
        }
    }
}
