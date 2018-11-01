using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Libro.Data;
using Libro.Google;
using Libro.Models;
using Newtonsoft.Json;

namespace Libro.Google
{
    public partial class GoogleBooks
    {
        public Book GetByIsbn(string isbn, bool tryOnline, bool useIssn = false)
        {
            // return Task.Factory.StartNew(()=> {
            Book bk = null;
            var b = Book.Cache.FirstOrDefault(x => x.Isbn == isbn || x.Isbn13 == isbn || x.Issn == isbn || x.OtherId == isbn);
            if(b != null)
            {
                return new Book(b);
            }

            {
                var cols = new string[] { "Isbn", "Isbn13", "Issn" };
                for(int i = 0; i < 3; i++)
                {
                    b = Db.GetBy<Book>("Isbn", isbn);
                    if(b != null)
                        return new Book(b);
                }
            }

            {
                if(!tryOnline)
                    return null;
                try
                {
                    var volume = GetVolume(isbn.Replace("-", "").Replace(" ", ""), useIssn ? "issn" : "isbn");
                    // if (token.IsCancellationRequested) return null;
                    if(volume == null)
                        return null;
                    bk = new Book()
                    {
                        Title = volume.Title ?? "",
                        SubTitle = volume.Subtitle ?? "",
                        Author = volume.Authors?.Count > 0 ? volume.Authors[0] : "(Unknown Author)",
                        Coauthors = volume.Authors?.Count > 1 ? string.Join(", ", volume.Authors.Skip(1)) : "",
                        Publisher = volume.Publisher ?? "",
                        Synopsis = volume.Description ?? "",
                        Isbn = isbn,
                        Pages = volume.PageCount ?? 0,
                        Height =
                            string.IsNullOrEmpty(volume.Dimensions?.Height) ? "" : volume.Dimensions.Height + " cm.",
                        Width = volume.Dimensions?.Width ?? "",
                        Thickness = volume.Dimensions?.Thickness ?? "",
                        Subject = volume.Categories?.Count > 0 ? volume.Categories[0] : "",
                        Type = volume.PrintType ?? "BOOK",
                        Published = volume.PublishedDate ?? "",
                        Thumbnail = volume.ImageLinks?.Thumbnail
                    };
                    if(volume.IndustryIdentifiers?.Count > 1)
                    {
                        foreach(var id in volume.IndustryIdentifiers)
                        {
                            if(id.Type == "ISBN_10")
                                bk.Isbn = id.Identifier;
                            else if(id.Type == "ISBN_13")
                                bk.Isbn13 = id.Identifier;
                            else if(id.Type == "ISSN")
                                bk.Issn = id.Identifier;
                            else if(id.Type == "OTHER")
                                bk.OtherId = id.Identifier;
                        }
                    }
                    DownloadThumbnail(bk, volume.ImageLinks?.Thumbnail);
                } catch(Exception ex)
                {
                    //
                }
            }

            return bk;

            //  }, token);
        }

        private void DownloadThumbnail(Book book, string url)
        {
            if(string.IsNullOrEmpty(url))
                return;
            if(!string.IsNullOrEmpty(book.Thumbnail) && !book.Thumbnail.StartsWith("http") && File.Exists(book.Thumbnail))
                return;
            if(book.Id == 0 && string.IsNullOrEmpty(book.Isbn))
                return;
            var thumbPath = Path.Combine(".", "Thumbnails");
            if(!Directory.Exists(thumbPath))
            {
                Directory.CreateDirectory(thumbPath);
                return;
            }
            var path = "";
            if(!string.IsNullOrEmpty(book.Isbn))
                path = Path.Combine(thumbPath, book.Isbn + ".pp");
            else if(book.Id > 0)
                path = Path.Combine(thumbPath, book.Id + ".p");
            else
                return;
            if(File.Exists(path))
            {
                book.Thumbnail = path;
                return;
            }
            Task.Factory.StartNew(async () =>
            {
                if(await Downloader.Download(url, path))
                    book.Thumbnail = path;
            });
        }

