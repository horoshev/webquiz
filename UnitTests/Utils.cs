using AutoMapper;
using Web;

namespace Tests
{
    public class Utils
    {
        private static readonly MapperConfiguration MapperConfiguration =
            new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));

        public static readonly IMapper Mapper = MapperConfiguration.CreateMapper();
    }
}