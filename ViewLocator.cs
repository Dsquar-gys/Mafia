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
            string name;

            if (vmName.Contains("Window")) // For windows
            {
                name = vmName.Replace("ViewModel", "");
            }
            else if (data is HeaderVMBase) // For headers
            {
                name = vmName.Replace("ViewModel", "View");
                name += "View";
            }
            else // For any other VM
            {
                name = vmName.Replace("ViewModel", "View");
            }
            
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