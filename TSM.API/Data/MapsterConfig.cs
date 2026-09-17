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

            TypeAdapterConfig<AppRolesScreen, AppRolesScreenDto>.NewConfig()
            .Map(dest => dest.RoleName, src => src.RoleObject != null ? src.RoleObject.RoleName : null) 
            .Map(dest => dest.ScreenName, src => src.ScreenObject != null ? src.ScreenObject.ScreenName : null);


            TypeAdapterConfig<AppUserRole, AppUserRoleDto>.NewConfig()
            .Map(dest => dest.RoleName, src => src.RoleObject != null ? src.RoleObject.RoleName : null)
            .Map(dest => dest.FullName, src => src.UserObject != null ? src.UserObject.FullName : null);
        }
    }
}
