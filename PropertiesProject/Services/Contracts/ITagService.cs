using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface ITagService
    {
        /// <summary>
        /// Importing Tags into the DB
        /// </summary>

        void AddTag(string name, int? importance = null);

        void BulkTagProperties();

    }
}
