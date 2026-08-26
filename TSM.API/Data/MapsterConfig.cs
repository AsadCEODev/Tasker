using Mapster;
using TMS.Shared.Model;
using TMS.Shared.Model.Setup;

namespace TSM.API.Data
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<UserTask, UserTaskDto>
                .NewConfig()
                .Map(
                    dest => dest.UserName,
                    src => src.UserObj != null
                        ? src.UserObj.FullName
                        : null
                );

            TypeAdapterConfig<SetupTask, SetupTaskDto>
                .NewConfig();
        }
    }
}
