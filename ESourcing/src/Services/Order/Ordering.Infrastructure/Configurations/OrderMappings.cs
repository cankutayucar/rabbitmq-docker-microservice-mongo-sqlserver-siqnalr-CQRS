using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Configurations
{
    public class OrderMappings : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);
            builder.Property(x => x.AuctionId).HasColumnType("nvarchar(MAX)");
            builder.Property(x => x.SellerUserName).HasColumnType("nvarchar(MAX)");
            builder.Property(x => x.ProductId).HasColumnType("nvarchar(MAX)");
            builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.CreatedAt).HasColumnType("datetime");
        }
    }
}
