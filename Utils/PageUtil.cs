using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MaterialDemo.Utils
{
    public class PageUtil
    {

        public static DialogHost? GetDialogHost(DependencyObject ob)
        {
            DependencyObject element = VisualTreeHelper.GetParent(ob);

            while (element != null && !(element is DialogHost))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            return element as DialogHost;
        }

        private static readonly string Identifier = "RootDialog";

        public static void ShowHostDialog(object param) {
            DialogHost.Show(param, Identifier);
        }

        public static void CloseHostDialog() {
            DialogHost.Close(Identifier);
        }

    }
}
