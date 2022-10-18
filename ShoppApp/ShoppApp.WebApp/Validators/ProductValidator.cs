using FluentValidation;
using ShoppApp.WebApp.Models;

namespace ShoppApp.WebApp.Validators
{
    public class ProductValidator:AbstractValidator<ProductModel>
    {
        public ProductValidator()
        {
            RuleFor(a=>a.Name).NotNull().MaximumLength(100).WithMessage("Name en fazla 100 karakter")
                .MinimumLength(8).WithMessage("en az 8 karakter");
            RuleFor(a => a.Price).NotNull().GreaterThan(0).LessThan(100000);
        }
    }
}
