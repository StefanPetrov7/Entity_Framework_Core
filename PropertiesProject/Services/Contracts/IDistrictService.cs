using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IDistrictService
    {
        IEnumerable<DistrictInfoModel> GetMostExpesiveDistricts(int count);
    }
}
