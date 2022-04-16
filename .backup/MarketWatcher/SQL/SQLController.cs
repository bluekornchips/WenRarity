using MarketWatcher.SQL.Rime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatcher.SQL
{
    public class SQLController
    {
        public static SQLController _instance;

        private SQLController() { }
        public static SQLController Instance { get { return _instance; } }
        public RimeService RimeService { get; set; }
        public RawSQLService RawSQLService { get; set; }
    }
}