        private HttpWebRequest _previousRequest;

        public void CancelRequest()
        {
            _previousRequest?.Abort();
        }

        public Volume.VolumeInfoData GetVolume(string isbn, string id)
        {
            _previousRequest?.Abort();
            // return await Task.Factory.StartNew(() =>
            //  {
            try
            {
                var file = Path.Combine(".", "Books");
                if (!Directory.Exists(file))
                    Directory.CreateDirectory(file);
                file = Path.Combine(file, $"{isbn}.pp");

                if (File.Exists(file))
                {
                    try
                    {
                        var content = File.ReadAllText(file);
                        var volumes = JsonConvert.DeserializeObject<Volumes>(content);
                        var v = volumes.Items?.FirstOrDefault()?.VolumeInfo;
                        if(v != null)
                            return v;
                    }
                    catch (Exception)
                    {
                        //throw;
                    }
                    
                }

                var fields = "items(id,volumeInfo/*)";

                var http = (HttpWebRequest)
                    WebRequest.Create(
                        new Uri(
                            $"https://www.googleapis.com/books/v1/volumes?key={Key}&q={id}:{isbn}&fields={fields}"));
                _previousRequest = http;

                http.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
                http.AutomaticDecompression = DecompressionMethods.GZip;
                http.UserAgent = "Libro (gzip)";
                //var rStream =    http.GetRequestStreamAsync();

                using (var response = http.GetResponse())
                {

                    var rs = response.GetResponseStream();
                    if (rs == null)
                        return null;
                    var sr = new StreamReader(rs);

                    //   _queryTask = sr.ReadToEndAsync();
                    //  _queryTask
                    var content = sr.ReadToEnd();
                    
                    var volumes = JsonConvert.DeserializeObject<Volumes>(content);
                    var v = volumes.Items?.FirstOrDefault()?.VolumeInfo;
                    if (v != null)
                    {
                        try
                        {
                            File.WriteAllText(file, content);
                        } catch(Exception)
                        {
                            //
                        }
                    }
                    return v;
                }
            }
            catch (Exception)
            {
                return null;
            }
            // }, token);
        }

        //public static async Task<string> GetTitle()
        //{
        //    return await Task.Factory.StartNew(() =>
        //    {
        //        var fields = "items(id,volumeInfo/*)";
        //        var http = WebRequest.CreateHttp(new Uri("https://www.googleapis.com/books/v1/volumes?key=&q=isbn:9712340686"));
        //        http.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
        //        http.AutomaticDecompression = DecompressionMethods.GZip;
        //        http.UserAgent = "Libro (gzip)";
        //        var response = http.GetResponse();

        //        var rs = response.GetResponseStream();
        //        if(rs == null)
        //            return string.Empty;
        //        var sr = new StreamReader(rs);
        //        var content = sr.ReadToEnd();
        //        var volumes = JsonConvert.DeserializeObject<Volumes>(content);
        //        return volumes.Items.FirstOrDefault()?.VolumeInfo.Categories[0];
        //    });
        //}

        //public static async Task<string> GetBook()
        //{
        //    var n = DateTime.Now;
        //    var service = new BooksService(new BaseClientService.Initializer()
        //    { 
        //        ApiKey = "",
        //        ApplicationName = "Libro"
        //    });
            
        //    var search = service.Volumes.List("Philippine History");
            
        //    search.MaxResults = 1;
        //    var title = "";
            

        //    await search.ExecuteAsync().ContinueWith(r =>
        //    {
        //        foreach (var volume in r.Result.Items)
        //        {
        //            title = volume.VolumeInfo.Title;
        //        }
        //    });
        //    Debug.Print((DateTime.Now - n).Milliseconds.ToString());
        //    return title;
        //}
    }
}
