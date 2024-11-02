using System.Text.Json;

namespace ProductStorageApp.Utils
{
    public static class Utils
    {
        public static string ordersPath = "orders.json";
        public static List<T> getObjects<T>(string filePath)
        {
            var jsonString = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(jsonString);
        }
        public static void writeObjects<T>(List<T> objects, string filePath)
        {
            var jsonString = JsonSerializer.Serialize(objects, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            System.IO.File.WriteAllText(filePath, jsonString);
        }
    }
}
