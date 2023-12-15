using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ThreeLayerContract;

namespace Enity
{
    public class DLLReader
    {
        public List<IGUI> guis = new List<IGUI>();
        public List<IBus> buses = new List<IBus>();
        public List<IDAO> daos = new List<IDAO>();

        public DLLReader() 
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string folder = System.IO.Path.GetDirectoryName(exePath);
            FileInfo[] fis = new DirectoryInfo(folder).GetFiles("*.dll");

            // Load all assemblies from current working directory
            foreach (FileInfo fileInfo in fis)
            {
                var domain = AppDomain.CurrentDomain;
                Assembly assembly = domain.Load(AssemblyName.GetAssemblyName(fileInfo.FullName));

                // Get all of the types in the dll
                Type[] types = assembly.GetTypes();

                // Only create instance of concrete class that inherits from IGUI, IBus or IDao
                foreach (var type in types)
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        if (typeof(IGUI).IsAssignableFrom(type))
                            guis.Add(Activator.CreateInstance(type) as IGUI);
                        else if (typeof(IBus).IsAssignableFrom(type))
                            buses.Add(Activator.CreateInstance(type) as IBus);
                        else if (typeof(IDAO).IsAssignableFrom(type))
                            daos.Add(Activator.CreateInstance(type) as IDAO);
                    }
                }
            }
        }
        public IGUI GetGUI(string name)
        {
            IGUI gui = null;
            foreach (var g in guis)
            {
                if (g.Name() == name)
                {
                    gui = g as IGUI;
                    break;
                }
            }
            return gui;
        }
        public IBus GetBus(string name)
        {
            IBus bus = null;
            foreach (var b in buses)
            {
                if (b.Name() == name)
                {
                    bus = b as IBus;
                    break;
                }
            }
            return bus;
        }

        public IDAO GetDao(string name)
        {
            IDAO dao = null;
            foreach (var d in daos)
            {
                if (d.Name() == name)
                {
                    dao = d as IDAO;
                    break;
                }
            }
            return dao;
        }
    }
}
