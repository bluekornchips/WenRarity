using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rime.Builders
{
    internal interface IFrameworkWriter
    {
        void ADOModel();
        void ViewModel();
        void UpdateRimeDB();
        void AttributeTables();

    }
}
