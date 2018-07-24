pragma solidity ^0.4.21;

contract IERC223 {
    function random(address _userAddress, uint _phase, uint[] _ticketIndex) public returns (string);
    function reRandom(address _userAddress, uint[] _ticketIndex, uint[] arrayIndex) public returns(string);
    function queryUserTicket(uint[] arrayIndex) public constant returns(string);
}

contract ContractReceiver {
    struct TKN {
        address sender;
        uint value;
    }

    function tokenFallback(address _from, uint _value) public pure {
        TKN memory tkn;
        tkn.sender = _from;
        tkn.value = _value;

        /* tkn variable is analogue of msg variable of Ether transaction
        *  tkn.sender is person who initiated this token transaction   (analogue of msg.sender)
        *  tkn.value the number of tokens that were sent   (analogue of msg.value)
        *  tkn.data is data of token transaction   (analogue of msg.data)
        *  tkn.sig is 4 bytes signature of function
        *  if data of token transaction is a function execution
        */
    }
}

/**
 * @title Ownable
 * @dev The Ownable contract has an owner address, and provides basic authorization control
 * functions, this simplifies the implementation of "user permissions".
 */
contract Ownable {
    address public owner;
    event OwnershipTransferred(address indexed previousOwner, address indexed newOwner);
    /**
    * @dev The Ownable constructor sets the original `owner` of the contract to the sender
    * account.
    */
    function Ownable() public {
        owner = msg.sender;
    }
    /**
    * @dev Throws if called by any account other than the owner.
    */
    modifier onlyOwner() {
        require(msg.sender == owner);
        _;
    }
    /**
    * @dev Allows the current owner to transfer control of the contract to a newOwner.
    * @param newOwner The address to transfer ownership to.
    */
    function transferOwnership(address newOwner) public onlyOwner {
        require(newOwner != address(0));
        emit OwnershipTransferred(owner, newOwner);
        owner = newOwner;
    }
}

/**
* @title SafeMath
* @dev Math operations with safety checks that throw on error
*/
library SafeMath {
    /**
    * @dev Multiplies two numbers, throws on overflow.
    */
    function mul(uint256 a, uint256 b) internal pure returns (uint256 c) {
        if (a == 0) {
            return 0;
        }
        c = a * b;
        assert(c / a == b);
        return c;
    }

    /**
    * @dev Integer division of two numbers, truncating the quotient.
    */
    function div(uint256 a, uint256 b) internal pure returns (uint256) {
        assert(b > 0); // Solidity automatically throws when dividing by 0
        // uint256 c = a / b;
        // assert(a == b * c + a % b); // There is no case in which this doesn't hold
        return a / b;
    }

    /**
    * @dev Subtracts two numbers, throws on overflow (i.e. if subtrahend is greater than minuend).
    */
    function sub(uint256 a, uint256 b) internal pure returns (uint256 c) {
        assert(b <= a);
        c = a - b;
        assert(c < a);
        return c;
    }

    /**
    * @dev Adds two numbers, throws on overflow.
    */
    function add(uint256 a, uint256 b) internal pure returns (uint256 c) {
        c = a + b;
        assert(c >= a && c >= b);
        return c;
    }
}

contract CryptoLot is IERC223, Ownable{
    
    using SafeMath for uint256;
    event Random(string);
    event Drawing(string);
    
    struct CryptoLotTicket
    {
        address userAddress;
        uint phase;
        uint32 ticketNumber;
        uint ticketIndex;
    }
    
    CryptoLotTicket[] TicketList;
    
    function uintToString(uint v) pure internal returns (string str) {
        uint maxlength = 100;
        bytes memory reversed = new bytes(maxlength);
        uint i = 0;
        while (v != 0) {
            uint remainder = v % 10;
            v = v / 10;
            reversed[i++] = byte(48 + remainder);
        }
        bytes memory s = new bytes(i);
        for (uint j = 0; j < i; j++) {
            s[j] = reversed[i - 1 - j];
        }
        str = string(s);
    }

    function strConcat(string _a, string _b, string _c, string _d, string _e) pure internal returns (string){
        bytes memory _ba = bytes(_a);
        bytes memory _bb = bytes(_b);
        bytes memory _bc = bytes(_c);
        bytes memory _bd = bytes(_d);
        bytes memory _be = bytes(_e);
        string memory abcde = new string(_ba.length + _bb.length + _bc.length + _bd.length + _be.length);
        bytes memory babcde = bytes(abcde);
        uint k = 0;
        for (uint i = 0; i < _ba.length; i++) babcde[k++] = _ba[i];
        for (i = 0; i < _bb.length; i++) babcde[k++] = _bb[i];
        for (i = 0; i < _bc.length; i++) babcde[k++] = _bc[i];
        for (i = 0; i < _bd.length; i++) babcde[k++] = _bd[i];
        for (i = 0; i < _be.length; i++) babcde[k++] = _be[i];
        return string(babcde);
    }

    function strConcat(string _a, string _b, string _c, string _d) pure internal returns (string) {
        return strConcat(_a, _b, _c, _d, "");
    }
    
    function strConcat(string _a, string _b, string _c) pure internal returns (string) {
        return strConcat(_a, _b, _c, "", "");
    }
    
    function strConcat(string _a, string _b) pure internal returns (string) {
        return strConcat(_a, _b, "", "", "");
    }

    function random(address _userAddress, uint _phase, uint[] _ticketIndex) public onlyOwner returns(string){
        string memory result;
        for(uint i = 0; i < _ticketIndex.length; i++)
        {
            uint32 ticketNo = uint32(keccak256(block.timestamp , _userAddress, _ticketIndex[i]))%1000000;
            TicketList.push(CryptoLotTicket({userAddress : _userAddress, phase : _phase, ticketNumber : ticketNo, ticketIndex : _ticketIndex[i]}));
            result = strConcat(result, uintToString(ticketNo), ";");
        }
        emit Random(result);
        return result;
    }
    
    function reRandom(address _userAddress, uint[] _ticketIndex, uint[] arrayIndex) public onlyOwner returns(string){
        string memory result;
        for(uint i = 0; i < arrayIndex.length; i++)
        {
            uint32 ticketNo = uint32(keccak256(block.timestamp , _userAddress, _ticketIndex[i]))%1000000;
            TicketList[arrayIndex[i]].ticketNumber = ticketNo;
            result = strConcat(result, uintToString(ticketNo), ";");
        }
        emit Random(result);
        return result;
    }
    
    function queryUserTicket(uint[] arrayIndex) public constant returns(string) {
        string memory result;
        for(uint i = 0; i < arrayIndex.length; i++)
        {
            result = strConcat(result, uintToString(TicketList[arrayIndex[i]].ticketNumber), ";");
        }
        return (result);
    }
}