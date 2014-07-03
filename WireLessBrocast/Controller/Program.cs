using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controller
{
    class Program
    {
     public  static  Controller controller;
      static Config config;
        static void Main(string[] args)
        {
            System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(typeof(Config));
            if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "config.xml"))
            {
                Config config = new Config() {id=1, WirelessComPort = "Com14", TouhPanelComPort = "Com18" };
            
                sr.Serialize(System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "config.xml"), config);
                Console.WriteLine("please modify the config.xml");
             //   Console.ReadKey();
                Environment.Exit(-1);
            }
            else
            {
                config = sr.Deserialize(System.IO.File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "config.xml")) as Config;
                if (config == null)
                {
                    Console.WriteLine("config.xml  reading error!");
                    Environment.Exit(-1);
                }
            }
            controller = new Controller(config.id,config.WirelessComPort,config.TouhPanelComPort);

            
        }
    }
}
