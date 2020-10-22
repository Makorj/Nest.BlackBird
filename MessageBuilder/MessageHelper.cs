using System;
using System.Collections.Generic;
using System.Text;

namespace BlackBird
{
    public static class MessageHelper
    {
        public static class HAWK
        {
            public readonly static string BASE = "HAWK"; 
            public readonly static string VALID = BASE + " VALID";
            public readonly static string DISCONNECT = BASE + " DC";
            public readonly static string CONNECT = BASE + " CONNECT";

            public readonly static string OK = BASE + " OK";
            public static class ERROR
            {
                private readonly static string BASE = HAWK.BASE + " ERROR";

                public readonly static string USER_UNKNOWN = BASE + " USER UNKNOWN";
                public readonly static string USER_PERMISSION_NOT_GRANTED = BASE + " USER PERMNOTGRTD";

                public readonly static string NO_AVAILABLE_WORKER = BASE + " NAVAILABLEWORKER";
            }

            public static class REQUEST
            {
                private readonly static string BASE = HAWK.BASE + " REQUEST";

                public readonly static string COMMAND_LIST = BASE + " COMMAND LIST";
            }

            public static class ORDER
            {
                public readonly static string BASE = HAWK.BASE + " ORDER";

                public readonly static string UPDATE = BASE + " UPDATE";
                public readonly static string BUILD = BASE + " BUILD";
                public readonly static string DEPLOY = BASE + " DEPLOY";
            }
            
        }

        public static class CLIENT
        {
            public readonly static string BASE = "CLIENT";
            public readonly static string DISCONNECT = BASE +" DC";
            public readonly static string USER = BASE + " USER";

            public static class REQUEST
            {
                private readonly static string BASE = CLIENT.BASE + " REQUEST";

                public readonly static string AVAILABLE_WORKER = BASE + " AVAILABLE_WORKER";
            }

            public static class SEND
            {
                private readonly static string BASE = CLIENT.BASE + " SEND";

                public readonly static string COMMAND_LIST = BASE + " COMMAND LIST";
            }

            public static string _USER(string login, string hasedPassword) => $"{CLIENT.USER} {login} {hasedPassword}";
        }

        public static class QUELEA
        {
            public readonly static string BASE = "QUELEA";

            public readonly static string READY = BASE + " READY";
            public readonly static string DONE = BASE + " DONE";
        }
    }
}
