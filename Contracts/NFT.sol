// SPDX-License-Identifier: MIT
pragma solidity >=0.7.0 <0.9.0;

import "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/utils/Strings.sol";

contract Token1155 is ERC1155, Ownable {
    
    string public name;
    string public description;
    uint256 public quantity;
    address public approvedAddress;
    constructor(string memory _name, string memory _description, uint256 _quantity, address _address, string memory _url)
        ERC1155(_url)
    {
        name = _name;
        description = _description;
        quantity = _quantity;
        approvedAddress = _address;
        _mint(msg.sender, 1, quantity, "");
        _setApprovalForAll(msg.sender, approvedAddress, true);
    }

    function uri(uint256 _tokenId, string memory url) public pure returns (string memory) {
        return string(
            abi.encodePacked(
                url, 
                Strings.toString(_tokenId), 
                ".json"
            )
        );
    } 

    function setURI(string memory newuri) public onlyOwner {
        _setURI(newuri);
    }

    function mint(address account, uint256 id, uint256 amount, bytes memory data)
        public
        onlyOwner
    {
        _mint(account, id, amount, data);
    }

    function mintBatch(address to, uint256[] memory ids, uint256[] memory amounts, bytes memory data)
        public
        onlyOwner
    {
        _mintBatch(to, ids, amounts, data);
    }
}
