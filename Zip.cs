using System;
using System.IO;
using System.IO.Compression;
using UnityEngine;

class ZipExtractor
{
    public string filePath;
    public string targetPath;

    public ZipExtractor(string filePath, string targetPath) {
        this.targetPath = targetPath;

        this.filePath = filePath;
    }
    public int Extract()
    {
        try
        {
            ZipFile.ExtractToDirectory(filePath, targetPath);
            File.Delete(filePath);
        }
        catch (Exception e)
        {
            Debug.Log($"{e}");
            return 1;
        }
        return 0;
        
    }
}
