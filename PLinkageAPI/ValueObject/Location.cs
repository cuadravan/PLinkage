using PLinkageShared.Enums;

namespace PLinkageAPI.ValueObject
{
    public record Location(double Latitude, double Longitude)
    {
        public double DistanceTo(Location other)
        {
            var R = 6371; // Earth radius in km
            var dLat = (other.Latitude - Latitude) * Math.PI / 180;
            var dLon = (other.Longitude - Longitude) * Math.PI / 180;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(Latitude * Math.PI / 180) *
                    Math.Cos(other.Latitude * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);    

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
        // We declare a static method accessible by entities
        // It is a factory method that creates Location object
        // Why use factory method? We still need to map the coordinates of the CebuLocation enum
        public static Location From(CebuLocation location) =>
            new Location(
                CebuLocationCoordinates.Map[location].Latitude,
                CebuLocationCoordinates.Map[location].Longitude
            );
    }    
}
