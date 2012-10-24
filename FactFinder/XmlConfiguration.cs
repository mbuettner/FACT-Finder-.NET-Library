﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Omikron.FactFinder
{
    public class XmlConfiguration : IConfiguration
    {
        public bool IsDebugModeOn { get; private set; }

        public string FactFinderVersion { get; private set; }

        public string RequestProtocol { get; private set; }
        public string ServerAddress { get; private set; }
        public string ServerPort { get; private set; }
        public string Context { get; private set; }
        public string Channel { get; private set; }
        public string Language { get; private set; }

        public string User { get; private set; }
        public string Password { get; private set; }
        public AuthenticationType AuthenticationType { get; private set; }
        public string AdvancedAuthPrefix { get; private set; }
        public string AdvancedAuthPostfix { get; private set; }

        public IDictionary<string, string> ClientMappings { get; private set; }
        public IDictionary<string, string> ServerMappings { get; private set; }
        public ICollection<string> IgnoredClientParams { get; private set; }
        public ICollection<string> IgnoredServerParams { get; private set; }
        public IDictionary<string, string> RequiredClientParams { get; private set; }
        public IDictionary<string, string> RequiredServerParams { get; private set; }

        public string PageContentEncoding { get; private set; }
        public string PageUrlEncoding { get; private set; }
        public string ServerUrlEncoding { get; private set; }

        private IDictionary<string, string> CustomValues { get; set; }

        public XmlConfiguration(string fileName)
        {
            try
            {
                XElement configuration = XElement.Load(fileName);

                IsDebugModeOn = Convert.ToBoolean((string)configuration.Element("debug"));
                RequestProtocol = (string)configuration.Element("protocol");
                ServerAddress = (string)configuration.Element("search").Element("address");
                ServerPort = (string)configuration.Element("search").Element("port");
                Context = (string)configuration.Element("search").Element("context");
                Channel = (string)configuration.Element("search").Element("channel");

                User = (string)configuration.Element("search").Element("auth").Element("user");
                Password = (string)configuration.Element("search").Element("auth").Element("password");
                
                string typeString = (string)configuration.Element("search").Element("auth").Element("type");

                switch (typeString)
                {
                    case "http":
                        AuthenticationType = AuthenticationType.Http;
                        break;
                    case "simple":
                        AuthenticationType = AuthenticationType.Simple;
                        break;
                    case "advanced":
                        AuthenticationType = AuthenticationType.Advanced;
                        break;
                }

                AdvancedAuthPrefix = (string)configuration.Element("search").Element("auth").Element("advancedPrefix");
                AdvancedAuthPostfix = (string)configuration.Element("search").Element("auth").Element("advancedPostfix");

                Language = (string)configuration.Element("search").Element("language");

                IgnoredServerParams = (
                    from element
                    in configuration.Element("params").Element("server").Elements("ignore")
                    select (string)element.Attribute("name")
                ).ToList();

                IgnoredClientParams = (
                    from element
                    in configuration.Element("params").Element("client").Elements("ignore")
                    select (string)element.Attribute("name")
                ).ToList();

                RequiredServerParams = new Dictionary<string, string>();

                foreach (XElement element in configuration.Element("params").Element("server").Elements("require"))
                {
                    RequiredServerParams.Add((string)element.Attribute("name"), (string)element.Attribute("default"));
                }

                RequiredClientParams = new Dictionary<string, string>();

                foreach (XElement element in configuration.Element("params").Element("client").Elements("require"))
                {
                    RequiredClientParams.Add((string)element.Attribute("name"), (string)element.Attribute("default"));
                }

                ServerMappings = new Dictionary<string, string>();

                foreach (XElement element in configuration.Element("params").Element("server").Elements("mapping"))
                {
                    ServerMappings.Add((string)element.Attribute("from"), (string)element.Attribute("to"));
                }

                ClientMappings = new Dictionary<string, string>();

                foreach (XElement element in configuration.Element("params").Element("client").Elements("mapping"))
                {
                    ClientMappings.Add((string)element.Attribute("from"), (string)element.Attribute("to"));
                }

                PageContentEncoding = (string)configuration.Element("encoding").Element("pageContent");
                PageUrlEncoding = (string)configuration.Element("encoding").Element("pageURI");
                ServerUrlEncoding = (string)configuration.Element("encoding").Element("serverURI");

                CustomValues = new Dictionary<string, string>();

                foreach (XElement element in configuration.Element("custom").Elements())
                {
                    CustomValues.Add(element.Name.LocalName, (string)element);
                }
            }
            catch (System.Xml.XmlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public string GetCustomValue(string key)
        {
            return CustomValues[key];
        }
    }
}