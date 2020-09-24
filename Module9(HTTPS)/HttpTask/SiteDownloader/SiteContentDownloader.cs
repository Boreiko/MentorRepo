using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;


namespace SiteDownloader
{
    public class SiteContentDownloader
    {
        string folderPath;
        int maxDeepLevel;
        string domain;
        bool onlyBaseDomain;
        HashSet<Uri> alreadyDownloadedSites = new HashSet<Uri>();
        public List<string> FileExstention { get; set; }

        public SiteContentDownloader(string folderPath, int deepLevelLimit, bool onlyBaseDomain)
        {
            this.folderPath = folderPath;
            CreateDirectory(this.folderPath);
            maxDeepLevel = deepLevelLimit;          
            this.onlyBaseDomain = onlyBaseDomain;
            FileExstention = new List<string>();
        }

        void CreateDirectory(string path)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }
      
        public void LoadSite(string URL)
        {
            alreadyDownloadedSites.Clear();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(URL);
                domain = httpClient.BaseAddress.Host;
                GetPage(httpClient, httpClient.BaseAddress, 0);
            }
        }     
        void GetPage(HttpClient httpClient, Uri uri, int level)
        {
            if (level > maxDeepLevel || alreadyDownloadedSites.Contains(uri) || domain != uri.Host && onlyBaseDomain)          
                return;
            
            alreadyDownloadedSites.Add(uri);
            
            var header = httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri)).Result;
       
            if (!header.IsSuccessStatusCode)            
                return;
            
            if (header.Content.Headers.ContentType != null && header.Content.Headers.ContentType.MediaType == "text/html")           
                SaveHtmlPage(httpClient, uri, level + 1);
            
            else
            {
                var extension = Path.GetExtension(uri.AbsoluteUri);
                if (FileExstention.Count == 0 || FileExstention.Contains(extension))
                    SaveContentFile(httpClient, uri);
                else                 
                Console.WriteLine("File: "+ uri.AbsoluteUri +" extension is not suitable");
            }
        }  
         void SaveContentFile(HttpClient httpClient, Uri uri)
        {
            var response = httpClient.GetAsync(uri).Result;
          
            Console.WriteLine("Save file: " + uri.AbsoluteUri +"");
            SaveFile(uri, response.Content.ReadAsStreamAsync().Result);
        }

         void SaveHtmlPage(HttpClient httpClient, Uri uri, int level)
        {
            var response = httpClient.GetAsync(uri).Result;
            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8);    
            
            Console.WriteLine("Save page: " + uri.AbsoluteUri + "");
            string fileName = htmlDocument.DocumentNode.Descendants("title").FirstOrDefault()?.InnerText + ".html";
            SaveHtmlDocument(uri, fileName, GetMemoryStreamHTMLPage(htmlDocument));

            var attributesWithLinks = htmlDocument.DocumentNode.Descendants().SelectMany(d => d.Attributes.Where(IsAttributeHasLink));
            foreach (var attributesWithLink in attributesWithLinks)
                GetPage(httpClient, new Uri(httpClient.BaseAddress, attributesWithLink.Value), level + 1);        
        }    
         Stream GetMemoryStreamHTMLPage(HtmlDocument document)
        {
            MemoryStream memoryStream = new MemoryStream();
            document.Save(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
         bool IsAttributeHasLink(HtmlAttribute attribute)
        {
            return attribute.Name == "src" || attribute.Name == "href";
        }

        public void SaveFile(Uri uri, Stream fileStream)
        {
            string filePath = Path.Combine(folderPath, uri.Host) + uri.LocalPath.Replace("/", @"\");
            var directoryPath = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directoryPath);

            if (Directory.Exists(filePath))
                filePath = Path.Combine(filePath, DateTime.Now.ToString());

            SaveToFileFullPath(fileStream, filePath);
            fileStream.Close();
        }
        void SaveToFileFullPath(Stream stream, string fileFullPath)
        {
            var createdFileStream = File.Create(fileFullPath);
            stream.CopyTo(createdFileStream);
            createdFileStream.Close();
        }
        void SaveHtmlDocument(Uri uri, string name, Stream documentStream)
        {
            string saveFolderPath = Path.Combine(folderPath, uri.Host) + uri.LocalPath.Replace("/", @"\"); ;
            Directory.CreateDirectory(saveFolderPath);

            var invalidSymbols = Path.GetInvalidFileNameChars();
            name =  new string(name.Where(c => !invalidSymbols.Contains(c)).ToArray());
            string filePath = Path.Combine(saveFolderPath, name);
            
            SaveToFileFullPath(documentStream, filePath);
            documentStream.Close();
        }            
    }
}
