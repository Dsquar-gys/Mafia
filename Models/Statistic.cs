using System.Reactive.Subjects;
using DynamicData;
using Mafia.Templated_Controls;

namespace Mafia.Models
{
    public static class Statistic
    {
        #region For Statistic

        public static SourceCache<PlayerCard, string> Players => new(x => x.PlayerName);
        public static string MasterNameProperty { get; private set; } = "";

        #endregion
        
        public static void DefineMaster(string name)
        {
            MasterNameProperty = name;
        }
    }
}
