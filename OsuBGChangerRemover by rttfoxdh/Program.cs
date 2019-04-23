using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace OsuBackgroundChanger
{
    class Program
    {
        static void Replace(string folderlocation)
        {
            string bgfile = "";
            Console.WriteLine("Choose a background");
            var filebrowser = new OpenFileDialog();
            using (filebrowser)
            {
                if(filebrowser.ShowDialog() == DialogResult.OK)
                {
                    bgfile = filebrowser.FileName;
                }
            }
            foreach (string folder in Directory.GetDirectories(folderlocation))
            {
                string foundedbg = "";
                foreach (string file in Directory.GetFiles(folder))
                {
                    if (file.IndexOf(".osu")>-1)
                    {
                        int ifnothing=0;
                        using (StreamReader reader = new StreamReader(file)){
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.IndexOf("//Background and Video events") > -1)
                                {
                                    line = reader.ReadLine();
                                    foundedbg = line.Remove(0, 5);
                                    foundedbg = foundedbg.Remove(foundedbg.Length - 5, 5);
                                    if (foundedbg == "")
                                        ifnothing = 1;
                                    break;
                                }
                            }
                        }
                        if (ifnothing == 1)
                        {
                            foundedbg = bgfile.Remove(0, bgfile.LastIndexOf("\\")+1);
                            string _text = File.ReadAllText(file);
                            _text = _text.Replace("0,0,\"\",0,0", "0,0,\""+foundedbg+ "\",0,0");
                            File.WriteAllText(file, _text);
                            if (File.Exists(folder + "\\" + foundedbg) == true)
                                File.Delete(folder + "\\" + foundedbg);
                            File.Copy(bgfile, folder + "\\" + foundedbg);
                        }
                        else
                        {
                            string text = File.ReadAllText(file);
                            text = text.Replace(foundedbg, foundedbg.Remove(foundedbg.IndexOf(".") + 1) + bgfile.Remove(0, bgfile.IndexOf(".") + 1));
                            File.WriteAllText(file, text);
                            if (foundedbg != "")
                            {
                                if (File.Exists(folder + "\\" + foundedbg) == true)
                                    File.Delete(folder + "\\" + foundedbg);
                                foundedbg = foundedbg.Remove(foundedbg.IndexOf(".") + 1) + bgfile.Remove(0, bgfile.IndexOf(".") + 1);
                                File.Copy(bgfile, folder + "\\" + foundedbg);
                            }
                        }
                    }
                }
            }
        }
        static void Delete(string folderlocation)
        {
            foreach (string folder in Directory.GetDirectories(folderlocation))
            {
                string foundedbg = "";
                foreach (string file in Directory.GetFiles(folder))
                {
                    if (file.IndexOf(".osu") > -1)
                    {
                        using (StreamReader reader = new StreamReader(file))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.IndexOf("//Background and Video events") > -1)
                                {
                                    line = reader.ReadLine();
                                    foundedbg = line.Remove(0, 5);
                                    foundedbg = foundedbg.Remove(foundedbg.Length - 5, 5);
                                    break;
                                }
                            }
                        }
                        string text = File.ReadAllText(file);
                        text = text.Remove(text.IndexOf(foundedbg), foundedbg.Length);
                        File.WriteAllText(file, text);
                        if (foundedbg != "")
                        {
                            if (File.Exists(folder + "\\" + foundedbg) == true)
                                File.Delete(folder + "\\" + foundedbg);
                        }
                    }
                }
            }
        }
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Developer : https://github.com/rttfoxdh");
            Console.WriteLine("Choose Songs location folder ( deffault: osu!\\Songs )");
            string folderlocation="";
            var folderbrowser = new FolderBrowserDialog();
            using (folderbrowser)
            {
                if (folderbrowser.ShowDialog() == DialogResult.OK)
                    folderlocation = folderbrowser.SelectedPath;
            }
            if (folderlocation == "")
            {
            }
            else
            {
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1. Replace all backgrounds");
                Console.WriteLine("2. Delete all backgrounds");
                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        Replace(folderlocation);
                        break;
                    case 2:
                        Delete(folderlocation);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
