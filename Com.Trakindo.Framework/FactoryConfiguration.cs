using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Com.Trakindo.Utility;

namespace Com.Trakindo.Framework
{

    public class Temp
    {
        public static void SaveSampleAssemblies()
        {
            Configurations configs = new Configurations();

            configs.FactoryConfigurations.Add(new FactoryConfiguration( new AssemblyInfo("Com.Trakindo.Gateway.UserManagement.Data.dll", "Com.Trakindo.Gateway.UserManagement.Repository.RecipeRepository"),
                new AssemblyType("Recipe", ClassType.clsTypeRepository), "Recipe Repository"));
            configs.FactoryConfigurations.Add(new FactoryConfiguration(new AssemblyInfo("Com.Trakindo.Gateway.UserManagement.Data.dll", "Com.Trakindo.Gateway.UserManagement.Repository.FoodRepository"),
               new AssemblyType("Food", ClassType.clsTypeRepository), "Food Repository"));

            string xml = Serializer.XML.Serialize(configs);
            System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/Com.Trakindo.Gateway.UserManagement.Webservice.Config", xml);
        }
    }

    public enum ClassType
    {
        clsTypeDataContext=1,
        clsTypeDataModel=2,
        clsTypeRepository=3,
        clsTypeBusinessService=4,
        clsTypeFormProcessor=5,
        clsTypeFormValidator=6,
        clsTypeForm=7
    }

    [XmlRoot("Configurations")]
    public class Configurations
    {
        protected List<FactoryConfiguration> factoryConfigurations;

        [XmlArray("FactoryConfigurations")]
        public List<FactoryConfiguration> FactoryConfigurations
        {
            get
            {
                if (factoryConfigurations == null)
                    factoryConfigurations = new List<FactoryConfiguration>();
                return factoryConfigurations;
            }
        }
    }

    public class AssemblyInfo
    {
        [XmlAttribute("DllFile")]
        public String DllFile { get; set; }

        [XmlAttribute("ClassName")]
        public String ClassName { get; set; }

        public AssemblyInfo()
        {

        }

        public AssemblyInfo(string dllFile, string className)
        {
            this.DllFile = dllFile;
            this.ClassName = className;
        }
    }

    public class AssemblyType
    {
        [XmlAttribute("Key")]
        public String Key { get; set; }

        [XmlAttribute("ClassType")]
        public ClassType ClassType { get; set; }

        public AssemblyType()
        {

        }

        public AssemblyType(string Key, ClassType classType)
        {
            this.Key = Key;
            this.ClassType = classType;
        }
    }

    public class FactoryConfiguration
    {
        [XmlElement("AssemblyInfo")]
        public AssemblyInfo AssemblyInfo { get; set; }

        [XmlElement("Info")]
        public String Info { get; set; }

        [XmlElement("AssemblyType")]
        public AssemblyType AssemblyType { get; set; }

        public FactoryConfiguration()
        {

        }

        public FactoryConfiguration(AssemblyInfo assemblyInfo, AssemblyType assemblyType, string info)
        {
            this.AssemblyInfo = assemblyInfo;
            this.AssemblyType = assemblyType;
            this.Info = info;
        }
    }
}
