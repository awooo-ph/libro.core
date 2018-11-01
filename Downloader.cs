using System;
using System.Net;
using System.Threading.Tasks;

namespace Libro
{
    public static class Downloader
    {
        public static async Task<bool> Download(string url,string path)
        {
            try
            {
                using (var client = new WebClient())
                {
                    await client.DownloadFileTaskAsync(new Uri(url), path);
                }
            }
            catch (Exception ex)
            {
                return false;
            }            
            return true;
        }
    }
}
