// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.Constants
{

    public static class Cache
    {
        public const string GetAllCustomersCacheKey = "all-customers";
        public const string GetAllKeyValuesCacheKey = "all-keyvalues";
        public const string GetAllDocumentTypesCacheKey = "all-documenttypes";
        public static string GetKeyValuesCacheKey(string name)
        {
            return $"{name}-keyvalues";
        }
    }

}
