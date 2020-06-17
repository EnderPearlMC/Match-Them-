using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3.Datas
{
    class DataManager
    {

        public static void WriteFile(string file, object obj)
        {
            string json = JsonConvert.SerializeObject(obj);

            bool exists = System.IO.Directory.Exists(Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData), References.DATAS_FOLDER));
            if (!exists)
                System.IO.Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData), References.DATAS_FOLDER));

            System.IO.File.WriteAllText(Path.Combine(Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData), References.DATAS_FOLDER), file), json);

        }

        public static T ReadFile<T>(string file)
        {

            string text = System.IO.File.ReadAllText(Path.Combine(Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData), References.DATAS_FOLDER), file));

            return JsonConvert.DeserializeObject<T>(text);

        }

    }
}
