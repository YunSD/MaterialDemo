using MaterialDemo.Domain;
using MaterialDemo.Domain.Models.Entity;
using System.ComponentModel.DataAnnotations;
using MaterialDemo.ViewModels.Pages.Upms.VObject;
using MaterialDemo.Domain.Enums;
using MaterialDemo.Utils;

namespace MaterialDemo.ViewModels.Pages.Upms
{
    public partial class MenuEditorViewModel : ObservableValidator
    {
        [ObservableProperty]
        private bool editModel = true;

        [ObservableProperty]
        private IList<SysMenuViewInfo> parents;

        private long? menuId;

        [ObservableProperty]
        private long? parentId;

        [ObservableProperty]
        private string? parentName;

        [Required(ErrorMessage ="该字段不能为空")]
        [ObservableProperty]
        private string? name;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? router;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        public string? icon;

        [ObservableProperty]
        private MenuPositionEnum position = MenuPositionEnum.TOP;

        [ObservableProperty]
        private int? seq;

        [ObservableProperty]
        private string? remark;


        private FormSubmitEventHandler<SysMenu> SubmitEvent;

        public MenuEditorViewModel(SysMenuViewInfo entity, IList<SysMenuViewInfo> sysMenuVOs, FormSubmitEventHandler<SysMenu> submitEvent) {
            this.SubmitEvent = submitEvent;

            if (entity.MenuId.HasValue) {
                this.menuId = entity.MenuId;
                editModel = false;
            }
            this.Parents = sysMenuVOs;
            this.ParentId = entity.ParentId;
            this.ParentName = entity.ParentName;
            this.Name = entity.Name;
            this.Router = entity.Router;
            this.Icon = entity.Icon;
            this.Position = entity.Position;
            this.Seq = entity.Seq;
            this.Remark = entity.Remark;
        }

        [RelayCommand]
        private void Submit() {
            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
            ValidateAllProperties();

            if (HasErrors) return;

            SysMenu entity = new()
            {
                MenuId = menuId,
                ParentId = ParentId == null ? 0 : ParentId,
                Name = Name,
                Icon = Icon,
                Router = Router,
                Position = Position,
                Seq = Seq,
                Remark = Remark,
            };
            SubmitEvent(entity);
        }
    }
}
