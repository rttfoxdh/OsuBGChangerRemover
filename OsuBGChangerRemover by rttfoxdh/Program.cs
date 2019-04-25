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
                    string foundedbg = "";
                    int onlyonetime = 1;
                    int bakcheck = 0;
                    foreach (string __file in Directory.GetFiles(folder))
                    {
                        if (__file.IndexOf(".bak") > -1)
                            bakcheck = 1;
                    }
                    foreach (string file in Directory.GetFiles(folder))
                    {
                        string version = "";
                        if (file.IndexOf(".osu") > -1)
                        {
                            using (StreamReader reader = new StreamReader(file))
                            {
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    if (line.IndexOf("Version:") > -1)
                                    {
                                        version = line.Remove(0, 8);
                                    }
                                    if (line.IndexOf("0,0,\"") > -1)
                                    {
                                        foundedbg = line.Remove(0, 5);
                                        foundedbg = foundedbg.Remove(foundedbg.Length - 5, 5);
                                        break;
                                    }
                                }
                            }
                            if (bakcheck == 1)
                            {
                                if (onlyonetime == 1)
                                {
                                    if (File.Exists(folder + "\\" + foundedbg) == true)
                                        File.Delete(folder + "\\" + foundedbg);
                                    if (File.Exists(folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1)) == true)
                                    {
                                        File.Delete(folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1));
                                        File.Copy(bgfile, folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1));
                                    }
                                    else
                                        File.Copy(bgfile, folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1));
                                    onlyonetime = 0;
                                }
                            }
                            if (foundedbg == "")
                            {
                                foundedbg = bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1);
                                string _text = File.ReadAllText(file);
                                _text = _text.Replace("0,0,\"\",0,0", "0,0,\"" + foundedbg + "\",0,0");
                                File.WriteAllText(file, _text);
                                if (onlyonetime == 1)
                                {
                                    if (File.Exists(folder + "\\" + foundedbg) == true)
                                        File.Delete(folder + "\\" + foundedbg);
                                    File.Copy(bgfile, folder + "\\" + foundedbg);
                                    onlyonetime = 0;
                                }
                            }
                            else
                            {
                                string text = File.ReadAllText(file);
                                text = text.Replace(foundedbg, bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1));
                                File.WriteAllText(file, text);
                                if (File.Exists(folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1)) == false & onlyonetime == 1)
                                {
                                    File.Copy(bgfile, folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1));
                                    onlyonetime = 0;
                                }
                                if (File.Exists(folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1)) == true & onlyonetime == 1 & bakcheck == 0)
                                {
                                    int check = 0;
                                    string filename = "";
                                    foreach (string _file in Directory.GetFiles(folder))
                                    {
                                        if (_file.IndexOf(".osu") > -1)
                                        {
                                            using (StreamReader reader = new StreamReader(_file))
                                            {
                                                string line;
                                                while ((line = reader.ReadLine()) != null)
                                                {
                                                    if (line.IndexOf("0,0,\"") > -1)
                                                    {
                                                        if (bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1) == line.Remove(0, 5).Remove(line.Remove(0, 5).Length - 5, 5))
                                                        {
                                                            check = 1;
                                                            filename = _file;
                                                            break;
                                                        }
                                                    }
                                                    if (filename != "")
                                                        break;
                                                }
                                                if (filename != "")
                                                    break;
                                            }
                                        }
                                        if (filename != "")
                                            break;
                                    }
                                    if (check == 0)
                                    {
                                        File.Delete(folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1));
                                        File.Copy(bgfile, folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1));
                                        onlyonetime = 0;
                                    }
                                    else
                                    {
                                        File.Copy(folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1), folder + "\\" + version + "].~!" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1) + ".bak");
                                        File.Delete(folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1));
                                        File.Copy(bgfile, folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1));
                                        onlyonetime = 0;
                                    }
                                }
                                if (File.Exists(folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1)) == true & onlyonetime == 1 & bakcheck == 1)
                                {
                                    File.Delete(folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1));
                                    File.Copy(bgfile, folder + "\\" + bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1));
                                    onlyonetime = 0;
                                }
                                if (File.Exists(folder + "\\" + foundedbg) == true & bakcheck == 0 & foundedbg != bgfile.Remove(0, bgfile.LastIndexOf("\\") + 1))
                                {
                                    File.Copy(folder + "\\" + foundedbg, folder + "\\[" + version + "].~!" + foundedbg + ".bak");
                                    File.Delete(folder + "\\" + foundedbg);
                                }
                                else
                                {
                                    int itsalreadybak = 0;
                                    string filename = "";
                                    foreach (string _file in Directory.GetFiles(folder))
                                    {
                                        if (_file.IndexOf(foundedbg) > -1 & _file.Remove(0, _file.LastIndexOf("\\") + 1) != foundedbg)
                                        {
                                            filename = _file;
                                            if (_file.IndexOf(version) > -1)
                                            {
                                                itsalreadybak = 1;
                                                break;
                                            }
                                            break;
                                        }
                                        if (filename != "")
                                            break;
                                    }
                                    if (filename != "" & itsalreadybak == 0)
                                    {
                                        File.Copy(filename, folder + "\\[" + version + "]&" + filename.Remove(0, filename.LastIndexOf("\\") + 1));
                                        File.Delete(filename);
                                    }
                                }
                            }
                        }
                    }
                    Success();
                }
            }
        }

        private static void Delete(string folderlocation)
        {
            foreach (string folder in Directory.GetDirectories(folderlocation))
            {
                string foundedbg = "";
                int bakcheck = 0;
                foreach (string __file in Directory.GetFiles(folder))
                {
                    if (__file.IndexOf(".bak") > -1)
                        bakcheck = 1;
                }
                foreach (string file in Directory.GetFiles(folder))
                {
                    string version = "";
                    if (file.IndexOf(".osu") > -1)
                    {
                        using (StreamReader reader = new StreamReader(file))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.IndexOf("Version:") > -1)
                                {
                                    version = line.Remove(0, 8);
                                }
                                if (line.IndexOf("0,0,\"") > -1)
                                {
                                    foundedbg = line.Remove(0, 5);
                                    foundedbg = foundedbg.Remove(foundedbg.Length - 5, 5);
                                    break;
                                }
                            }
                        }
                        string text = File.ReadAllText(file);
                        text = text.Remove(text.IndexOf(foundedbg), foundedbg.Length);
                        File.WriteAllText(file, text);
                        if (foundedbg != "" & bakcheck == 0)
                        {
                            if (File.Exists(folder + "\\" + foundedbg) == true)
                            {
                                File.Copy(folder + "\\" + foundedbg, folder + "\\[" + version + "].~!" + foundedbg + ".bak");
                                File.Delete(folder + "\\" + foundedbg);
                            }
                            else
                            {
                                string filename = "";
                                foreach (string _file in Directory.GetFiles(folder))
                                {
                                    if (_file.IndexOf(foundedbg) > -1 & _file.Remove(0, _file.LastIndexOf("\\") + 1) != foundedbg)
                                    {
                                        filename = _file;
                                        break;
                                    }
                                }
                                if (filename != "")
                                {
                                    File.Copy(filename, folder + "\\[" + version + "]&" + filename.Remove(0, filename.LastIndexOf("\\") + 1));
                                    File.Delete(filename);
                                }
                            }
                        }
                        if (bakcheck == 1 & foundedbg != "")
                        {
                            if (File.Exists(folder + "\\" + foundedbg) == true)
                            {
                                File.Delete(folder + "\\" + foundedbg);
                            }
                        }
                    }
                }
                Success();
            }
        }

        private static void Restore(string folderlocation)
        {
            foreach (string folder in Directory.GetDirectories(folderlocation))
            {
                int bakcheck = 0;
                foreach (string file in Directory.GetFiles(folder))
                {
                    if (file.IndexOf(".bak") > -1)
                        bakcheck = 1;
                }
                if (bakcheck == 1)
                {
                    string restoredfilename = "";
                    int numberofbakfiles = 0;
                    foreach (string file in Directory.GetFiles(folder))
                    {
                        string bakfilename = "";
                        if (file.IndexOf(".bak") > -1)
                            bakfilename = file;
                        if (bakfilename != "")
                        {
                            bakfilename = bakfilename.Remove(bakfilename.IndexOf(".~!"));
                            bakfilename = bakfilename.Remove(0, bakfilename.LastIndexOf("\\") + 1);
                            string[] bakfilenamesplited = bakfilename.Split('&');
                            numberofbakfiles += bakfilenamesplited.Length;
                        }
                    }
                    string[] allbakfilenames = new string[numberofbakfiles];
                    string[] allrestoredfilenames = new string[numberofbakfiles];
                    foreach (string file in Directory.GetFiles(folder))
                    {
                        string bakfilename = "";
                        string _restoredfilename = "";
                        if (file.IndexOf(".bak") > -1)
                            bakfilename = file;
                        if (bakfilename != "")
                        {
                            _restoredfilename = bakfilename.Remove(0, bakfilename.IndexOf(".~!") + 3).Remove(bakfilename.Remove(0, bakfilename.IndexOf(".~!") + 3).LastIndexOf("."));
                            allrestoredfilenames.SetValue(_restoredfilename, Array.IndexOf(allrestoredfilenames, null));
                            bakfilename = bakfilename.Remove(bakfilename.IndexOf(".~!"));
                            bakfilename = bakfilename.Remove(0, bakfilename.LastIndexOf("\\") + 1);
                            string[] bakfilenamesplited = bakfilename.Split('&');
                            foreach (string name in bakfilenamesplited)
                            {
                                allbakfilenames.SetValue(name, Array.IndexOf(allbakfilenames, null));
                            }
                        }
                    }
                    foreach (string file in Directory.GetFiles(folder))
                    {
                        string bakfilename = "";
                        if (file.IndexOf(".bak") > -1)
                            bakfilename = file;
                        if (bakfilename != "")
                        {
                            restoredfilename = bakfilename.Remove(0, bakfilename.IndexOf(".~!") + 3).Remove(bakfilename.Remove(0, bakfilename.IndexOf(".~!") + 3).LastIndexOf("."));
                            if (File.Exists(folder + "\\" + restoredfilename) == true & File.Exists(bakfilename) == true)
                            {
                                File.Delete(folder + "\\" + restoredfilename);
                                File.Copy(bakfilename, folder + "\\" + restoredfilename);
                                File.Delete(bakfilename);
                            }
                            if (File.Exists(folder + "\\" + restoredfilename) == false & File.Exists(bakfilename) == true)
                            {
                                File.Copy(bakfilename, folder + "\\" + restoredfilename);
                                File.Delete(bakfilename);
                            }
                            bakfilename = bakfilename.Remove(bakfilename.IndexOf(".~!"));
                            bakfilename = bakfilename.Remove(0, bakfilename.LastIndexOf("\\") + 1);
                            string[] bakfilenamesplited = bakfilename.Split('&');
                            foreach (string osufilename in bakfilenamesplited)
                            {
                                string foundedbg = "";
                                foreach (string _file in Directory.GetFiles(folder))
                                {
                                    if (_file.IndexOf(".osu") > -1)
                                    {
                                        string filename = _file.Remove(0, _file.LastIndexOf("["));
                                        filename = filename.Remove(filename.LastIndexOf("]") + 1);
                                        if (_file.IndexOf(osufilename) < 0 & Array.IndexOf(allbakfilenames, filename) < 0)
                                        {
                                            using (StreamReader reader = new StreamReader(_file))
                                            {
                                                string line;
                                                while ((line = reader.ReadLine()) != null)
                                                {
                                                    if (line.IndexOf("0,0,\"") > -1)
                                                    {
                                                        line = line.Remove(0, 5);
                                                        foundedbg = line.Remove(line.LastIndexOf("\""));
                                                        break;
                                                    }
                                                }
                                            }
                                            string text = File.ReadAllText(_file);
                                            text = text.Replace("0,0,\"" + foundedbg + "\",0,0", "0,0,\"\",0,0");
                                            File.WriteAllText(_file, text);
                                            if (foundedbg != "" & Array.IndexOf(allrestoredfilenames, foundedbg) < 0)
                                                File.Delete(folder + "\\" + foundedbg);
                                        }
                                        if (_file.IndexOf(osufilename) > -1)
                                        {
                                            using (StreamReader reader = new StreamReader(_file))
                                            {
                                                string line;
                                                while ((line = reader.ReadLine()) != null)
                                                {
                                                    if (line.IndexOf("0,0,\"") > -1)
                                                    {
                                                        line = line.Remove(0, 5);
                                                        foundedbg = line.Remove(line.LastIndexOf("\""));
                                                        break;
                                                    }
                                                }
                                            }
                                            if (foundedbg != "")
                                            {
                                                if (File.Exists(folder + "\\" + foundedbg) == true & foundedbg != restoredfilename)
                                                    File.Delete(folder + "\\" + foundedbg);
                                                string text = File.ReadAllText(_file);
                                                text = text.Replace(foundedbg, restoredfilename);
                                                File.WriteAllText(_file, text);
                                            }
                                            else
                                            {
                                                string text = File.ReadAllText(_file);
                                                text = text.Replace("0,0,\"\",0,0", "0,0,\"" + restoredfilename + "\",0,0");
                                                File.WriteAllText(_file, text);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Success();
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
                Console.Write("Enter : ");
                try
                {
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
                catch
                {
                }
            }
        }
    }
}