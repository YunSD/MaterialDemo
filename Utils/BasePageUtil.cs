﻿using HandyControl.Controls;
using log4net;
using MaterialDemo.ViewModels.Pages.Business;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Controls;

namespace MaterialDemo.Utils
{
    public class BasePageUtil
    {
        private static ILog logger = LogManager.GetLogger(nameof(BasePageUtil));
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


        public static SymbolRegular ParseSymbolIcon(string? symbol)
        {
            SymbolRegular symbolRegular = SymbolRegular.ErrorCircle24;
            try {
                symbolRegular = (SymbolRegular)Enum.Parse(typeof(SymbolRegular), symbol);
            } catch (Exception e) {
                logger.Error(e);
            }
            return symbolRegular;
        }

        public static Type ParseClassType(string? clazz){
            Type type = typeof(Views.Pages.Base.EmptyViewPage);
            if (clazz == null) return type;
            try {
                Type? cur = Type.GetType(clazz);
                if (cur != null) type = cur;
            } catch (Exception e) {
                logger.Error(e);
            }
            return type;
        }

    }
}