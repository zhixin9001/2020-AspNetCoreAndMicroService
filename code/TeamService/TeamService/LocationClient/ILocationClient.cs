using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamService.Entities;

namespace TeamService.LocationClient
{
    public interface ILocationClient
    {
        Task<LocationRecord> GetLatestForMember(Guid memberId);
        Task<LocationRecord> AddLocation(Guid memberId, LocationRecord locationRecord);
    }
}
