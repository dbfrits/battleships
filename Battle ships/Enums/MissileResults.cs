using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_ships.Enums
{
    public enum MissileResults
    {
        NotLaunched,
        InvalidOcean,
        InvalidTarget,
        UsedTarget,
        Missed,
        Hit,
        Sunk,
        GameOver
    }
}
