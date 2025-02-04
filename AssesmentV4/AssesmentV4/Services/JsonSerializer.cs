using AssesmentV4.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace AssesmentV4.Services
{
    public class JsonSerializer
    {
        public void SerializeToFile(List<Product> products, string filePath)
        {
            if (!File.Exists(filePath))
            {
                string json = JsonConvert.SerializeObject(products, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }
    }
}
