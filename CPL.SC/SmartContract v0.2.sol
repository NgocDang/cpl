pragma solidity ^0.4.21;

contract ICryptoLot {
    function random(uint32 _phase, address _playerAddress, uint32[] _listTicketIndex) public;
    function reRandom(uint32 _phase, address _playerAddress, uint32[] _listTicketIndex) public;
    function getTicket(uint32 _phase, address _playerAddress, uint32 _ticketIndex) public constant returns(uint32);
    
    event TicketNumberGenerated(uint32 indexed phaseNumber, address indexed playerAddress, uint32 indexed ticketIndex, uint32 ticketNumber);
    event TicketNumberReGenerated(uint32 indexed phaseNumber, address indexed playerAddress, uint32 indexed ticketIndex, uint32 oldTicketNumber, uint32 newTicketNumber);
    
    mapping (uint32 => mapping(address => mapping(uint32 => uint32))) TicketNumber;
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

contract CryptoLot is ICryptoLot, Ownable{
    function random(uint32 _phase, address _playerAddress, uint32[] _listTicketIndex) public onlyOwner{
        for(uint32 i = 0; i < _listTicketIndex.length; i++)
        {
            uint32 ticketNo = uint32(keccak256(block.timestamp , _playerAddress, _listTicketIndex[i]))%1000000;
            
            TicketNumber[_phase][_playerAddress][_listTicketIndex[i]] = ticketNo;
            
            emit TicketNumberGenerated(_phase, _playerAddress, _listTicketIndex[i], ticketNo);
        }
    }
    
    function reRandom(uint32 _phase, address _playerAddress, uint32[] _listTicketIndex) public onlyOwner{
        for(uint32 i = 0; i < _listTicketIndex.length; i++)
        {
            uint32 oldTicketNumber = TicketNumber[_phase][_playerAddress][_listTicketIndex[i]];
        
            uint32 ticketNo = uint32(keccak256(block.timestamp , _playerAddress, _listTicketIndex[i]))%1000000;
            
            TicketNumber[_phase][_playerAddress][_listTicketIndex[i]] = ticketNo;
            
            emit TicketNumberReGenerated(_phase, _playerAddress, _listTicketIndex[i], oldTicketNumber, ticketNo);
            emit TicketNumberGenerated(_phase, _playerAddress, _listTicketIndex[i], ticketNo);
        }
        
    }
    
    function getTicket(uint32 _phase, address _playerAddress, uint32 _ticketIndex) public view returns(uint32) {
        return TicketNumber[_phase][_playerAddress][_ticketIndex];
    }
}