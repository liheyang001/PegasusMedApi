using Microsoft.EntityFrameworkCore;
using PegasusMedApi.Server.Models;

namespace PegasusMedApi.Server.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

    public DbSet<MedRequest> Requests { get; set; }
    public DbSet<VendorStatus> VendorStatuses { get; set; }

    
}