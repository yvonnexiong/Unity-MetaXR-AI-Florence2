using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxItemsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI boxTitleText;
    [SerializeField] private GameObject detectedItemsBtnsParent;
    [SerializeField] private GameObject itemsInTheBoxBtnsParent;
    [SerializeField] private GameObject buttonPrefab_DetectedItem;
    [SerializeField] private GameObject buttonPrefab_ItemInBox;
    [SerializeField] private GameObject buttonClear;
    [SerializeField] private GameObject buttonDone;
    [SerializeField] private PackingNoteSceneChanger sceneChanger;
    
    [SerializeField] private ReadWriteItemsInJson readWriteItemsInJson;
    private List<string> existingItemsInBox = new List<string>();
    private List<string> detectedItemsList = new List<string>();
    private string currentBoxName;
    private int currentBoxIndex; //Start from 1, not 0
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetBoxName(GameData.SelectedBoxName);
        SetBoxIndexAndTitle(GameData.SelectedBoxIndex);
        
        SetUpButtonListeners();

        LoadAndDisplayItemsInBox();

        //TODO: move this method to when Item detected
        // ResetBoxItems();//hard reset
        //
        // AddDetectedItemButton("mouse");
        // AddDetectedItemButton("knife");
        // AddDetectedItemButton("cup");
        // AddDetectedItemButton("book");
        // AddDetectedItemButton("bowl");
        //
    }

    void ResetBoxItems()
    {
        foreach (Transform child in itemsInTheBoxBtnsParent.transform)
        {
            Destroy(child.gameObject);
        }

        List<string> emptyList = new List<string>();
        existingItemsInBox = emptyList;
        readWriteItemsInJson.WriteItemsToJson(currentBoxName, existingItemsInBox);
        
    }

    private void SetUpButtonListeners()
    {
        // Button Clear
        var toggleClear = buttonClear.GetComponent<Toggle>();
        if (toggleClear != null)
        {
            toggleClear.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    ResetBoxItems(); /////// TODO: "EmptyBox" doesn't work properly, use "resetBoxItems" instead
                }
                
            });
        }
        else
        {
            Debug.LogError("Toggle component missing on the button buttonClear prefab.");
        }
        
        // Button Done
        var toggleDone = buttonClear.GetComponent<Toggle>();
        if (toggleDone != null)
        {
            toggleDone.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    sceneChanger.LoadSceneByName("PackingNoteDisplayMode");
                }
                
            });
        }
        else
        {
            Debug.LogError("Toggle component missing on the button buttonClear prefab.");
        }
    }

    private void LoadAndDisplayItemsInBox()
    {
        existingItemsInBox = readWriteItemsInJson.LoadItemsFromJson(currentBoxName);
        foreach (string item in existingItemsInBox)
        {
            AddItemButtonInBox(item);
            Debug.Log("Load and Display items in box: " + currentBoxName + ", added item: " + item);
        }
    }

    public void SetBoxName(string boxName)
    {
        currentBoxName = boxName;
    }
    public void SetBoxIndexAndTitle(int index)
    {
        currentBoxIndex = index;
        SetBoxTitle(index);
    }

    private void SetBoxTitle(int index)
    {
        boxTitleText.text = $"Items In the Box {index}:";    
    }

    // Main function called by the AI detection
    public void AddDetectedItemButton(string buttonText)
    {
        // Only add button when it's not in the box already
        if (existingItemsInBox.Contains(buttonText)) return;
        
        // Check if it's in the detect item list
        if (detectedItemsList.Contains(buttonText)) return;
        
        // Add button
        var newBtn = Instantiate(buttonPrefab_DetectedItem, detectedItemsBtnsParent.transform);

        // Validate BtnManager_AddItems component
        var btnManager = newBtn.GetComponent<BtnManager_AddItems>();
        if (btnManager != null)
        {
            btnManager.SetLabelText(buttonText);
        }
        else
        {
            Debug.LogError("BtnManager_AddItems component missing on the button prefab.");
            return;
        }

        // Validate Toggle component
        var toggle = newBtn.GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    RemoveDetectedItemButton(buttonText);
                    AddItemButtonInBox(buttonText);
                    existingItemsInBox.Add(buttonText);
                    readWriteItemsInJson.WriteItemsToJson(currentBoxName, existingItemsInBox);
                }
                
            });
        }
        else
        {
            Debug.LogError("Toggle component missing on the button prefab.");
        }
        
        // Add to detected items list
        detectedItemsList.Add(buttonText);
    }

    private void RemoveDetectedItemButton(string buttonText)
    {
        // Find the button object in the parent that matches the buttonText
        foreach (Transform child in detectedItemsBtnsParent.transform)
        {
            var btnManager = child.GetComponent<BtnManager_AddItems>();
            if (btnManager != null && btnManager.GetLabelText() == buttonText)
            {
                // Remove the button from the UI
                Destroy(child.gameObject);
    
                // Remove the item from the detected items list
                detectedItemsList.Remove(buttonText);
    
                return;
            }
        }
    
        Debug.LogWarning($"Button with label '{buttonText}' not found to remove.");
    }

    private void RemoveBoxItemButton(string buttonText)
    {
        // Check if the buttonText exists in the box item list
        if (!existingItemsInBox.Contains(buttonText))
        {
            Debug.LogWarning($"Button with label '{buttonText}' not found in the box item list.");
            return;
        }
    
        // Search for the button in the parent and remove it
        foreach (Transform child in itemsInTheBoxBtnsParent.transform)
        {
            var btnManager = child.GetComponent<BtnManager_AddItems>();
            if (btnManager != null && btnManager.GetLabelText() == buttonText)
            {
                // Remove the button from the UI
                Destroy(child.gameObject);
    
                // Remove the item from the list and update JSON
                existingItemsInBox.Remove(buttonText);
                readWriteItemsInJson.WriteItemsToJson(currentBoxName, existingItemsInBox);
    
                return;
            }
        }
    
        Debug.LogWarning($"UI Button with label '{buttonText}' not found to remove.");
    }

    public void EmptyBox()
    {
        if (existingItemsInBox.Count == 0) return;
        
        foreach (string item in existingItemsInBox)
        {
            RemoveBoxItemButton(item);
        }
        
    }

    public void AddItemButtonInBox(string buttonText)
    {
        var newBtn = Instantiate(buttonPrefab_ItemInBox, itemsInTheBoxBtnsParent.transform);
        newBtn.GetComponent<BtnManager_AddItems>().SetLabelText(buttonText);
        
        
        //Add Toggle Event ( click to destroy/remove button)
        // Validate Toggle component
        var toggle = newBtn.GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    RemoveBoxItemButton(buttonText);
                }
                
            });
        }
        else
        {
            Debug.LogError("Toggle component missing on the button prefab.");
        }
    }

    
}
