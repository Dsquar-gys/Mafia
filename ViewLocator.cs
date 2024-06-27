using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Mafia.ViewModels;
using System;
using Mafia.ViewModels.Headers;

namespace Mafia
{
    public class ViewLocator : IDataTemplate
    {
        public Control Build(object data)
        {
            var vmName = data.GetType().FullName!;
            var name = vmName.Replace("ViewModel", "View");
            if (data is HeaderVMBase) name += "View";
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object data)
        {
            return data is ViewModelBase or HeaderVMBase;
        }
    }
}