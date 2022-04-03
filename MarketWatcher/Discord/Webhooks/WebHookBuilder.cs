using MarketWatcher.Classes;
using MarketWatcher.Classes.JPGStore;
using MarketWatcher.Discord.Webhooks;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarketWatcher.Discord.Webhooks
{
    public class WebHookBuilder
    {
        public static WebHookBuilder _instance;
        // Test URL = https://discord.com/api/webhooks/957835242763333652/V_FkSC6FGd_uf8K9SipGPi9z_ZrrM1j-Ev6wjYRtUEyQ-Ltxavr1MGiYo_krFA1hAcpb
        private WebHookBuilder() { }
        public static WebHookBuilder GetInstance()
        {
            if (_instance == null) _instance = new WebHookBuilder();
            return _instance;
        }

        public WebHook Build(CollectionItemData item)
        {
            return item.AsWebHook(item);
        }
    }
}