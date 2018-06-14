using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;

namespace TextHandler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Dictionary<int, Dictionary<string, Dictionary<string, string>>> Voice =
            new Dictionary<int, Dictionary<string, Dictionary<string, string>>>();

        int Name2Page(string s)
        {
            string Num = "";

            foreach (var VARIABLE in s.Select(x => x.ToString()).ToArray())
            {
                if (int.TryParse(VARIABLE, out int ret))
                {
                    Num += VARIABLE;
                }
                else
                {
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(Num)) return -1;
            return int.Parse(Num);
        }
        HashSet<string> NameList = new HashSet<string>();
        private void OpenXlxs_Click(object sender, EventArgs e)
        {
            Dictionary<string, Dictionary<string, string>> VoiceListHandle(string Path)
            {
                var List = new Dictionary<string, Dictionary<string, string>>();
                using (var reader =
                    ExcelReaderFactory.CreateReader(new FileStream(Path, FileMode.Open, FileAccess.Read,
                        FileShare.ReadWrite)))
                {
                    foreach (DataTable Table in reader.AsDataSet().Tables)
                    {
                        var Dat = new Dictionary<string, string>();
                        foreach (DataRow Date in Table.Rows)
                        {
                            var Temp = Date[0].ToString();
                            if (Temp == Table.TableName)
                            {
                                Temp = Date[1].ToString();
                            }
                            Dat.Add(Temp, Date[2].ToString().Replace("“", "").Replace("”", ""));
                        }
                        List.Add(Table.TableName, Dat);
                        if (!NameList.Contains(Table.TableName))
                        {
                            NameList.Add(Table.TableName);
                        }
                    }
                }

                return List;
            }

            var Fol = new FolderBrowserDialog();
            Fol.Description = @"选择语音表";
            Fol.ShowNewFolderButton = false;
            Fol.ShowDialog();
            if (Fol.SelectedPath != "")
            {
                Voice.Clear();
                //Voice[章节][姓名][路径,名字]
                foreach (var VARIABLE in new DirectoryInfo(Fol.SelectedPath).GetFiles("*.xlsx"))
                {
                    var cap = Name2Page(VARIABLE.Name);
                    if (cap == -1) continue;
                    var Xlsx = VoiceListHandle(VARIABLE.FullName);
                    if (Voice.ContainsKey(cap))//章节是否存在
                    {
                        foreach (var VARIABLE2 in Xlsx)//存在章节，遍历获取
                        {
                            if (Voice[cap].ContainsKey(VARIABLE2.Key))//索引章节，判断名字是否存在
                            {
                                foreach (var VARIABLE3 in VARIABLE2.Value)//名字存在，添加数据
                                {
                                    if (!Voice[cap][VARIABLE2.Key].ContainsKey(VARIABLE3.Key))//数据不存在，添加
                                    Voice[cap][VARIABLE2.Key].Add(VARIABLE3.Key, VARIABLE3.Value);
                                }
                            }
                            else//索引章节，名字不存在
                            {
                                Voice[cap].Add(VARIABLE2.Key, VARIABLE2.Value);
                            }
                        }
                    }
                    else
                    {
                        Voice.Add(cap, Xlsx);
                    }

                }
                VoiceList.Items.Clear();
                VoiceList.Items.AddRange(NameList.ToArray());
            }

        }

        internal class TxtFile
        {
            internal int Cap;
            internal string Name;
            internal List<string> Log=new List<string>();
            internal string[] Ori;
            internal List<string> Save=new List<string>();
        }

        private List<TxtFile> Scr = new List<TxtFile>();
        private void LoadScr_Click(object sender, EventArgs e)
        {
            var Fol = new FolderBrowserDialog();
            Fol.Description = @"选择脚本目录";
            Fol.ShowNewFolderButton = false;
            Fol.ShowDialog();
            if (Fol.SelectedPath != "")
            {
                Scr.Clear();
                foreach (var VARIABLE in new DirectoryInfo(Fol.SelectedPath).GetFiles("*.txt"))
                {
                    var cap = Name2Page(VARIABLE.Name);
                    if (cap == -1) continue;
                    var temp = new TxtFile();
                    temp.Name = VARIABLE.Name;
                    temp.Cap = cap;
                    temp.Ori = File.ReadAllLines(VARIABLE.FullName);
                    Scr.Add(temp);
                }
                CountS.Text = Scr.Count.ToString();
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (Voice.Count != 0 && Scr.Count != 0)
            {
                var VoiceCopy = new Dictionary<int, Dictionary<string, Dictionary<string, string>>>(Voice);
                var _NameList = NameList.ToList();
                foreach (var VARIABLE in VoiceList.CheckedItems)
                {
                    _NameList.Remove(VARIABLE.ToString());
                }
                foreach (var VARIABLE in _NameList)
                {
                    foreach (Dictionary<string, Dictionary<string, string>> VARIABLE2 in VoiceCopy.Values)
                    {
                        VARIABLE2.Remove(VARIABLE);
                    }
                }
                foreach (var VARIABLE in Scr)
                {
                    TextHandler(VARIABLE, VoiceCopy[VARIABLE.Cap]);
                }
                var Fol = new DirectoryInfo("Save");
                var LOG = new DirectoryInfo("log");
                if (!Directory.Exists("Save"))
                {
                    LOG.Create();
                }
                if (!Directory.Exists("log"))
                {
                    LOG.Create();
                }

                foreach (var VARIABLE in Scr)
                {
                    File.WriteAllLines($"{Fol.FullName}\\{VARIABLE.Name}", VARIABLE.Save.ToArray());
                    File.WriteAllLines($"{LOG.FullName}\\{VARIABLE.Name}", VARIABLE.Log.ToArray());
                }

                MessageBox.Show("完成");
            }
            else
            {
                MessageBox.Show("请准备好后再运行");
            }
        }

        private void TextHandler(TxtFile OriText, Dictionary<string, Dictionary<string, string>> Exl)
        {
            var TempText = new List<string>();
            bool StartRec = false;
            string Name = "";
            var Command = new HashSet<string>();
            var Add = new List<string>();

            foreach (var Oridat in OriText.Ori)
            {
                if (Oridat.StartsWith("&"))
                {
                    Command.Add(Oridat);
                }

                if (StartRec)
                {
                    if (Oridat[Oridat.Length - 1] != '”')
                    {
                        Add.Add(Oridat);
                        TempText.Add(Oridat);
                        continue;
                    }
                    Add.Add(Oridat);
                    var ContinueCheck = Add.FirstOrDefault(X => X.IndexOf("{{不停}}", StringComparison.Ordinal) != -1);
                    if (ContinueCheck == null)
                    {
                        var BreakCheck = Add.FirstOrDefault(X => X.IndexOf("{{跳过}}", StringComparison.Ordinal) != -1);
                        if (BreakCheck == null)
                        {
                            var Find = false;
                            foreach (var VARIABLE in Add)
                            {
                                var Ret = Search(VARIABLE);
                                if (Ret != null)
                                {
                                    CheckTextExists(Ret);
                                    Find = true;
                                    break;
                                }
                               
                            }
                            if (!Find)
                            {
                                OriText.Log.Add($"原始文本未找到语音，行数:{OriText.Save.Count}");
                            }
                        }
                    }
                    else
                    {
                        var Find = false;
                        foreach (var VARIABLE in Add)
                        {
                            var Ret = Search(VARIABLE.Replace(@"{{不停}}", ""));
                            if (Ret != null)
                            {
                                CheckTextExists(Ret);
                                Find = true;
                                break;
                            }
                        }
                        if (!Find)
                        {
                            OriText.Log.Add($"未找到文本，行数:{OriText.Save.Count}");
                        }
                    }

                    string Search(string T)
                    {
                        StringCompute String = new StringCompute();
                        T = T.Replace("“", "").Replace("”", "");
                        return Exl[Name].FirstOrDefault(x =>
                        {
                            String.Compute(T, x.Value);
                            return (float)String.ComputeResult.Rate > 0.8;
                        }).Key ?? Exl[Name]
                            .FirstOrDefault(x => x.Value.IndexOf(T, StringComparison.Ordinal) != -1).Key;
                    }

                    void CheckTextExists(string Zwei)
                    {
                        if (!string.IsNullOrWhiteSpace(Zwei))
                        {
                            var AddS = $"&{Name}={OriText.Cap}_{Zwei}.ogg";
                            if (!Command.Contains(AddS))
                            {
                                OriText.Save.Add(AddS);
                            }
                            Exl[Name].Remove(Zwei);
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
                            Add.Clear();
                            OriText.Save.AddRange(TempText.ToArray());
                            TempText.Clear();
                            StartRec = true;
                        }
                    }
                }

                TempText.Add(Oridat);
            }
            OriText.Save.AddRange(TempText.ToArray());
        }
    }
}
