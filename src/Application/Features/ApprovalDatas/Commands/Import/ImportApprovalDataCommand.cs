using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ApprovalDatas.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ApprovalDatas.Commands.Import
{
    public class ImportApprovalDataCommand: ApprovalDataDto,IRequest<Result>
    {
      
    }

    public class ImportApprovalDataCommandHandler : IRequestHandler<ImportApprovalDataCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ImportApprovalDataCommandHandler> _localizer;
        public ImportApprovalDataCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<ImportApprovalDataCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(ImportApprovalDataCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing ImportApprovalDataCommandHandler method 
           throw new System.NotImplementedException();
        }
    }
}
