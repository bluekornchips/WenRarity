using Newtonsoft.Json.Linq;
using Rime.ADO.Classes;
using Rime.ViewModels;
using System.Collections.Generic;

namespace Rime.Builders
{
    public interface ITokenBuilder
    {
        void Build();
        Asset Clean(JToken jToken);
        void SetAttributes();
        void Rarity();
    }
}
