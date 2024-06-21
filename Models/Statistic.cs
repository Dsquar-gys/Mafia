using DynamicData;

namespace Mafia.Models
{
    public static class Statistic
    {
        #region For Statistic

        public static SourceList<Player> Players { get; } = new();
        public static string MasterNameProperty { get; private set; } = "";
        
        // TODO В статистике должна быть информация о красных и чёрных
        
        #endregion
        
        public static void DefineMaster(string name)
        {
            MasterNameProperty = name;
        }

        public static void CreateReport( GameOver state )
        {
            //TODO
        }
    }
}
