using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace ImageGrabber
{
    public class Program
    {
        static void Main(string[] args)
        {
            var basePath = @"C:\Projects\HSRecord\HSRecord\HSRecord\HSRecord.Web\";
            var cardImageMapPath = basePath + @"data\cardimagemap.js";
            var cardIdImageMapPath = basePath + @"data\cardimagemap.js";
            var allSetsPath = basePath + @"scripts\AllSets.js";
            var images = LoadImagesMap(@"http://www.hearthpwn.com/cards?display=2&page=");
            File.WriteAllText(cardImageMapPath, images.ToJson());

            var map = Parse(allSetsPath, cardImageMapPath);
            File.WriteAllText(cardIdImageMapPath, map.ToJson());
        }

        static Dictionary<string, string> Parse(string allSetspath, string cardImageMapPath)
        {
            var allSetsContent = File.ReadAllText(allSetspath);
            JObject allSetsJson = JObject.Parse(allSetsContent);

            var cardImageMapContent = File.ReadAllText(cardImageMapPath);
            JObject cardImageMapJson = JObject.Parse(cardImageMapContent);

            var cards = new Dictionary<string, string>();
            foreach (KeyValuePair<string, JToken> item in allSetsJson)
            {
                foreach (JToken jCard in item.Value.Children())
                {
                    if (!cards.ContainsKey(jCard["name"].ToString()))
                        cards.Add(jCard["name"].ToString(), jCard["id"].ToString());
                }
            }

            var imageMap = new Dictionary<string, string>();
            foreach (KeyValuePair<string, JToken> item in cardImageMapJson)
            {
                if (!imageMap.ContainsKey(item.Key))
                    imageMap.Add(item.Key, item.Value.ToString());
            }

            var id2ImageMap = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> map in imageMap)
            {
                if (cards.ContainsKey(map.Key))
                    id2ImageMap.Add(cards[map.Key], map.Value);
            }
            return id2ImageMap;
        }

        static Dictionary<string, string> LoadImagesMap(string linkTemplate)
        {
            var grabber = new Grabber();
            var images = new Dictionary<string, string>();
            for (var i = 1; i <= 13; i++)
                images = images.Merge(grabber.GetImages(string.Format("{0}{1}", linkTemplate, i)));

            return images;
        }

        static void DownloadAllImages(Dictionary<string, string> source)
        {
            foreach (KeyValuePair<string, string> item in source)
            {
                var imagePathTemplate =
                    @"C:\Projects\HSRecord\HSRecord\HSRecord\HSRecord.Web\images\cards\{0}.png";
                ImageDownloader.Download(item.Value, string.Format(imagePathTemplate, item.Key));
            }
        }
    }

    public class Grabber
    {
        private string url;
        private string content;
        private string[] lines;
        private int index;

        public Grabber()
        {
            index = 0;
        }
        public Dictionary<string, string> GetImages(string url)
        {
            this.url = url;
            this.LoadPageContent();
            return this.ParsePage();
        }

        private void LoadPageContent()
        {
            var client = new WebClient();
            this.content = client.DownloadString(this.url);
            index = 0;
            this.lines = this.content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }

        private Dictionary<string, string> ParsePage()
        {
            var result = new Dictionary<string, string>();
            while (index < this.lines.Length)
            {
                var entry = GetNext();
                if (!result.ContainsKey(entry.Key))
                    result.Add(entry.Key, entry.Value);
            }
            return result;
        }

        private KeyValuePair<string, string> GetNext()
        {
            var imgSrcRE = new Regex(@"img\sclass=""hscard-static""\ssrc=""(?<imageLink>.*)""\swidth");
            var cardNameRE = new Regex(@"<a href="".*"">(?<cardName>.*)<\/a>");
            string link = string.Empty, cardName = string.Empty;
            while (index < this.lines.Length)
            {
                if (this.lines[index].Contains(@"<img class=""hscard-static"" src="))
                {
                    var match = imgSrcRE.Match(this.lines[index]);
                    link = match.Groups["imageLink"].Value;
                    break;
                }

                index++;
            }
            while (index < this.lines.Length)
            {
                if (this.lines[index].Contains(@"<a href="))
                {
                    var match = cardNameRE.Match(this.lines[index]);
                    cardName = match.Groups["cardName"].Value;
                    break;
                }

                index++;
            }

            return new KeyValuePair<string, string>(cardName, link);
        }
    }
    public static class DictionaryExtensions
    {
        public static Dictionary<string, string> Merge(this  Dictionary<string, string> source,
            Dictionary<string, string> target)
        {
            foreach (KeyValuePair<string, string> item in target)
            {
                if (!source.ContainsKey(item.Key))
                    source.Add(item.Key, item.Value);
            }

            return source;
        }

        public static string ToJson(this Dictionary<string, string> source)
        {
            var entries = source.Select(d =>
                string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));
            return "{" + string.Join(",", entries) + "}";
        }
    }

    public class ImageDownloader
    {
        public static void Download(string url, string path)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, path);
            }
        }
    }
}
