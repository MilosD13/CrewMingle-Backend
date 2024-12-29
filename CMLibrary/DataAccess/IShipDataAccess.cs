namespace CMLibrary.DataAccess
{
    using CMLibrary.Models;

    public interface IShipDataAccess
    {
        Task<List<CruiseLineModel>> GetCruiseLines();
        Task<List<ShipModel>> GetShips(int? cruiseLineId = null);
        Task<List<ShipItineraryFlatModel>> GetShipItineraryFlat(int shipId);
        Task<ShipModel?> GetShipItineraryStructured(int shipId);
    }
}
