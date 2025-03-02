// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Documents.Commands.Delete
{
    public class DeleteDocumentCommand: IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class DeleteCheckedDocumentsCommand : IRequest<Result>
    {
        public int[] Id { get; set; }
    }

    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Result>,
        IRequestHandler<DeleteCheckedDocumentsCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public DeleteDocumentCommandHandler(
            IApplicationDbContext context
            )
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var item =await _context.Documents.FindAsync(request.Id);
            _context.Documents.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedDocumentsCommand request, CancellationToken cancellationToken)
        {
            var items = await _context.Documents.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach(var item in items)
            {
                _context.Documents.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
