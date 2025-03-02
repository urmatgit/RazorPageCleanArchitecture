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

namespace CleanArchitecture.Razor.Application.Features.ApprovalDatas.Commands.AddEdit
{
    public class AddEditApprovalDataCommand: ApprovalDataDto,IRequest<Result>
    {
      
    }

    public class AddEditApprovalDataCommandHandler : IRequestHandler<AddEditApprovalDataCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditApprovalDataCommandHandler> _localizer;
        public AddEditApprovalDataCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditApprovalDataCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(AddEditApprovalDataCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing AddEditApprovalDataCommandHandler method 
           throw new System.NotImplementedException();
        }
    }
}
