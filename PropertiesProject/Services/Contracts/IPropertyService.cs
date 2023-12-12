using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IPropertyService
    {
        /// <summary>
        /// Will be used to import object into the DB
        /// </summary>

        void AddProperty(string district, int floor, int totalFloor, int size, int yardSize, int year, string propertyType, string buildingType, int Price);

        /// <summary>
        /// Will be used to export view model after search by given criteria
        /// </summary>

        IEnumerable<PropertyInfoModel> Search(int minPrice, int maxPrice, int minSize, int maxSize);

        IEnumerable<PropertyFullDataModel> GetPropertyFullDataModels(int count);

        decimal AveragePricePerM2();

        decimal AveragePricePerM2(int? districtId);

        double AverageSize(int? districtId);
    }
}
