// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.DocumentTypes.Commands.AddEdit;
using CleanArchitecture.Razor.Application.DocumentTypes.Queries.Export;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.DocumentTypes.Commands.Import
{
    public class ImportDocumentTypesCommand : IRequest<Result>
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
    public class CreateDocumentTypeTemplateCommand : IRequest<byte[]>
    {
        public IEnumerable<string> Fields { get; set; }
        public string SheetName  { get;set; }
    }
    public class ImportDocumentTypesCommandHandler :
        IRequestHandler<CreateDocumentTypeTemplateCommand, byte[]>,
        IRequestHandler<ImportDocumentTypesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ImportDocumentTypesCommandHandler> _localizer;
        private readonly IValidator<AddEditDocumentTypeCommand> _addValidator;

        public ImportDocumentTypesCommandHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ImportDocumentTypesCommandHandler> localizer,
            IValidator<AddEditDocumentTypeCommand> addValidator
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
            _addValidator = addValidator;
        }
        public async Task<Result> Handle(ImportDocumentTypesCommand request, CancellationToken cancellationToken)
        {
            var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, DocumentType, object>>
            {
                { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]]?.ToString() },
                { _localizer["Description"], (row,item) => item.Description =  row[_localizer["Description"]]?.ToString() }
            }, _localizer["DocumentTypes"]);

            if (result.Succeeded)
            {
                var importItems = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var item in importItems)
                {
                    var validationResult = await _addValidator.ValidateAsync(_mapper.Map<AddEditDocumentTypeCommand>(item), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        var exist = await _context.DocumentTypes.AnyAsync(x => x.Name==item.Name,cancellationToken);
                        if (!exist)
                        {
                            await _context.DocumentTypes.AddAsync(item, cancellationToken);
                        }
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(item.Name) ? $"{item.Name} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result.FailureAsync(errors);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return await Result.SuccessAsync();
            }
            else
            {
                return await Result.FailureAsync(result.Errors);
            }
        }

        public async Task<byte[]> Handle(CreateDocumentTypeTemplateCommand request, CancellationToken cancellationToken)
        {
            var fields = new string[] {
                _localizer["Name"],
                _localizer["Description"]
                };
            var result = await _excelService.CreateTemplateAsync(fields, _localizer["DocumentTypes"]);
            return result;
        }
    }
}
