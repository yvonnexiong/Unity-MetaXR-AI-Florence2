using System;
using TMPro;
using UnityEngine;

public class AddDetectedItemButtonsToUI : MonoBehaviour
{
    //[SerializeField] private TMP_Text itemsDetectedText;
    [SerializeField] private BoxItemsManager ui_boxItemsManager;
    private string[] m_labels;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
                
    }
    
    public void SetLabels(TextAsset labelsAsset)
    {
        //Parse neural net m_labels
        m_labels = labelsAsset.text.Split('\n');
            
            
    }
    
    // public void AddItemButtonWithString(String buttonText)
    // {
    //     itemsDetectedText.text += buttonText + "\n";
    //     
    //     //TODO: detect if duplicate.
    // }


/*****
    public void AddItemButton(Tensor<float> output, Tensor<int> labelIDs)
    {
        var boxesFound = output.shape[0];
        var maxBoxes = Mathf.Min(boxesFound, 200);
        //Draw the bounding boxes
        for (var n = 0; n < maxBoxes; n++)
        {
            // Get object class name
            var classname = m_labels[labelIDs[n]].Replace(" ", "_");

            // Get the 3D marker world position using Depth Raycast
            //var centerPixel = new Vector2Int(Mathf.RoundToInt(perX * camRes.x), Mathf.RoundToInt((1.0f - perY) * camRes.y));
            //var ray = PassthroughCameraUtils.ScreenPointToRayInWorld(CameraEye, centerPixel);
            //var worldPos = m_environmentRaycast.PlaceGameObjectByScreenPos(ray);






            ui_boxItemsManager.AddDetectedItemButton(classname);
            
            // }
        }
    }

    // Input: a classname of the button
    // Method: Create a button 
*****/



    public void AddItemButton(string label)
    {
        ui_boxItemsManager.AddDetectedItemButton(label);
            
        
    }

    // Input: a classname of the button
    // Method: Create a button 
}
