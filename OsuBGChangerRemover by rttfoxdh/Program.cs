using System;
using System.IO;
using System.Windows.Forms;

namespace OsuBackgroundChanger
{
    internal class Program
    {
        private static void Replace(string folderlocation)
        {
            string bgfile = "";
            Console.WriteLine("Choose a background");
            var filebrowser = new OpenFileDialog();
            using (filebrowser)
            {
                if (filebrowser.ShowDialog() == DialogResult.OK)
                {
                    bgfile = filebrowser.FileName;
                }
            }
            if (bgfile != "")
            {
                foreach (string folder in Directory.GetDirectories(folderlocation))
                {
                    int bakcheck = 0;
                    string foundedbg = "";
                    int onlyonetime = 1;
                    int onlyonetimenothing = 1;
                    string foundedfixed = "";
                    string _foundedbg = "";
                    foreach (string file in Directory.GetFiles(folder))
                    {
                        if (file.IndexOf(".bak") > -1)
                            bakcheck = 1;
                        if (file.IndexOf(".osu") > -1)
                        {
                            int ifnothing = 0;
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
                                        if (foundedbg == "")
                                            ifnothing = 1;
                                        else
                                            ifnothing = 0;
                                        break;
                                    }
                                }
                            }
                            if (ifnothing == 1)
                            {
                                if (foundedfixed != "")
                                    foundedbg = foundedfixed;
                                else
                                    foundedbg = bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1);
                                string _text = File.ReadAllText(file);
                                _text = _text.Replace("0,0,\"\",0,0", "0,0,\"" + foundedbg + "\",0,0");
                                File.WriteAllText(file, _text);
                                if (onlyonetimenothing == 1)
                                {
                                    if (File.Exists(folder + "\\" + foundedbg) == true)
                                        File.Delete(folder + "\\" + foundedbg);
                                    File.Copy(bgfile, folder + "\\" + foundedbg);
                                    onlyonetimenothing = 0;
                                }
                                if (foundedfixed == "")
                                    foundedfixed = foundedbg;
                                Success();
                            }
                            else
                            {
                                _foundedbg = foundedbg;
                                if (foundedfixed != "")
                                    foundedbg = foundedfixed;
                                string text = File.ReadAllText(file);
                                text = text.Replace(_foundedbg, foundedbg.Remove(foundedbg.IndexOf(".") + 1) + bgfile.Remove(0, bgfile.IndexOf(".") + 1));
                                File.WriteAllText(file, text);
                                if (foundedbg != "" & (onlyonetime == 1 || _foundedbg != foundedfixed))
                                {
                                    if (bakcheck == 0 & File.Exists(folder + "\\" + _foundedbg + ".bak") == false & File.Exists(folder + "\\" + _foundedbg) == true)
                                        File.Copy(folder + "\\" + _foundedbg, folder + "\\" + _foundedbg + ".bak");
                                    if (File.Exists(folder + "\\" + _foundedbg) == true)
                                        File.Delete(folder + "\\" + _foundedbg);
                                    if (File.Exists(folder + "\\" + foundedbg) == true)
                                        File.Delete(folder + "\\" + foundedbg);
                                    if (File.Exists(folder + "\\" + foundedbg.Remove(foundedbg.IndexOf(".") + 1) + bgfile.Remove(0, bgfile.IndexOf(".") + 1)) == true)
                                        File.Delete(foundedbg.Remove(foundedbg.IndexOf(".") + 1) + bgfile.Remove(0, bgfile.IndexOf(".") + 1));
                                    if (foundedfixed == "")
                                        foundedfixed = foundedbg;
                                    foundedbg = foundedbg.Remove(foundedbg.IndexOf(".") + 1) + bgfile.Remove(0, bgfile.IndexOf(".") + 1);
                                    File.Copy(bgfile, folder + "\\" + foundedbg);
                                    onlyonetime = 0;
                                    Success();
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void Delete(string folderlocation)
        {
            foreach (string folder in Directory.GetDirectories(folderlocation))
            {
                int bakcheck = 0;
                string foundedbg = "";
                foreach (string file in Directory.GetFiles(folder))
                {
                    if (file.IndexOf(".bak") > -1)
                        bakcheck = 1;
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
                        if (foundedbg != "" & File.Exists(folder + "\\" + foundedbg) == true)
                        {
                            if (bakcheck == 0 & File.Exists(folder + "\\" + foundedbg + ".bak") == false)
                                File.Copy(folder + "\\" + foundedbg, folder + "\\" + foundedbg + ".bak");
                            File.Delete(folder + "\\" + foundedbg);
                            Success();
                        }
                    }
                }
            }
        }

        private static void Restore(string folderlocation)
        {
            foreach (string folder in Directory.GetDirectories(folderlocation))
            {
                string restorefilename = "";
                string bakfilename = "";
                string foundedbg = "";
                foreach (string file in Directory.GetFiles(folder))
                {
                    if (file.IndexOf(".bak") > -1)
                    {
                        bakfilename = file;
                        break;
                    }
                }
                if (bakfilename != "")
                {
                    int onlyonetime = 1;
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
                            if (foundedbg != "")
                            {
                                string text = File.ReadAllText(file);
                                restorefilename = bakfilename.Remove(0, bakfilename.LastIndexOf("\\") + 1);
                                restorefilename = restorefilename.Remove(restorefilename.LastIndexOf("."));
                                text = text.Replace(foundedbg, restorefilename);
                                File.WriteAllText(file, text);
                                if (onlyonetime == 1)
                                {
                                    if (File.Exists(folder + "\\" + restorefilename) == true)
                                        File.Delete(folder + "\\" + restorefilename);
                                    if (File.Exists(folder + "\\" + foundedbg) == true)
                                        File.Delete(folder + "\\" + foundedbg);
                                    File.Copy(bakfilename, folder + "\\" + restorefilename);
                                    File.Delete(bakfilename);
                                    onlyonetime = 0;
                                    Success();
                                }
                            }
                            else
                            {
                                string text = File.ReadAllText(file);
                                restorefilename = bakfilename.Remove(0, bakfilename.LastIndexOf("\\") + 1);
                                restorefilename = restorefilename.Remove(restorefilename.LastIndexOf("."));
                                text = text.Replace("0,0,\"\",0,0", "0,0,\"" + restorefilename + "\",0,0");
                                File.WriteAllText(file, text);
                                if (onlyonetime == 1)
                                {
                                    if (File.Exists(folder + "\\" + restorefilename) == true)
                                        File.Delete(folder + "\\" + restorefilename);
                                    File.Copy(bakfilename, folder + "\\" + restorefilename);
                                    File.Delete(bakfilename);
                                    onlyonetime = 0;
                                    Success();
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void Success()
        {
            Console.Write("[" + DateTime.Now.ToString() + "]");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  Success");
            Console.ResetColor();
        }

        [STAThread]
        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Developer : https://github.com/rttfoxdh");
            Console.ResetColor();
            Console.WriteLine("Choose Songs location folder ( deffault: osu!\\Songs )");
            string folderlocation = "";
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
                Console.WriteLine("Choosed folder : " + folderlocation);
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1. Replace all backgrounds");
                Console.WriteLine("2. Delete all backgrounds");
                Console.WriteLine("3. Restore all backgrounds");
                Console.Write("Enter: ");
                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        Replace(folderlocation);
                        break;

                    case 2:
                        Delete(folderlocation);
                        break;

                    case 3:
                        Restore(folderlocation);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}