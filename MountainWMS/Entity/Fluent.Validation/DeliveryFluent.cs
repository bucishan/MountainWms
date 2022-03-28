using FluentValidation;

namespace M.Core.Entity.Fluent.Validation
{
    public class DeliveryFluent : AbstractValidator<Wms_delivery>
    {
        public DeliveryFluent()
        {
            RuleFor(x => x.Remark).MaximumLength(200).WithMessage("备注长度不能超过200");
        }
    }
}