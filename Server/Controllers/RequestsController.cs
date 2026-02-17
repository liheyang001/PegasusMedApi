using Microsoft.AspNetCore.Mvc;
using PegasusMedApi.Server.Models;
using PegasusMedApi.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace PegasusMedApi.Server.Controllers;

/// <summary>
/// Data transfer object for creating a medical supply request.
/// </summary>
public record CreateRequestRequest(
    int ClientId, 
    string ItemDetails, 
    List<string> VendorIds, 
    bool IsFlagged
);

[ApiController]
[Route("api/requests")]
public class RequestsController : ControllerBase
{
    private readonly ApiDbContext _db;

    public RequestsController(ApiDbContext db) => _db = db;

    /// <summary>
    /// Retrieves a list of all medical supply requests, including vendor assignment statuses.
    /// </summary>
    /// <returns>A list of medical requests with their associated vendor data.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllRequests()
    {
        var requests = await _db.Requests
            .Include(r => r.VendorAssignments)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
            
        return Ok(requests);
    }

    /// <summary>
    /// Creates a new medical supply request and assigns it to specified vendors.
    /// </summary>
    /// <param name="req">The request payload from the client.</param>
    /// <returns>A response with the new request data.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateRequest(CreateRequestRequest req)
    {
        var entry = new MedRequest
        {
            ClientId = req.ClientId.ToString(),
            ItemDetails = req.ItemDetails,
            VendorAssignments = req.VendorIds.Select(vId => new VendorStatus
            {
                VendorId = vId,
                IsFlagged = req.IsFlagged
            }).ToList()
        };
        _db.Requests.Add(entry);
        await _db.SaveChangesAsync();
        return Ok(entry);
    }

    /// <summary>
    /// Retrieves a specific medical request by its unique integer ID.
    /// </summary>
    /// <param name="id">The integer ID of the request.</param>
    /// <returns>The request details including vendor assignments.</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRequestById(int id)
    {
        var request = await _db.Requests
            .Include(r => r.VendorAssignments)
            .FirstOrDefaultAsync(r => r.Id == id); 
        if (request == null) 
        {
            return NotFound();
        }
        return Ok(request);
    }

    /// <summary>
    /// Gets all medical requests assigned to a specific vendor.
    /// </summary>
    /// <param name="vendorId">The string ID of the vendor.</param>
    /// <returns>A list of summarized requests for the vendor.</returns>
    [HttpGet("vendor/{vendorId}")]
    public async Task<IActionResult> GetRequestsForVendor(string vendorId)
    {
        var allRequests = await _db.Requests
            .Include(r => r.VendorAssignments)
            .ToListAsync();
        var results = new List<object>();
        foreach (var req in allRequests)
        {
            foreach (var assignment in req.VendorAssignments)
            {
                if (assignment.VendorId == vendorId)
                {
                    var summary = new {
                        Id = req.Id,
                        ClientId = req.ClientId,
                        ItemDetails = req.ItemDetails,
                        CreatedAt = req.CreatedAt,
                        IsFlagged = assignment.IsFlagged
                    };
                    results.Add(summary);
                }
            }
        }
        if (results.Count == 0)
        {
            return NotFound();
        }
        return Ok(results);
    }

    /// <summary>
    /// Updates the acknowledgment status for a specific vendor on a request.
    /// </summary>
    /// <param name="id">The integer ID of the request.</param>
    /// <param name="vendorId">The string ID of the vendor.</param>
    /// <returns>A confirmation message if updated successfully.</returns>
    [HttpPatch("{id:int}/vendor/{vendorId}/acknowledge")]
    public async Task<IActionResult> Acknowledge(int id, string vendorId)
    {
        var assignment = await _db.VendorStatuses
            .FirstOrDefaultAsync(v => v.MedRequestId == id && v.VendorId == vendorId);

        if (assignment == null)
        {
            return NotFound();
        }
        assignment.AcknowledgedAt = DateTime.UtcNow;
        assignment.IsFlagged = true; 
        await _db.SaveChangesAsync();
        return Ok(new { message = "Update successful" });
    }

    /// <summary>
    /// Retrieves a unique list of all Vendor IDs from the existing assignments.
    /// </summary>
    [HttpGet("vendors")]
    public async Task<IActionResult> GetVendors()
    {
        // This query gets all unique vendor IDs currently in your database
        var vendors = await _db.VendorStatuses
            .Select(v => v.VendorId)
            .Distinct()
            .ToListAsync();

        // Returning as a simple list of strings to match your current logic
        return Ok(vendors);
    }
}
