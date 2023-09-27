using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class Crawler
{
    const string downloadUrl = "https://web.sanguosha.com/220/h5_2/res/runtime/pc/general/big/static/";
    public static IEnumerator DownloadSkin(IEnumerable<Character> characters)
    {
        foreach (var character in characters)
        {
            string localPath = $"{Directory.GetCurrentDirectory()}/Assets/Resources/Image/skin/{character.name}/";
            if (!File.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            int i = 0;
            while (true)
            {
                string index = i++.ToString().PadLeft(2, '0');
                Debug.Log(index);
                string path = localPath + index + ".png";
                if (File.Exists(path))
                {
                    continue;
                }
                string url = $"{downloadUrl}{character.id}{index}.png";
                UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);
                webRequest.timeout = 5;
                yield return webRequest.SendWebRequest();
                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    break;
                }
                using (FileStream fileStream = File.Create(path))
                {
                    Texture2D texture2D = DownloadHandlerTexture.GetContent(webRequest);
                    byte[] bytes = texture2D.EncodeToPNG();
                    fileStream.Write(bytes);
                };
            }
        }
    }
}