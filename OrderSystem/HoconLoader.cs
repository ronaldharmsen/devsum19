using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AkkaUtilities
{
    public static class HoconLoader
    {
        public static Config FromFile(string hoconPath)
        {
            return ConfigurationFactory.ParseString(File.ReadAllText(hoconPath));
        }
    }
}
