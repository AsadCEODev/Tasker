using System;
using System.Collections.Generic;
using System.Text;

namespace TMS.Shared.Enum
{
    public enum AppScreenEnum
    {
        Projects = 1,
        Settings = 2,
        Departments = 3,
        Designations = 4,
        Users = 5,
        AssignProjects = 6,
        CreateRoles = 7,
        AssignRoles = 8,
        CreateTasks = 9,
        TodoTasks = 10,
        Calendar = 11,
        Chat = 12,
        UsersChat = 13,
        Channels = 14,
        Notifications = 15,
        UserProfile = 16,
        Inbox = 17
    }

    public static class AppScreenExtensions
    {
        public static string GetScreenName(this AppScreenEnum screen)
        {
            return screen switch
            {
                AppScreenEnum.Projects => "Projects",
                AppScreenEnum.Settings => "Settings",
                AppScreenEnum.Departments => "Departments",
                AppScreenEnum.Designations => "Designations",
                AppScreenEnum.Users => "Users",
                AppScreenEnum.AssignProjects => "Assign Projects",
                AppScreenEnum.CreateRoles => "Create Roles",
                AppScreenEnum.AssignRoles => "Assign Roles",
                AppScreenEnum.CreateTasks => "Create Tasks",
                AppScreenEnum.TodoTasks => "Todo Tasks",
                AppScreenEnum.Calendar => "Calendar",
                AppScreenEnum.Chat => "Chat",
                AppScreenEnum.UsersChat => "Users Chat",
                AppScreenEnum.Channels => "Channels",
                AppScreenEnum.Notifications => "Notifications",
                AppScreenEnum.UserProfile => "User Profile",
                AppScreenEnum.Inbox => "Inbox",
                _ => screen.ToString()
            };
        }
    }
}
