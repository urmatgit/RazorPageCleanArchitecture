// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Caching;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Constants;
using CleanArchitecture.Razor.Application.Customers.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Customers.Commands.Delete
{
    public class DeleteCustomerCommand: IRequest<Result>, ICacheInvalidator
    {
        public int Id { get; set; }
        public string CacheKey => Cache.GetAllCustomersCacheKey;

        public CancellationTokenSource ResetCacheToken => CustomerCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedCustomersCommand : IRequest<Result>, ICacheInvalidator
    {
        public int[] Id { get; set; }
        public string CacheKey => Cache.GetAllCustomersCacheKey;

        public CancellationTokenSource ResetCacheToken => CustomerCacheTokenSource.ResetCacheToken;
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>,
        IRequestHandler<DeleteCheckedCustomersCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCustomerCommandHandler(
            IApplicationDbContext context
            )
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var item =await _context.Customers.FindAsync(request.Id);
            _context.Customers.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedCustomersCommand request, CancellationToken cancellationToken)
        {
            var items = await _context.Customers.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach(var item in items)
            {
                _context.Customers.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
