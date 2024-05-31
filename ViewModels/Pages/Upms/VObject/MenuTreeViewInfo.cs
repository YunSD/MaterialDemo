using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.Utils;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MaterialDemo.ViewModels.Pages.Upms.VObject
{
    public class MenuTreeViewInfo : SysMenu
    {
        public ObservableCollection<MenuTreeViewInfo>? Children { get; set; } // 子节点集合


        public static List<MenuTreeViewInfo> build(List<SysMenu> menus, long? root = 0) {
            var list = menus.Where(m => m.ParentId == root).OrderBy(m => m.Seq)
                .Select(m=> {
                    MenuTreeViewInfo info = MapperUtil.Map<SysMenu, MenuTreeViewInfo>(m);
                    List<MenuTreeViewInfo> list = build(menus, m.MenuId);
                    if(list.Any()) info.Children = new ObservableCollection<MenuTreeViewInfo>(list);
                    return info;
                }).ToList();
            return list;
        }

        //private static ObservableCollection<MenuTreeViewInfo> findNodes(MenuTreeViewInfo menu, ObservableCollection<SysMenu> menus) {
        //   var list = menus.Where(m => m.MenuId == menu.MenuId).OrderBy(m => m.Seq)
        //        .Select(m=> {
        //            MenuTreeViewInfo info = MapperUtil.Map<SysMenu, MenuTreeViewInfo>(m);
        //            info.Children = findNodes(info, menus);
        //            return info;
        //        }).ToList();

        //    return new ObservableCollection<MenuTreeViewInfo>(list);
        //}
    }
}
