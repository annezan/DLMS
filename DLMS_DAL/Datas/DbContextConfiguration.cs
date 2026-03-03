using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS_DAL
{
    public class DbContextConfiguration
    {
        public static IConfigurationRoot Configuration { get; private set; }

        public static void Initialize(string configFilePath)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(configFilePath)
                .Build();
        }
    }
}
