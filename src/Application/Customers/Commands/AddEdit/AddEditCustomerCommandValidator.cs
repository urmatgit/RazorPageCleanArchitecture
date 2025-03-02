// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Customers.Commands.AddEdit
{
    public class AddEditCustomerCommandValidator : AbstractValidator<AddEditCustomerCommand>
    {
        public AddEditCustomerCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(256)
                .NotEmpty();
            RuleFor(v => v.NameOfEnglish)
                .MaximumLength(256)
                .NotEmpty();
            RuleFor(v => v.GroupName)
                .MaximumLength(256)
                .NotEmpty();
            RuleFor(v => v.Region)
                .MaximumLength(256)
                .NotEmpty();
            RuleFor(v => v.Sales)
                .MaximumLength(256)
                .NotEmpty();
            RuleFor(v => v.RegionSalesDirector)
                .MaximumLength(256)
                .NotEmpty();
            RuleFor(v=>v.PartnerType)
                .MaximumLength(128)
                .NotEmpty();
        }
    }
}
