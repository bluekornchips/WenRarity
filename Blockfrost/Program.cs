using Blockfrost.Builder;
using WenRarityLibrary.ADO.Blockfrost.Models;

BlockfrostBuilder bb = BlockfrostBuilder.Instance;

var collection = new Collection()
{
    PolicyId = "3f00d83452b4ead45cf5e0ca811fe8da561dfc45a5e414c88c4d8759",
    Name = "KBot",
    DatabaseName = "KBot",
    RealName = "KBot"
};

//var collection = new Collection()
//{
//    PolicyId = "b65ce524203b7a7d48b55ff037c847c5ec8c185cd3bdb7abad0a02d4",
//    Name = "DeluxeBotOGCollection",
//    DatabaseName = "DeluxeBotOGCollection",
//    RealName = "DeluxeBot OG Collection"
//};

//var collection = new Collection()
//{
//    PolicyId = "f96584c4fcd13cd1702c9be683400072dd1aac853431c99037a3ab1e",
//    Name = "PuurrtyCatsSociety",
//    DatabaseName = "PuurrtyCatsSociety",
//    RealName = "Puurrty Cats Society"
//};


//var collection = new Collection()
//{
//    PolicyId = "30874c13bca652c93bd475c748f819f5ace005f6689bb53503075938",
//    Name = "InvisibleBuddies",
//    DatabaseName = "InvisibleBuddies",
//    RealName = "Invisible Buddies"
//};


//collection = new Collection()
//{
//    PolicyId = "de2340edc45629456bf695200e8ea32f948a653b21ada10bc6f0c554",
//    Name = "DeadRabbits",
//    DatabaseName = "DeadRabbits",
//    RealName = "Dead Rabbit Resurrection Society"
//};




bb.Build(collection);