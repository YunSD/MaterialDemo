using System.Runtime.InteropServices;
using System.Windows.Markup;


[assembly: ComVisible(false)]
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]
[assembly: XmlnsPrefix("https://github.com/materialDemo/xaml", "ui")]
[assembly: XmlnsDefinition("https://github.com/materialDemo/xaml", "Wpf.Ui")]
[assembly: XmlnsDefinition("https://github.com/materialDemo/xaml", "Wpf.Ui.Controls")]
[assembly: XmlnsDefinition("https://github.com/materialDemo/xaml", "Wpf.Ui.Markup")]
[assembly: XmlnsDefinition("https://github.com/materialDemo/xaml", "Wpf.Ui.Converters")]


[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

