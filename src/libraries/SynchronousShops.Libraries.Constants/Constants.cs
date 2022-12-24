namespace SynchronousShops.Libraries.Constants
{
    public static class Project
    {
        public const string Name = "Synchronous Shops";
    }

    public static class Api
    {
        public static class V1
        {
            public static class Authentication
            {
                public const string Url = "/api/v1/authentication";
                public const string Register = Url + "/register";
                public const string ConfirmRegistrationEmail = Url + "/confirmregistrationemail";
                public const string ConfirmInvitationEmail = Url + "/confirminvitationemail";
                public const string ResendEmailConfirmation = Url + "/resendemailconfirmation";
                public const string ResetPassword = Url + "/resetpassword";
                public const string PasswordForgotten = Url + "/passwordforgotten";
                public const string Login = Url + "/login";
            }

            public static class Account
            {
                public const string Url = "/api/v1/account/";
                public const string Password = Url + "password";
                public const string Profile = Url + "profile";
            }

            public static class AccountChild
            {
                public const string Url = "/api/v1/account-child";
                public const string Password = Url + "password";
                public const string Profile = Url + "profile";
            }

            public static class User
            {
                public const string Url = "/api/v1/user";
                public const string Lock = Url + "/{id:guid}/lock";
                public const string Unlock = Url + "/{id:guid}/unlock";
            }

            public static class Role
            {
                public const string Url = "/api/v1/role";
            }

            public static class Item
            {
                public const string Url = "/api/v1/item";
            }

            public static class NotificationStatus
            {
                public const string Url = "/api/v1/notification";
                public const string Read = Url + "/{id:guid}/read";
            }

            public static class Hub
            {
                public const string Url = "/hub";
            }
        }
    }
}