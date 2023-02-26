using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UIElements;

public class ScreenCapture : MonoBehaviour
{
    public int captureWidth = 1920;
    public int captureHeight = 1080;

    public enum Format { RAW ,JPG, PNG, PPM};
    public Format format = Format.PNG;

    private string outputFolder;

    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenshot;

    public bool isProcessing;

    private void Start()
    {
        outputFolder = Application.persistentDataPath + "/Screenshots/";
        if(!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
            Debug.Log("Save path will be : " + outputFolder);
        }
    }

    private string CreateFileName(int width, int height)
    {
        string timeStamp = DateTime.Now.ToString("yyyyMMddTHHmmss");

        var fileName = string.Format("{0}/screen_{1}x{2}_{3}.{4}", outputFolder, width, height, timeStamp, format.ToString().ToLower());

        return fileName;
    }

    private void CaptureScreenShot()
    {
        isProcessing = true;

        if(renderTexture == null)
        {
            rect = new Rect(0, 0, captureWidth, captureHeight);
            renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
            screenshot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        }

        Camera camera = Camera.main;
        camera.targetTexture = renderTexture;
        camera.Render();

        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(rect, 0, 0);

        camera.targetTexture = null;
        RenderTexture.active = null;

        string fileName = CreateFileName((int)rect.width, (int)rect.height);

        byte[] fileHeader = null;
        byte[] fileData = null;

        switch (format)
        {
            case Format.RAW:
                fileData = screenshot.GetRawTextureData();
                break;
            case Format.JPG:
                fileData = screenshot.EncodeToJPG();
                break;
            case Format.PNG:
                fileData = screenshot.EncodeToPNG();
                break;
            default:
                string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
                fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
                fileData = screenshot.GetRawTextureData();
                break;
        }

        new System.Threading.Thread(() =>
        {
            var file = System.IO.File.Create(fileName);
            if(fileHeader != null)
            {
                file.Write(fileHeader, 0, fileHeader.Length);
            }
            file.Write(fileData, 0, fileData.Length);
            file.Close();
            Debug.Log(string.Format("Screenshot Saved {0}, size {1}", fileName, fileData.Length));
            isProcessing = false;
        }
        ).Start();

        Destroy(renderTexture);
        renderTexture = null;
        screenshot = null;
    }

    public void TakeScreenShot()
    {
        if(!isProcessing)
        {
            //UIController.Instance.
            CaptureScreenShot();
        }
        else
        {
            Debug.Log("Currently Processing...");
        }
    }
}
