using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Libro.Model;
using Xceed.Words.NET;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;

namespace Libro
{
    public class Card
    {
        public static async Task GenerateCardAsync(Book book)
        {
            await Task.Factory.StartNew(() =>
            {
                GenerateCard(book);
            });
        }

        public static void GenerateCardsAsync(IList<Book> books)
        {
            Parallel.ForEach(books,async b=>await GenerateCardAsync(b));
        }

        public static async void MergeDocsAsync(string path, string dest)
        {
            await Task.Factory.StartNew(() =>
            {
                MergeDocs(path,dest);
            });
        }

        public static void GenerateCard(Book book)
        {
            try
            {
                using(var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Libro.card.docx"))
                using(var doc = DocX.Load(stream))
                {
                    doc.ReplaceText("[LOC]", book.Location??Environment.NewLine);
                    doc.ReplaceText("[CODE]", book.ClassificationNumber?? Environment.NewLine);
                    doc.ReplaceText("[AUTH]", book.AuthorNumber?? Environment.NewLine);
                    DateTime year;
                    if(DateTime.TryParse(book.Published, out year))
                        doc.ReplaceText("[YEAR]", year.Year.ToString());
                    else
                        doc.ReplaceText("[YEAR]", "");

                    doc.ReplaceText("[ACC_NUM]", book.AccessionNumber??"");
                    doc.ReplaceText("[TITLE]", book.Title??"");
                    doc.ReplaceText("[AUTHOR]", string.IsNullOrWhiteSpace(book.Author) ? "(Unknown Author)" : book.Author??"");
                    doc.ReplaceText("[SUBTITLE]", book.SubTitle??"");
                    doc.ReplaceText("[T:]", string.IsNullOrWhiteSpace(book.Subject) ? "" : ":");
                    doc.ReplaceText("[COAUTHOR]", book.Coauthors??"");
                    doc.ReplaceText("[PLACE]", string.IsNullOrEmpty(book.PublicationPlace) ? "" : book.PublicationPlace??"");
                    doc.ReplaceText("[PUBLISHER]", string.IsNullOrEmpty(book.Publisher) ? "(Unknown Publisher)" : book.Publisher??"");
                    doc.ReplaceText("[INITIAL_PAGES]", book.InitialPages??"");
                    doc.ReplaceText("[P,]", string.IsNullOrWhiteSpace(book.InitialPages) ? "" : ", ");
                    doc.ReplaceText("[PAGES]", book.Pages.ToString());
                    doc.ReplaceText("[P:]", string.IsNullOrWhiteSpace(book.Illustrations) ? "" : " : ");
                    doc.ReplaceText("[ILLUSTRATIONS]", book.Illustrations??"");
                    doc.ReplaceText("[HEIGHT]", book.Height??"");
                    doc.ReplaceText("[ISBN]", book.Isbn??"");
                    doc.ReplaceText("[SUBJECT]", book.Subject??"(No Subject)");
                    doc.ReplaceText("[NOTES]", book.Remarks??"");
                    
                    doc.AddPasswordProtection(EditRestrictions.readOnly, "4Project7Pepe7");
                    var path = Path.Combine(".", "Cards");
                    if(!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    doc.SaveAs(Path.Combine(path, $"{book.Id}.docx"));
                }

            } catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
           
        }

        public static void MergeDocs(string path, string dest)
        {
            try
            {

            var files = Directory.GetFiles(path);
            
            if(File.Exists(dest)) File.Delete(dest);
            File.Copy(files[0],dest);

            using(WordprocessingDocument doc = WordprocessingDocument.Open(dest,true))
            {
                MainDocumentPart mainPart = doc.MainDocumentPart;
                //var body = new Document(new Body());
                //body.Body.AppendChild(new Paragraph());
                //mainPart.Document = body;
                
                for (var x=1; x<files.Length;x++)
                {
                    string altChunkId = "PEPE" + x;
                    AlternativeFormatImportPart altPart =
                        mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.WordprocessingML,
                            altChunkId);

                    using (var fs = File.Open(files[x], FileMode.Open))
                    {
                        altPart.FeedData(fs);
                    }

                    var altChunk = new AltChunk();
                    altChunk.Id = altChunkId;

                    mainPart.Document.Body.InsertAfter(altChunk, mainPart.Document.Body.Elements<Paragraph>().Last());
                }

                mainPart.Document.Save();
            }

            } catch(Exception ex)
            {
                try
                {
                    if(File.Exists("Error.log"))
                        File.AppendAllText("Error.log", ex.Message + Environment.NewLine);
                    else
                        File.WriteAllText("Error.log", ex.Message);
                }
                catch (Exception)
                {
                    //throw;
                }
                
               // throw;
            }
            //File.WriteAllBytes(Path.Combine(path,"cards.docx"),memoryStreamDest.ToArray());
        }
    }
}
