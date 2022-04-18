using Blockfrost.Builder;
using WenRarityLibrary.ADO.Rime.Models;

BlockfrostBuilder bb = BlockfrostBuilder.Instance;

//var collection = new Collection()
//{
//    PolicyId = "3f00d83452b4ead45cf5e0ca811fe8da561dfc45a5e414c88c4d8759",
//    Name = "KBot",
//    DatabaseName = "KBot",
//    RealName = "KBot"
//};

var collection = new Collection()
{
    PolicyId = "b65ce524203b7a7d48b55ff037c847c5ec8c185cd3bdb7abad0a02d4",
    Name = "DeluxeBotOGCollection",
    DatabaseName = "DeluxeBotOGCollection",
    RealName = "DeluxeBot OG Collection"
};

bb.Build(collection);