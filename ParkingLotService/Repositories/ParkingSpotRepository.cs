using Microsoft.EntityFrameworkCore.ChangeTracking;
using Packt.Shared;
using System.Collections.Concurrent;
namespace ParkingLotService.Repositories
{
    public class ParkingSpotRepository : IParkingSpotRepository
  {
    // use a static thread-safe dictionary field to cache the customers
    private static ConcurrentDictionary
      <string, ParkingSpot> parkingSlotCache;
    // use an instance data context field because it should not be
    // cached due to their internal caching
    private ParkingLot db;
    public ParkingSpotRepository(ParkingLot db)
    {
      this.db = db;
      // pre-load customers from database as a normal
      // Dictionary with CustomerID as the key,
      // then convert to a thread-safe ConcurrentDictionary
      if (parkingSlotCache == null)
      {
        parkingSlotCache = new ConcurrentDictionary<string, ParkingSpot>(
          db.ParkingSpots.ToDictionary(c => c.Id.ToString()));
      }
    }
    public async Task<ParkingSpot> CreateAsync(ParkingSpot c)
    {
      // add to database using EF Core
      EntityEntry<ParkingSpot> added = await db.ParkingSpots.AddAsync(c);
      int affected = await db.SaveChangesAsync();
      if (affected == 1)
      {
        // if the customer is new, add it to cache, else
        // call UpdateCache method
        return parkingSlotCache.AddOrUpdate(c.Id.ToString(), c, UpdateCache);
      }
      else
      {
        return null;
      }
    }
    public Task<IEnumerable<ParkingSpot>> RetrieveAllAsync()
    {
      // for performance, get from cache
      return Task.Run<IEnumerable<ParkingSpot>>(
        () => parkingSlotCache.Values);
    }
    public Task<ParkingSpot?> RetrieveAsync(string id)
    {
      return Task.Run(() =>
      {
        // for performance, get from cache
        parkingSlotCache.TryGetValue(id.ToString(), out ParkingSpot? c);
        return c;
      });
    }
    private ParkingSpot UpdateCache(string id, ParkingSpot c)
    {
      ParkingSpot old;
      if (parkingSlotCache.TryGetValue(id, out old))
      {
        if (parkingSlotCache.TryUpdate(id, c, old))
        {
          return c;
        }
      }
      return null;
    }
    public async Task<ParkingSpot> UpdateAsync(string id, ParkingSpot c)
    {
      // normalize customer ID
      // id = id.ToUpper();
      // c.CustomerID = c.CustomerID.ToUpper();
      // update in database
      db.ParkingSpots.Update(c);
      int affected = await db.SaveChangesAsync();
      if (affected == 1)
      {
        // update in cache
        return UpdateCache(id, c);
      }
      return null;
    }
    public async Task<bool?> DeleteAsync(string id)
    {
      // remove from database
      ParkingSpot c = db.ParkingSpots.Find(id.ToString());
      db.ParkingSpots.Remove(c);
      int affected = await db.SaveChangesAsync();
      if (affected == 1)
      {
        // remove from cache
        return parkingSlotCache.TryRemove(id, out c);
      }
      else
      {
        return null;
      }
    }
  }
}