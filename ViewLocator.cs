using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Mafia.ViewModels;
using System;

namespace Mafia
{
    public class ViewLocator : IDataTemplate
    {
        public Control Build(object data)
        {
            var vmName = data.GetType().FullName!;
            var name = data is ViewModelBase ? vmName.Replace("ViewModel", "View") : vmName + "View";
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object data)
        {
            return data is ViewModelBase or HeaderTemplateBase;
        }
    }
}