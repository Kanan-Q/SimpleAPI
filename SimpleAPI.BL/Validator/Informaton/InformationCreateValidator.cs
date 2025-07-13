using FluentValidation;
using SimpleAPI.BL.DTO.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.BL.Validator.Informaton;

public sealed class InformationCreateValidator : AbstractValidator<InformationCreateDTO>
{
    public InformationCreateValidator()
    {
        RuleFor(x => x.ProductName).Must(x => !string.IsNullOrWhiteSpace(x?.Trim().ToLower())).WithMessage("ProductName is required.");
        RuleFor(x => x.Description).Must(x => !string.IsNullOrWhiteSpace(x?.Trim().ToLower())).WithMessage("Description is required.");
    }
}
