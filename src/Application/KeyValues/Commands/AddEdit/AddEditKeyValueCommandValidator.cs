// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.KeyValues.Queries;
using FluentValidation;

namespace CleanArchitecture.Razor.Application.KeyValues.Commands.AddEdit
{
    public class AddEditKeyValueCommandValidator : AbstractValidator<AddEditKeyValueCommand>
    {
        public AddEditKeyValueCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(256)
                .NotEmpty();
            RuleFor(v => v.Text)
                .MaximumLength(256)
                .NotEmpty();
            RuleFor(v => v.Value)
                .MaximumLength(256)
                .NotEmpty();
          
        }
    }
}
