using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2RBot.Common.Enum
{
    /// <summary>
    /// An enumeration for QuestHelper objects to enable extra functionality depending on which type of quest's it is being called by. 
    /// </summary>
    public enum QuestType
    {
        Main,

        Weekly,

        Scroll,

        Dungeon,

        TOI,

        AltarOfMadness
    }
}
