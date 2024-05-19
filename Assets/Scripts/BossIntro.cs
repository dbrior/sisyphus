using UnityEngine;
using System.Collections;
using System.IO;

public class BossIntro : Singleton<BossIntro>
{
    private string[] introLines;

    void Start()
    {
        LoadIntroLines();
    }

    void LoadIntroLines()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "devilIntroLines.txt");
        if (File.Exists(filePath))
        {
            introLines = File.ReadAllLines(filePath);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }

    public string GetRandomIntroLine()
    {
        if (introLines != null && introLines.Length > 0)
        {
            int randomIndex = Random.Range(0, introLines.Length);
            return introLines[randomIndex];
        }
        else
        {
            return "No intro lines available.";
        }
    }
}
