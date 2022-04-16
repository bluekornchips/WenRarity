using Rime.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WenRarityLibrary;

namespace Rarity.Controller
{
    internal class RimeController
    {
        private static RimeController _instance;
        public static RimeController Instance => _instance ?? (_instance = new RimeController());
        private static Ducky _ducky = Ducky.Instance;
        private RimeController() { }

        private void template()
        {
            using RimeDb context = new RimeDb();
            var dbContextTransaction = context.Database.BeginTransaction();
            {
                try
                {
                    context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    _ducky.Error("RimeController", "", ex.Message);
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }
    }
}
