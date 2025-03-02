// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.KeyValues.Queries.ByName;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitecture.Application.IntegrationTests.KeyValues.Queries
{
    using static Testing;
    public class KeyValuesQueryTests : TestBase
    {
        [Test]
        public void ShouldNotNullKeyValuesQueryByName()
        {
            var query = new KeyValuesQueryByName() {  Name="Status" };
            var result = SendAsync(query);
            FluentActions.Invoking(() =>
                SendAsync(query)).Should().NotBeNull();
        }
    }
}
