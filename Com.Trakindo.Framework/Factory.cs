using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Trakindo.Framework
{
    public class Factory
    {
        protected static Dictionary<string, object> loadedLibraries = new Dictionary<string, object>();
        public static object Create(string key, ClassType classType)
        {
            string xml = System.IO.File.ReadAllText(GetFileName());
            Configurations configurations = Com.Trakindo.Utility.Serializer.XML.Deserialize<Configurations>(xml);
            FactoryConfiguration fc = GetFromList(configurations, key, classType);
            if(fc != null)
            {
                string folderName = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                IEnumerable<System.Reflection.Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic && a.ManifestModule.Name == fc.AssemblyInfo.DllFile);
                string folderName2 = "";
                if (assemblies.Count<System.Reflection.Assembly>() > 0)
                    folderName2 = assemblies.Select(a => a.Location).FirstOrDefault();
                string filename = folderName + "Resources\\Dlls\\" + fc.AssemblyInfo.DllFile;
                if (folderName2.Length > 0)
                    filename = folderName2;
                object obj = Com.Trakindo.Utility.Assembly.LoadObject(filename, fc.AssemblyInfo.ClassName);
                return obj;
            }
            return null;
        }

        public static T Create<T>(string key, ClassType classType)
        {
            object oo = null;
            T obj = (T)oo;
            string xml = System.IO.File.ReadAllText(GetFileName());
            Configurations configurations = Com.Trakindo.Utility.Serializer.XML.Deserialize<Configurations>(xml);
            FactoryConfiguration fc = GetFromList(configurations, key, classType);
            if (fc != null)
            {
                string folderName = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                IEnumerable<System.Reflection.Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic && a.ManifestModule.Name == fc.AssemblyInfo.DllFile);
                string folderName2 = "";
                if(assemblies.Count<System.Reflection.Assembly>() > 0)
                    folderName2 = assemblies.Select(a => a.Location).FirstOrDefault();
                string filename = folderName + "Resources\\Dlls\\" + fc.AssemblyInfo.DllFile;
                if(folderName2.Length > 0)
                    filename = folderName2 ;

                obj = (T)Com.Trakindo.Utility.Assembly.LoadObject(filename, fc.AssemblyInfo.ClassName);
                return obj;
            }

            return obj;
        }

        static FactoryConfiguration GetFromList(Configurations configs, string key, ClassType classType)
        {
            foreach(FactoryConfiguration fc in configs.FactoryConfigurations)
            {
                if(fc.AssemblyType.Key.ToLower().Equals(key.ToLower()) && fc.AssemblyType.ClassType == classType)
                {
                    return fc;
                }
            }

            return null;
        }


        static string GetFileName()
        {
            //string folderName = System.IO.Directory.GetCurrentDirectory();
            string folderName = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            string fileName = folderName + "Resources\\Config\\Com.Trakindo.Factory.Config";
            if (System.IO.File.Exists(fileName))
                return fileName;
            else
            {
                string[] files = System.IO.Directory.GetFiles(folderName + "Resources\\Config\\", "*.config");
                string[] files2 = System.IO.Directory.GetFiles(folderName + "Resources\\Config\\", "*.xml");
                if (files.Length > 0)
                    return files[0];
                else if (files2.Length > 0)
                    return files2[0];
            }

            return fileName;
        }
    }
}
