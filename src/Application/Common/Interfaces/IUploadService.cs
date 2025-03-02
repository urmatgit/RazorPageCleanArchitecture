// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Models;

namespace CleanArchitecture.Razor.Application.Common.Interfaces
{
    public interface IUploadService
    {
        Task<string> UploadAsync(UploadRequest request);
    }
}
