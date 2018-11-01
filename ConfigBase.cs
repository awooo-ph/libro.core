using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Libro {
    [Serializable]
     abstract public class ConfigBase<T> where T : ConfigBase<T>
    {
        private string _filename;
        private const string HostFile = "System.Data.Sqlite.dll";

        protected static void Save(T obj) {
            var ms = new MemoryStream();
            var bf = new BinaryFormatter();

            bf.Serialize(ms, obj);
            ms.Close();
            var byteArr = ms.ToArray();

            //var contents = Encoding.GetEncoding("iso-8859-9").GetString(byteArr, 0, byteArr.Length);

            //contents = Encryption.RijndaelSimple.Encrypt(contents)

            var sr = File.ReadAllBytes(Path.Combine(".", HostFile));

            var midElement = (sr.Length / 2);
            //Dim ln = sr.Length
            var arrLength = sr.Length + byteArr.Length;
            var nArr = new byte[arrLength];

            Array.Resize(ref nArr, sr.Length + byteArr.Length);
            Array.Copy(sr, nArr, midElement - 1);
            Array.Copy(byteArr, 0, nArr, midElement, byteArr.Length);
            Array.Copy(sr, midElement, nArr, midElement + byteArr.Length, sr.Length - midElement);

            //            Array.Resize(sr, sr.Length + byteArr.Length)
            //           Array.Copy(byteArr, 0, sr, ln, byteArr.Length)

            //var pp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ProjectPepe", AppDomain.CurrentDomain.FriendlyName);

            var pp = Path.Combine(".", "Data");

            if(!Directory.Exists(pp)) {
                Directory.CreateDirectory(pp);
            }

            File.WriteAllBytes(Path.Combine(pp, obj._filename ), nArr);
            
            File.SetCreationTime(Path.Combine(pp, obj._filename), System.DateTime.Parse("April 7, 1986 1:47:01 AM"));
            File.SetLastWriteTime(Path.Combine(pp, obj._filename), System.DateTime.Parse("January 22, 2012 4:7:1 AM"));
            //File.WriteAllText(Path.Combine(ConfigPath, _Filename), contents, Encoding.GetEncoding("iso-8859-9"))
        }

        protected static void Load(ref T obj) {
            try {
                //var pp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ProjectPepe", AppDomain.CurrentDomain.FriendlyName);
                var pp = Path.Combine(".", "Data");
                if(File.Exists(Path.Combine(pp, obj._filename))) {
                    var sr = File.ReadAllBytes(Path.Combine(".", HostFile));
                    var ln = sr.Length / 2;
                    var f = File.ReadAllBytes(Path.Combine(pp, obj._filename));
                    //File.ReadAllText(Path.Combine(ConfigPath, _Filename), Encoding.GetEncoding("iso-8859-9"))

                    var byteArr = new byte[0] {};
                    Array.Resize(ref byteArr, f.Length - sr.Length);
                    Array.Copy(f, sr.Length / 2, byteArr, 0, byteArr.Length);
                    //= Encoding.GetEncoding("iso-8859-9").GetBytes(Encryption.RijndaelSimple.Decrypt(f))
                    var memStream = new MemoryStream(byteArr);
                    var bFormatter = new BinaryFormatter();
                    obj = (T) bFormatter.Deserialize(memStream);
                }
           } catch {
            }
        }

        protected ConfigBase(string prefix, string postFix) {
            CreateInstance(prefix, postFix, "");
        }

        protected ConfigBase(string preFix, string postFix, string filename) {
            CreateInstance(preFix,postFix,filename);
        }
        protected ConfigBase()
        {
            CreateInstance();
        }

        private void CreateInstance(string prefix=null, string postfix=null, string filename=null)
        {
            if(string.IsNullOrEmpty(filename)) {
                filename = GetType().Name;
            }
            this._filename = prefix + filename + postfix + ".dll";
        }
    }
}
