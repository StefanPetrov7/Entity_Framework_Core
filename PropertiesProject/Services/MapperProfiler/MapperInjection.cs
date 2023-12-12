using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MapperProfiler
{
    public abstract class MapperInjection
    {
        /// <summary>
        /// Will be used to inject mapper into other classes
        /// </summary>
        /// 
        public MapperInjection()
        {
            InitializeAutoMapper();
        }

        protected IMapper Mapper { get; private set; }

        private void InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AppProfiler>();
            });

            this.Mapper = config.CreateMapper();
        }
    }
}
