// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.KeyValues.Caching
{
    public  sealed class KeyValueCacheTokenSource
    {
        static KeyValueCacheTokenSource() {
            ResetCacheToken = new CancellationTokenSource();
        }
        public static CancellationTokenSource ResetCacheToken { get; private set; }
    }
}
