// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Caching;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Constants;
using CleanArchitecture.Razor.Application.KeyValues.Caching;
using CleanArchitecture.Razor.Application.KeyValues.DTOs;
using CleanArchitecture.Razor.Application.KeyValues.Queries;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Enums;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;

namespace CleanArchitecture.Razor.Application.KeyValues.Commands.SaveChanged
{
    public class SaveChangedKeyValuesCommand:IRequest<Result>, ICacheInvalidator
    {
      public KeyValueDto[] Items { get; set; }

        public string CacheKey => Cache.GetAllKeyValuesCacheKey;

        public CancellationTokenSource ResetCacheToken => KeyValueCacheTokenSource.ResetCacheToken;
    }

    public class SaveChangedKeyValuesCommandHandler : IRequestHandler<SaveChangedKeyValuesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SaveChangedKeyValuesCommandHandler(
            IApplicationDbContext context,
             IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result> Handle(SaveChangedKeyValuesCommand request, CancellationToken cancellationToken)
        {
            foreach(var item in request.Items)
            {
                switch (item.TrackingState)
                {
                    case TrackingState.Added:
                        var newitem = _mapper.Map<KeyValue>(item);
                        await _context.KeyValues.AddAsync(newitem, cancellationToken);
                        break;
                    case TrackingState.Deleted:
                        var delitem =await _context.KeyValues.FindAsync(item.Id);
                        _context.KeyValues.Remove(delitem);
                        break;
                    case TrackingState.Modified:
                        var edititem = await _context.KeyValues.FindAsync(item.Id);
                        edititem.Name = item.Name;
                        edititem.Text = item.Text;
                        edititem.Value = item.Value;
                        edititem.Description = item.Description;
                        _context.KeyValues.Update(edititem);
                        break;
                    case TrackingState.Unchanged:
                    default:
                        break;
                }
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
    }
}
