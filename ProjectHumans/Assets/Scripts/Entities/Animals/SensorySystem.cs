using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class SensorySystem {

    public Animal thisAnimal;

    public Camera visualInputCamera;
    Matrix<float> visualInputArray;

    int visualResolution;

    public SensorySystem(Animal animal) {
        this.thisAnimal = animal;
        visualInputCamera = thisAnimal.GetGameObject().GetComponentInChildren<Camera>();
        visualResolution = (int)this.thisAnimal.GetPhenotype().GetTraitDict()["visual_resolution"];
    
        InitVisualInput();
    }

    public void InitVisualInput() {
        visualInputArray = Matrix<float>.Build.Dense(3, visualResolution * visualResolution);

        if (visualInputCamera != null) {
            if (visualInputCamera.targetTexture == null) {
                visualInputCamera.targetTexture = new RenderTexture(visualResolution, visualResolution, 24);
                /* 24 is the depth buffer, or depth texture, is actually just a render texture that contains values of how far objects in the scene are from the camera.*/
            }
            else {
                visualResolution = visualInputCamera.targetTexture.width;
                visualResolution = visualInputCamera.targetTexture.height;
            }
        }

        UpdateVisualInput();
    }

    public Matrix<float> GetVisualInput() {
        if (this.thisAnimal.GetAge() % this.thisAnimal.GetPhenotype().GetTraitDict()["visual_refresh_rate"] == 0)
        {
            UpdateVisualInput();
            //string outputString = visualInputArray.GetLength(0).ToString() + "," + visualInputArray.GetLength(1).ToString();
            //Debug.Log(visualInputArray[1, 1]);
        }
        return visualInputArray;
    }

    public void UpdateVisualInput() {
        if (this.thisAnimal.visualInputCamera.gameObject.activeInHierarchy) {
            
            Texture2D visualInputTexture = new Texture2D(visualResolution, visualResolution, TextureFormat.RGB24, false);

            this.thisAnimal.visualInputCamera.Render();
            RenderTexture.active = this.thisAnimal.visualInputCamera.targetTexture;
            visualInputTexture.ReadPixels(new Rect(0, 0, visualResolution, visualResolution), 0, 0);

            Color[] colorArray = visualInputTexture.GetPixels();
            
            int resolutionSquared = visualResolution*visualResolution;
            for (int i=0; i<resolutionSquared; i++){
                visualInputArray[0,i] = colorArray[i].r;
                visualInputArray[1,i] = colorArray[i].g;
                visualInputArray[2,i] = colorArray[i].b;
            }
            //SaveVisualImage(visualInputTexture);
    
        } else {
            Debug.Log("Camera is off");
        }
    }
    
    void SaveVisualImage(Texture2D visualInputTexture){
        byte[] bytes = visualInputTexture.EncodeToPNG();
        string fileName = GetImageName();
        System.IO.File.WriteAllBytes(fileName, bytes);
    }

    string GetImageName() {
        return string.Format("{0}/VisualInputs/visualInput_{1}x{2}_{3}.png",
        Application.dataPath,
        visualResolution,
        visualResolution,
        System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

   // Texture2D ResizeTexture(Texture2D pSource, float pScale){
    //     //*** Variables
    //     int i;
    
    //     //*** Get All the source pixels
    //     Color[] aSourceColor = pSource.GetPixels(0);
    //     Vector2 vSourceSize = new Vector2(pSource.width, pSource.height);
    
    //     //*** Calculate New Size
    //     float xWidth = Mathf.RoundToInt((float)pSource.width * pScale);                     
    //     float xHeight = Mathf.RoundToInt((float)pSource.height * pScale);
    
    //     //*** Make New
    //     Texture2D oNewTex = new Texture2D((int)xWidth, (int)xHeight, TextureFormat.RGB24, false);
    
    //     //*** Make destination array
    //     int xLength = (int)xWidth * (int)xHeight;
    //     Color[] aColor = new Color[xLength];
    
    //     Vector2 vPixelSize = new Vector2(vSourceSize.x / xWidth, vSourceSize.y / xHeight);
    
    //     //*** Loop through destination pixels and process
    //     Vector2 vCenter = new Vector2();
    //     for(i=0; i<xLength; i++){
    
    //         //*** Figure out x&y
    //         float xX = (float)i % xWidth;
    //         float xY = Mathf.Floor((float)i / xWidth);
    
    //         //*** Calculate Center
    //         vCenter.x = (xX / xWidth) * vSourceSize.x;
    //         vCenter.y = (xY / xHeight) * vSourceSize.y;


    //         int xXFrom = (int)Mathf.Max(Mathf.Floor(vCenter.x - (vPixelSize.x * 0.5f)), 0);
    //         int xXTo = (int)Mathf.Min(Mathf.Ceil(vCenter.x + (vPixelSize.x * 0.5f)), vSourceSize.x);
    //         int xYFrom = (int)Mathf.Max(Mathf.Floor(vCenter.y - (vPixelSize.y * 0.5f)), 0);
    //         int xYTo = (int)Mathf.Min(Mathf.Ceil(vCenter.y + (vPixelSize.y * 0.5f)), vSourceSize.y);
 
    //         //*** Loop and accumulate
    //         Color oColorTemp = new Color();
    //         float xGridCount = 0;
    //         for(int iy = xYFrom; iy < xYTo; iy++){
    //             for(int ix = xXFrom; ix < xXTo; ix++){
 
    //                 //*** Get Color
    //                 oColorTemp += aSourceColor[(int)(((float)iy * vSourceSize.x) + ix)];
 
    //                 //*** Sum
    //                 xGridCount++;
    //             }
    //         }
 
    //         //*** Average Color
    //         aColor[i] = oColorTemp / (float)xGridCount;
    //     }
    //     oNewTex.SetPixels(aColor);
    //     oNewTex.Apply();
    //     Debug.Log("visualInput taken");
    //     return oNewTex;
    // }

}