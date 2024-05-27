using ControlzEx.Standard;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.System;

namespace MaterialDemo.Utils
{
    public class BasePageUtil
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


        public static void ShowImageSelector(HandyControl.Controls.ImageSelector imageSelector, string? imagePath)
        {
            imagePath = BaseFileUtil.GetOriFilePath(imagePath);
            if (!File.Exists(imagePath)) return;
            imageSelector.SetValue(ImageSelector.UriPropertyKey, new Uri(imagePath));
            imageSelector.SetValue(ImageSelector.PreviewBrushPropertyKey, new ImageBrush(BitmapFrame.Create(imageSelector.Uri, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.None))
            {
                Stretch = imageSelector.Stretch
            });
            imageSelector.SetValue(ImageSelector.HasValuePropertyKey, true);
        }

    }
}