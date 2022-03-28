using FluentValidation;

namespace M.Core.Entity.Fluent.Validation
{
    public class SysAuthFluent : AbstractValidator<Sys_auth>
    {
        public SysAuthFluent()
        {
            RuleFor(x => x.AuthName).NotNull().NotEmpty().WithMessage("操作名称不能为空").Length(1, 10).WithMessage("操作名称长度不能超过10");
            RuleFor(x => x.Remark).MaximumLength(200).WithMessage("备注长度不能超过200");
        }
    }
}
