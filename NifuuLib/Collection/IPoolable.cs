using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.Collection
{
    public interface IPoolable
    {
        void Initialise();
        bool InUse { get; set; }
    }
}
