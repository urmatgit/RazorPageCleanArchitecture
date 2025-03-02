using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Audit;
using CleanArchitecture.Razor.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class AuditTrailConfiguration : IEntityTypeConfiguration<AuditTrail>
    {
        public void Configure(EntityTypeBuilder<AuditTrail> builder)
        {
            builder.Property(t => t.AuditType)
               .HasConversion<string>();
            builder.Property(e => e.AffectedColumns)
               .HasConversion(
                     v => JsonSerializer.Serialize(v, null),
                     v => JsonSerializer.Deserialize<List<string>>(v, null),
                     new ValueComparer<ICollection<string>>(
                            (c1, c2) => c1.SequenceEqual(c2),
                                   c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                   c => (ICollection<string>)c.ToList()));

            builder.Property(u => u.OldValues)
                .HasConversion(
                    d => JsonSerializer.Serialize(d, null),
                    s => JsonSerializer.Deserialize<Dictionary<string, object>>(s, null)
                );
            builder.Property(u => u.NewValues)
                .HasConversion(
                    d => JsonSerializer.Serialize(d, null),
                    s => JsonSerializer.Deserialize<Dictionary<string, object>>(s, null)
                );
            builder.Property(u => u.PrimaryKey)
                .HasConversion(
                    d => JsonSerializer.Serialize(d, null),
                    s => JsonSerializer.Deserialize<Dictionary<string, object>>(s, null)
                );

            builder.Ignore(x => x.TemporaryProperties);
            builder.Ignore(x => x.HasTemporaryProperties);
        }
    }
}
