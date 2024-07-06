using System;
using DynamicData;
using Mafia.Models.Enums;

namespace Mafia.Models
{
    public static class Statistic
    {
        #region For Statistic

        public static SourceList<Player> Players { get; } = new();
        public static string MasterNameProperty { get; private set; } = "";
        
        #endregion
        
        public static void DefineMaster(string name)
        {
            MasterNameProperty = name;
        }
    }
}
