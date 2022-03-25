using MoralisWeb3ApiSdk;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Moralis.Platform.Objects;
using Moralis.Web3Api.Models;
public class AwardableController : MonoBehaviour
{
    public string NftTokenId;
    public string AwardContractAddress;

    public bool isOwned = false;

    private bool isInitialized = false;
    private bool canBeClaimed = false;

    // Start is called before the first frame update
    async void Awake()
    {
       
    }
    
    // Update is called once per frame
    async void Update()
    {
        if (!isInitialized && MoralisInterface.Initialized && MoralisInterface.IsLoggedIn())
        {
            isInitialized = true;

            MoralisUser user = await MoralisInterface.GetUserAsync();

            string addr = user.authData["moralisEth"]["id"].ToString();

            try
            {
#if UNITY_WEBGL
                NftOwnerCollection noc =
                    await MoralisInterface.GetClient().Web3Api.Account.GetNFTsForContract(addr.ToLower(),
                    AwardContractAddress,
                    ChainList.mumbai);
#else
                NftOwnerCollection noc =
                    await MoralisInterface.GetClient().Web3Api.Account.GetNFTsForContract(addr.ToLower(),
                    AwardContractAddress,
                    ChainList.mumbai);
#endif
                IEnumerable<NftOwner> ownership = from n in noc.Result
                                                  where n.TokenId.Equals(NftTokenId.ToString())
                                                  select n;

                if (ownership != null && ownership.Count() > 0)
                {
                    Debug.Log("Already Owns Mug.");
                    isOwned = true;
                    transform.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Does not own Mug.");
                }
            }
            catch (Exception exp)
            {
                Debug.LogError(exp.Message);
            }
        }

       if (isInitialized && 
            canBeClaimed && 
            !isOwned &&
            Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var diff = UnityEngine.Vector3.Distance(hit.transform.position, transform.position);
              
                if (diff < 0.9f)
                {
                    await ClaimRewardAsync();
                }
            }
        }
    }

    private async UniTask ClaimRewardAsync()
    {
         if (isOwned) return;

        // Need the user for the wallet address
        MoralisUser user = await MoralisInterface.GetUserAsync();

        string addr = user.authData["moralisEth"]["id"].ToString();

        // Convert token id to integer
        BigInteger bi = 0;

        if (BigInteger.TryParse(NftTokenId, out bi))
        {

#if UNITY_WEBGL

            // Convert token id to hex as this is what the contract call expects
            object[] pars = new object[] { bi.ToString() };

            // Set gas estimate
            HexBigInteger gas = new HexBigInteger(0);
            string resp = await MoralisInterface.ExecuteFunction(Constants.MUG_CONTRACT_ADDRESS, Constants.MUG_ABI, Constants.MUG_CLAIM_FUNCTION, pars, new HexBigInteger("0x0"), gas, gas);
#else

            // Convert token id to hex as this is what the contract call expects
            object[] pars = new object[] { bi.ToString("x") };

            // Set gas estimate
            HexBigInteger gas = new HexBigInteger(0);
            // Call the contract to claim the NFT reward.
            string resp = await MoralisInterface.SendEvmTransactionAsync("Rewards", "mumbai", "claimReward", addr, gas, new HexBigInteger("0x0"), pars);
#endif
           
            transform.gameObject.SetActive(false);
        }
    }

    public void Display(UnityEngine.Vector3 vec3)
    {
        transform.Translate(vec3);
    }

    public void SetCanBeClaimed()
    {
        canBeClaimed = true;
    }
}
