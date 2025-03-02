// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Domain.Enums;

namespace CleanArchitecture.Razor.Application.Models
{
    public class UploadRequest
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public UploadType UploadType { get; set; }
        public byte[] Data { get; set; }
    }
}
