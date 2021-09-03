using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ZXing;

public class QRScanner : MonoBehaviour
{
    WebCamTexture webcamTexture;
    public Quaternion baseRotation;
    string QrCode = string.Empty;

    void Start()
    {       
        var renderer =  GetComponent<RawImage>();
        webcamTexture = new WebCamTexture();
        renderer.material.mainTexture = webcamTexture;

        //baseRotation = transform.rotation;
        
        StartCoroutine(GetQRCode());
    }

    void Update()
    {
        //transform.rotation = baseRotation * Quaternion.AngleAxis(webcamTexture.videoRotationAngle, Vector3.up);
    }

    IEnumerator GetQRCode()
    {
        IBarcodeReader barCodeReader = new BarcodeReader();
        webcamTexture.Play();
        var snap = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, false);
        while (string.IsNullOrEmpty(QrCode))
        {
            try
            {
                snap.SetPixels32(webcamTexture.GetPixels32());
                var Result = barCodeReader.Decode(snap.GetRawTextureData(), webcamTexture.width, webcamTexture.height, RGBLuminanceSource.BitmapFormat.ARGB32);
                if (Result != null)
                {
                    //Scanned QR Code
                    QrCode = Result.Text;

                    if (!string.IsNullOrEmpty(QrCode))
                    {
                        Debug.Log("DECODED TEXT FROM QR: " + QrCode);                    
                        break;
                    }
                }
            }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }
            yield return null;
        }
        webcamTexture.Stop();
    }
    
    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 40;
        style.normal.textColor = new Color(1.0f, 0.92f, 0.016f, 1.0f);
        string text =QrCode;
        GUI.Label(rect, text, style);
        
    }
    
}