using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using xml_json.Models;
using Formatting = Newtonsoft.Json.Formatting;

namespace xml_json.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Conversion conversion)
        {
            if (string.IsNullOrWhiteSpace(conversion.XmlText))
            {
                return View();
            }
            const string prefixAttributes = "@";
            var doc = new XmlDocument();
            doc.LoadXml(conversion.XmlText);

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var jsonFromXml = JsonConvert.SerializeXmlNode(doc, Formatting.None, true);
            var jsonObject = JsonConvert.DeserializeObject<ExpandoObject>
                (jsonFromXml.Replace($"\"{prefixAttributes}", "\""));
            conversion.JsonText = JsonConvert.SerializeObject(jsonObject, Formatting.Indented, jsonSerializerSettings);

            ModelState.Clear();
            return View(conversion);
        }
    }
}