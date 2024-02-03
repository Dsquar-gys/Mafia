using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;

namespace Mafia.Models
{
    public static class Statistic
    {
        #region Observables

        public static Subject<string> MasterNameObservable { get; } = new Subject<string>();

        #endregion

        #region For Statistic

        public static IEnumerable<PlayerCard> Players { get; set; } = new ObservableCollection<PlayerCard>();
        public static string MasterNameProperty { get; private set; } = "";

        #endregion

        public static void DefineMaster(string name)
        {
            MasterNameProperty = name;
            MasterNameObservable.OnNext(name);
        }
    }
}
