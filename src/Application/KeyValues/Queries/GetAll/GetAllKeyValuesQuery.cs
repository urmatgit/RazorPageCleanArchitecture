// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Caching;
using CleanArchitecture.Razor.Application.Constants;
using CleanArchitecture.Razor.Application.KeyValues.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.Razor.Application.KeyValues.Queries.ByName
{
    public class GetAllKeyValuesQuery : IRequest<IEnumerable<KeyValueDto>>, ICacheable
    {

        public string CacheKey =>Cache.GetAllKeyValuesCacheKey;

        public MemoryCacheEntryOptions Options =>null;
    }
    public class GetAllKeyValuesQueryHandler : IRequestHandler<KeyValuesQueryByName, IEnumerable<KeyValueDto>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllKeyValuesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<KeyValueDto>> Handle(KeyValuesQueryByName request, CancellationToken cancellationToken)
        {
            var data = await _context.KeyValues
               .ProjectTo<KeyValueDto>(_mapper.ConfigurationProvider)
               .ToListAsync(cancellationToken);
            return data;
        }
    }
}
