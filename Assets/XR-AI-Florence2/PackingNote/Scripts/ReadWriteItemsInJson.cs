using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class ReadWriteItemsInJson : MonoBehaviour
{
    // Public list for runtime modifications in Unity Editor
    public string box1JsonFileName = "PackingNoteBox1";
    public string box2JsonFileName = "PackingNoteBox2";
    public string box3JsonFileName = "PackingNoteBox3";

    // Method to load items from a JSON file
    public List<string> LoadItemsFromJson(string fileName)
    {
        try
        {
            // Construct the file path just like in WriteItemsToJson
            string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");

            // Debug to log the file path being accessed
            Debug.Log($"Attempting to load JSON file from: {filePath}");

            // Check if the file exists before attempting to read it
            if (!File.Exists(filePath))
            {
                Debug.LogError($"File not found: {filePath}");
                return new List<string>(); // Return an empty list if the file is missing
            }

            // Read the JSON content from the file
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON content into a dictionary
            var itemList = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

            // Return the list of items if available; otherwise, return an empty list
            return itemList != null && itemList.ContainsKey("Items") ? itemList["Items"] : new List<string>();
        }
        catch (Exception ex)
        {
            // Log an error if something goes wrong
            Debug.LogError($"Error reading JSON from file: {ex.Message}");
            return new List<string>();
        }
    }


    
    // Method to write items to a JSON file
    public void WriteItemsToJson(string fileName, List<string> items)
    {
        try
        {
            // Construct the writable path on the platform
            string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");

            // Create a dictionary to store the list of items
            var itemList = new Dictionary<string, List<string>> { { "Items", items } };

            // Serialize the dictionary into JSON format with nice indentation
            string json = JsonConvert.SerializeObject(itemList, Formatting.Indented);

            // Debug the path where the file will be written
            Debug.Log($"Writing JSON file at: {filePath}");

            // Write the JSON string to the specified file path
            File.WriteAllText(filePath, json);

            // Debug message for confirmation
            Debug.Log($"Items successfully written to JSON file at: {filePath}");
        }
        catch (Exception ex)
        {
            // Log error if something goes wrong
            Debug.LogError($"Error writing JSON to file: {ex.Message}");
        }
    }


}