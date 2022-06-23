using Packt.Shared;
namespace ParkingLotService.Repositories
{
    public interface IParkingSpotRepository
  {
    Task<ParkingSpot> CreateAsync(ParkingSpot s);
    Task<IEnumerable<ParkingSpot>> RetrieveAllAsync();
    Task<ParkingSpot> RetrieveAsync(string id);
    Task<ParkingSpot> UpdateAsync(string id, ParkingSpot c);
    Task<bool?> DeleteAsync(string id);
  }
}