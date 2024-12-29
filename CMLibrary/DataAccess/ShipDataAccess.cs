using CMLibrary.Models;

namespace CMLibrary.DataAccess
{
    public class ShipDataAccess : IShipDataAccess
    {
        private readonly ISqlDataAccess _sql;

        public ShipDataAccess(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public async Task<List<CruiseLineModel>> GetCruiseLines()
        {
            // Option A: using stored procedure
            var results = await _sql.LoadData<CruiseLineModel, dynamic>(
                "dbo.spCruiseline_GetAll",
                new { },
                "Default"
            );

            return results.ToList();
        }

        public async Task<List<ShipModel>> GetShips(int? cruiseLineId = null)
        {
            var results = await _sql.LoadData<ShipModel, dynamic>(
                "dbo.spCruiseShip_GetAll",
                new { CruiselineId = cruiseLineId },
                "Default"
            );

            return results.ToList();
        }

        public async Task<List<ShipItineraryFlatModel>> GetShipItineraryFlat(int shipId)
        {
            var results = await _sql.LoadData<ShipItineraryFlatModel, dynamic>(
                "dbo.spShipItinerary_GetFlat",
                new { ShipId = shipId },
                "Default"
            );

            return results.ToList();
        }

        // Fill ShipModel + List<ShipScheduleModel>
        public async Task<ShipModel?> GetShipItineraryStructured(int shipId)
        {
            // Step 1: get the FLAT rows
            var flatRows = await GetShipItineraryFlat(shipId);
            if (!flatRows.Any())
                return null;

            // Step 2: build the ShipModel from the first row
            var first = flatRows.First();
            var ship = new ShipModel
            {
                ShipId = first.ShipId,
                ShipName = first.Ship,
                CruiselineId = first.ShipId, // or first.CruiselineId if you store it
                CruiselineName = first.Cruiseline,
                ShipSchedules = new List<ShipScheduleModel>()
            };

            // Step 3: convert each flat row to a ShipScheduleModel
            foreach (var row in flatRows)
            {
                // Combine arrival date/time into a single DateTime if both present
                var arrivalDateTime = CombineDateTime(row.ArrivalDate, row.ArrivalTime);
                var departureDateTime = CombineDateTime(row.DepartureDate, row.DepartureTime);

                ship.ShipSchedules.Add(new ShipScheduleModel
                {
                    PortId = row.PortId,
                    PortName = row.PortName,
                    PortCountry = row.PortCountry,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                    ArrivalDateTime = arrivalDateTime,
                    DepartureDateTime = departureDateTime
                });
            }

            return ship;
        }

        private DateTime? CombineDateTime(DateTime? date, TimeSpan? time)
        {
            if (date.HasValue && time.HasValue)
                return date.Value.Date + time.Value;

            return null;
        }
    }
}
