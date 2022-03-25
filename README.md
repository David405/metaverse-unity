## About Project
Metaverse world is a simple android game that allows you to hunt for a special treasure hidden in a chest. The special treasure has been minted as an NFT. Along the way, you can pick up coins gifted to you.
- Connect your wallet.
- Use the joystick on the bottom left to move and navigate the camera.
- Move towards the chest, destroy the chest with your sword and touch the special
treasure to gain ownership of it (This requires a smart contract call to your wallet).


## Libraries and SDKs
- Unity 2021.2.16f1
- JsonDontNet
- MoralisWeb3 SDK
- WalletConnect SDK
- OpenZeppelin 

## Improvements
- Add multiplayer functionality.
- Add a script that will allow players to send themselves tokens either fungible or
non-fungible.
- Add a script to allow players sell their special treasure in-game and also in an open
marketplace such as opensea etc.
- Also, fix the bugs with walletconnect not getting user wallet information sometimes after
connecting.
- Fix the issue with signing transactions (This might be an android 12 specific issue).
- Add obstacles or bosses to the game.
- Improve the general experience and interface of the game.

## Note
- Remember to add Openzeppelin to node packages if you intend to deploy the contract using hardhat or truffle.

