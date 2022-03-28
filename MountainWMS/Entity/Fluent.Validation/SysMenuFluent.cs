using FluentValidation;

namespace M.Core.Entity.Fluent.Validation
{
    public class SysMenuFluent : AbstractValidator<Sys_menu>
    {
        public SysMenuFluent()
        {
            RuleFor(x => x.MenuName).NotNull().NotEmpty().WithMessage("菜单名称不能为空").Length(1, 8).WithMessage("菜单名称不能超过8");
            RuleFor(x => x.MenuUrl).NotNull().NotEmpty().WithMessage("菜单地址不能为空").Length(1, 20).WithMessage("菜单地址长度不能超过20");
            RuleFor(x => x.MenuIcon).NotNull().NotEmpty().WithMessage("菜单图标不能为空").Length(1, 15).WithMessage("菜单图标长度不能超过15");
            RuleFor(x => x.Remark).MaximumLength(200).WithMessage("备注长度不能超过200");
        }
    }
}