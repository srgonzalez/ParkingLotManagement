using Microsoft.AspNetCore.Mvc;
using Packt.Shared;
using ParkingLotService.Repositories;
namespace ParkingLotService.Controllers
{
    // base address: api/parkingspots 
    [Route("api/[controller]")]
  [ApiController]
  public class ParkingSpotsController : ControllerBase
  {
    private IParkingSpotRepository repo;
    // constructor injects repository registered in Startup
    public ParkingSpotsController(IParkingSpotRepository repo)
    {
      this.repo = repo;
    }
    // GET: api/parkingspots
    // GET: api/parkingspots/?licensePlate=[licensePlate]
    // this will always return a list of parking spots even if its empty
    [HttpGet]
    [ProducesResponseType(200, 
      Type = typeof(IEnumerable<ParkingSpot>))]
    public async Task<IEnumerable<ParkingSpot>> GetParkingSpots(
      string? licensePlate)
    {
      if (string.IsNullOrWhiteSpace(licensePlate))
      {
        return await repo.RetrieveAllAsync();
      }
      else
      {
        return (await repo.RetrieveAllAsync())
          .Where(slot => slot.LicensePlate == licensePlate);
      }
    }
    // GET: api/parkingspots/[id] 
    [HttpGet("{id}", Name = nameof(GetParkingSpot))] // named route
    [ProducesResponseType(200, Type = typeof(ParkingSpot))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetParkingSpot(string id)
    {
      ParkingSpot c = await repo.RetrieveAsync(id); 
      if (c == null)
      {
        return NotFound(); // 404 Resource not found
      }
      return Ok(c); // 200 OK with parkingspot in body
    }
    // POST: api/parkingspots
    // BODY: Customer (JSON, XML) 
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(ParkingSpot))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] ParkingSpot c)
    {
      if (c == null)
      {
        return BadRequest(); // 400 Bad request
      }
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState); // 400 Bad request
      }
      ParkingSpot added = await repo.CreateAsync(c);
      return CreatedAtRoute( // 201 Created
        routeName: nameof(GetParkingSpot),
        routeValues: new { id = added.Id },
        value: added);
    }
    // PUT: api/parkingspots/[id]
    // BODY: ParkingSpot (JSON, XML) 
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(
      string id, [FromBody] ParkingSpot c)
    {
      if (c == null || c.Id.ToString() != id)
      {
        return BadRequest(); // 400 Bad request
      }
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState); // 400 Bad request
      }
      var existing = await repo.RetrieveAsync(id.ToString());
      if (existing == null)
      {
        return NotFound(); // 404 Resource not found
      }
      await repo.UpdateAsync(id.ToString(), c);
      return new NoContentResult(); // 204 No content
    }
    // DELETE: api/parkingspots/[id] 
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(string id)
    {
      var existing = await repo.RetrieveAsync(id); 
      if (existing == null)
      {
        return NotFound(); // 404 Resource not found
      }
      bool? deleted = await repo.DeleteAsync(id);
      if (deleted.HasValue && deleted.Value) // short circuit AND
      {
        return new NoContentResult(); // 204 No content
      }
      else
      {
        return BadRequest( // 400 Bad request
          $"ParkingSpot {id} was found but failed to delete.");
      }
    }
  }
}