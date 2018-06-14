using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;

namespace TextHandler
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.Run(new Form1());
        //    var run = new Run();

        }

    class Run
        {
            StringCompute String = new StringCompute();
            Dictionary<string, string> NameList= new Dictionary<string,string>();
            internal Run()
            {
                string path = "13w_梦.txt";
                foreach (var VARIABLE in File.ReadAllLines("名字表.txt"))
                {
                    var TN = VARIABLE.Split('|');
                    NameList.Add(TN[0], TN[1]);
                }
                var Txt = new List<string>(File.ReadAllLines(@"13w_梦.txt"));
                SaveToFile(path, TextHandler(Txt));
            }


            private string[] TextHandler(List<string> OriText, int Chapter = 13)
            {
                var Exl = 读取并分析分类表();
                var SaveText = new List<string>();
                var TempText = new List<string>();
                bool StartRec = false;
                string Name = "";
                var Command = new HashSet<string>();
                var Add = new List<string>();
              
                foreach (var Oridat in OriText)
                {
                    if (Oridat.StartsWith("&"))
                    {
                        Command.Add(Oridat);
                    }

                    if (StartRec)
                    {
                        if (Oridat == "“哈哈，那么本人就恭敬不如从命了，")
                        {
                          //  Console.WriteLine();
                        }

                        if (Oridat[Oridat.Length - 1] != '”')
                        {
                            Add.Add(Oridat);
                            TempText.Add(Oridat);
                            continue;
                        }
                        Add.Add(Oridat);
                        bool FindCheck = false;
                        var ContinueCheck = Add.FirstOrDefault(X => X.IndexOf("{{不停}}", StringComparison.Ordinal) != -1);
                        if (ContinueCheck == null)
                        {
                            var BreakCheck = Add.FirstOrDefault(X => X.IndexOf("{{跳过}}", StringComparison.Ordinal) != -1);
                            if (BreakCheck == null)
                            {
                               
                                foreach (var VARIABLE in Add)
                                {
                                    var Ret = Search(VARIABLE);
                                    if (Ret != null)
                                    {
                                        CheckTextExists(Ret);
                                        FindCheck = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                FindCheck = true;
                            }
                        }
                        else
                        {
                            foreach (var VARIABLE in Add)
                            {
                                var Ret = Search(VARIABLE.Replace(@"{{不停}}", ""));
                                if (Ret != null)
                                {
                                    CheckTextExists(Ret);
                                    FindCheck = true;
                                    break;
                                }
                            }
                        }

                        if (!FindCheck)
                        {
                            Console.WriteLine($"未找到文本，行数:{OriText.IndexOf(Oridat)}");
                        }

                        string Search(string T)
                        {
                            T = T.Replace("“", "").Replace("”", "");
                            return Exl[Name].FirstOrDefault(x =>
                            {
                                String.Compute(T, x.Value);
                                return (float) String.ComputeResult.Rate > 0.8;
                            }).Key ?? Exl[Name]
                                .FirstOrDefault(x => x.Value.IndexOf(T, StringComparison.Ordinal) != -1).Key;
                        }
                        /* string Zwei;
                         if (Ein.IndexOf(@"{{不停}}", StringComparison.Ordinal) != -1)
                         {
 
                             Ein = Ein.Replace(@"{{不停}}", "");
 
                             Zwei = Exl[Name].FirstOrDefault(x => x.Value.IndexOf(Ein, StringComparison.Ordinal) != -1)
                                 .Key;
                             CheckTextExists();
                         }
                         else if (Ein.IndexOf(@"{{跳过}}", StringComparison.Ordinal) == -1)
                         {
                             Zwei = Exl[Name].FirstOrDefault(x =>
                             {
                                 String.Compute(Ein, x.Value);
                                 return (float) String.ComputeResult.Rate > 0.8;
                             }).Key;
                             if (Zwei == null)
                             {
                                 Zwei = Exl[Name]
                                     .FirstOrDefault(x => x.Value.IndexOf(Ein, StringComparison.Ordinal) != -1).Key;
                             }
 
                             CheckTextExists();
                         }*/

                        void CheckTextExists(string Zwei)
                        {
                            if (!string.IsNullOrWhiteSpace(Zwei))
                            {
                                var AddS = $"&{NameList[Name]}={Chapter}_{Zwei}.ogg";
                                if (!Command.Contains(AddS))
                                {
                                    SaveText.Add(AddS);
                                }
                                Exl[Name].Remove(Zwei);
                            }
                            else
                            {
                                Console.WriteLine($"未找到文本，行数:{SaveText.Count}");
                            }
                        }

                        StartRec = false;
                    }

                    if (Oridat.StartsWith("【"))
                    {
                        Name = Oridat.Replace("【", "").Replace("】", "");
                        if (!string.IsNullOrWhiteSpace(Name))
                        {
                            if (Exl.ContainsKey(Name))
                            {
                                //SaveText.AddRange(Add.ToArray());
                                Add.Clear();
                                SaveText.AddRange(TempText.ToArray());
                                TempText.Clear();
                                StartRec = true;
                            }
                        }
                    }

                    TempText.Add(Oridat);
                }

                SaveText.AddRange(TempText.ToArray());
                return SaveText.ToArray();
            }

            private void SaveToFile(string path, string[] Date)
            {
                var Fol = new DirectoryInfo("Save");
                if (!Directory.Exists("Save"))
                {
                    Fol.Create();
                }
            
              /*  for (int i = 0; i < Date.Length; i++)
                {
                    Date[i] =i+":"+ Date[i];
                }*/
                File.WriteAllLines($"{Fol.FullName}\\{path}", Date);
            }
            Dictionary<string, Dictionary<string, string>> 读取并分析分类表(string Path = "13.xlsx")
            {
                var 分类列表 = new Dictionary<string, Dictionary<string, string>>();
                using (var reader =
                    ExcelReaderFactory.CreateReader(new FileStream(Path, FileMode.Open, FileAccess.Read,
                        FileShare.ReadWrite)))
                {
                    foreach (DataTable Table in reader.AsDataSet().Tables)
                    {
                        var Dat = new Dictionary<string, string>();
                        foreach (DataRow Date in Table.Rows)
                        {
                            Dat.Add(Date[0].ToString(), Date[2].ToString().Replace("“","").Replace("”",""));
                        }

                        分类列表.Add(Table.TableName, Dat);
                    }
                }

                分类列表.Remove("古梦诗");
                return 分类列表;
            }
        }
    }
}
