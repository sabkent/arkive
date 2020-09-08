using System;
using System.Collections.Generic;
using System.Text;

namespace pws.Shared.Hubs
{
    public class ConfigurationSettings
    {
        public static string HubUrl => "http://localhost:5001/hubs/myhub";

        public static class Events
        {
            public static string TimeSent => nameof(IMyHub.SpreadAsync);
        }
    }
}
