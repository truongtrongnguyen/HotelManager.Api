using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Entities.Message
{
    public static class ErrorMessage
    {
        public static class Generic
        {
            public static string ObjectNotFound = "Object Not Found";
            public static string InvalidRequest = "Invalid Request";
            public static string TypeBadRequest = "Bad Request";
            public static string InvalidPayload = "Invalid Payload";
            public static string SomethingWentWrong = "Something went wrong, please try again latter";
            public static string UnableToProcess = "Unable to process Request";
        }

        public static class UserMessage
        {
            public static string UserNotFound = "User Not Found";
        }
    }
}
