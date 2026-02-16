using Microsoft.AspNetCore.Mvc;
using PegasusMedApi.Server.Models;
using PegasusMedApi.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace PegasusMedApi.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    private readonly ApiDbContext _context;
    public RequestsController(ApiDbContext context)
    {
        _context = context;
    }
    [HttpPost]
    public async Task<IActionResult> CreateRequest(CreateRequestDto dto)
    {
        var newRequest = new MedRequest
        {
            ClientId = dto.ClientId.ToString(),
            ItemDetails = dto.ItemDetails,
            CreatedAt = DateTime.UtcNow,
            VendorAssignments = new List<VendorStatus>()
        };

        foreach (var vId in dto.VendorIds)
        {
            newRequest.VendorAssignments.Add(new VendorStatus
            {
                VendorId = vId,
                IsFlagged = false
            });
        }
        _context.Requests.Add(newRequest);

        await _context.SaveChangesAsync();

        return Ok(newRequest);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRequestById(int id)
    {
        var request = await _context.Requests
            .Include(r => r.VendorAssignments)
            .FirstOrDefaultAsync(r => r.Id == id); 

        if (request == null) return NotFound();

        return Ok(request);
    }

    [HttpGet("vendor/{vendorId}")]
    public async Task<IActionResult> GetRequestsForVendor(string vendorId)
    {
        var vendorRequests = await _context.Requests
            .Where(r => r.VendorAssignments.Any(v => v.VendorId == vendorId))
            .Select(r => new {
                r.Id,
                r.ClientId,
                r.ItemDetails,
                r.CreatedAt,
                MyStatus = r.VendorAssignments
                            .Where(v => v.VendorId == vendorId)
                            .Select(v => new { v.IsFlagged })
                            .FirstOrDefault()
            })
            .ToListAsync();

        if (!vendorRequests.Any())
        {
            return NotFound($"No requests found for vendor: {vendorId}");
        }

        return Ok(vendorRequests);
    }

    [HttpPatch("{id}/vendor/{vendorId}/acknowledge")]
    public async Task<IActionResult> Acknowledge(int id, string vendorId)
    {
        var assignment = await _context.VendorStatuses
            .FirstOrDefaultAsync(v => v.MedRequestId == id && v.VendorId == vendorId);

        if (assignment == null)
        {
            return NotFound($"Assignment not found for Request {id} and Vendor {vendorId}");
        }
        assignment.AcknowledgedAt = DateTime.UtcNow;
        assignment.IsFlagged = true; 

        await _context.SaveChangesAsync();

        return Ok(new 
        { 
            message = "Acknowledged and Flagged", 
            time = assignment.AcknowledgedAt, 
            isFlagged = assignment.IsFlagged 
        });
    }
}

public class CreateRequestDto
{
    public int ClientId { get; set; }
    public string ItemDetails { get; set; } = "";
    public List<string> VendorIds { get; set; } = new();
}