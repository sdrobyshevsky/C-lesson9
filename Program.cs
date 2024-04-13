// Разработка приложения на C# (семинары)
// Урок 9. Сериализация
// Напишите приложение, конвертирующее произвольный JSON в XML. Используйте JsonDocument.

using System;
using System.Text.Json;
using System.Xml;

class JsonToXmlConverter
{
    public string ConvertJsonToXml(string jsonString)
    {
        var jsonDoc = JsonDocument.Parse(jsonString);

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml("<root></root>");

        var root = xmlDoc.DocumentElement;

        ConvertJsonNodeToXmlElement(jsonDoc.RootElement, root);

        return xmlDoc.InnerXml;
    }

    private void ConvertJsonNodeToXmlElement(JsonElement jsonElement, XmlElement parentElement)
    {
        foreach (var property in jsonElement.EnumerateObject())
        {
            XmlElement newElement = parentElement.OwnerDocument.CreateElement(property.Name);

            if (property.Value.ValueKind == JsonValueKind.Object)
            {
                ConvertJsonNodeToXmlElement(property.Value, newElement);
            }
            else if (property.Value.ValueKind == JsonValueKind.Array)
            {
                foreach (var arrayElement in property.Value.EnumerateArray())
                {
                    ConvertJsonNodeToXmlElement(arrayElement, newElement);
                }
            }
            else
            {
                newElement.InnerText = property.Value.ToString();
            }

            parentElement.AppendChild(newElement);
        }
    }
}

class Program
{
    static void Main()
    {
        string jsonString = "{\"name\":\"John\",\"age\":30,\"cars\":{\"car1\":\"Ford\",\"car2\":\"BMW\"}}";

        JsonToXmlConverter converter = new JsonToXmlConverter();
        string xmlString = converter.ConvertJsonToXml(jsonString);

        Console.WriteLine(xmlString);
    }
}