using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace MazeAnalyzer
{
    public static class CurrentSettings
    {
        //public static string curDirectory;
        //public static List<string> curPaths = new List<string>(10);


        //public static string curAppDirectory;
        public static string curRegDirectory;  // Application.CommonAppDataPath

        private static string curSettingsFile = "\\settings.xml";
        public static string curSettingsPath = "";

        public static bool showPropertiesPane = true;
        public static bool showListPane = true;
        public static int themeIndex = 0;

        //public static Queue<string> previousFiles = new Queue<string>(10);

        public static List<string> previousMazeFiles = new List<string>(10);

        public static List<string> previousProjectFiles = new List<string>(10);

        public static List<string> previousLogFiles = new List<string>(10);

        public static List<string> previousRegionDefinitionFiles = new List<string>(10);
        
        public static bool ReadSettings()
        {
            return ReadSettings(Application.CommonAppDataPath);
        }

        public static bool SaveSettings()
        {
            return SaveSettings(Application.CommonAppDataPath);
        }

        //inp: Application.CommonAppDataPath
        public static bool ReadSettings(string inp)
        {
            if (curSettingsPath == "")
            {
                curSettingsPath = inp.Substring(0, inp.LastIndexOf('\\'));
                curRegDirectory = inp + "\\";
            }
            try
            {
                String label;
                XmlTextReader sw = new XmlTextReader(curSettingsPath + curSettingsFile);
                while (sw.Read())
                {
                    if (sw.NodeType == XmlNodeType.Element)
                    {
                        label = sw.Name;
                        if (label.Contains("Panes"))
                        {
                            sw.Read();
                            sw.ReadStartElement("properties");
                            if (sw.ReadString().CompareTo("False")==0)
                                showPropertiesPane = false;
                            else
                                showPropertiesPane = true;
                            sw.ReadEndElement();
                            sw.ReadStartElement("list");
                            if (sw.ReadString().CompareTo("False")==0)
                                showListPane = false;
                            else
                                showListPane = true;
                            sw.ReadEndElement();

                        }
                        //else if (label.Contains("Path"))
                        //{
                        //    sw.Read();
                        //    sw.ReadStartElement("name");
                        //    curPaths.Add(sw.ReadString());
                        //    sw.ReadEndElement();
                        //}
                        else if (label.Contains("ProjectFiles"))
                        {
                            sw.Read();
                            sw.ReadStartElement("Count");
                            int num = int.Parse(sw.ReadString());
                            sw.ReadEndElement();
                            if (num > 10) num = 10;
                            previousProjectFiles.Clear();
                            if (num > 0)
                            {
                                for (int i = 0; i < num; i++)
                                {
                                    sw.ReadStartElement("File");
                                    //previousFiles.Enqueue(sw.ReadString());
                                    previousProjectFiles.Add(sw.ReadString());
                                    sw.ReadEndElement();
                                }
                            }
                            //sw.ReadEndElement();
                        }

                        else if (label.Contains("MazeFiles"))
                        {
                            sw.Read();
                            sw.ReadStartElement("Count");
                            int num = int.Parse(sw.ReadString());
                            sw.ReadEndElement();
                            if (num > 10) num = 10;
                            previousMazeFiles.Clear();
                            if (num > 0)
                            {
                                for (int i = 0; i < num; i++)
                                {
                                    sw.ReadStartElement("File");
                                    //previousFiles.Enqueue(sw.ReadString());
                                    previousMazeFiles.Add(sw.ReadString());
                                    sw.ReadEndElement();
                                }
                            }
                            //sw.ReadEndElement();
                        }
                        else if (label.Contains("LogFiles"))
                        {
                            sw.Read();
                            sw.ReadStartElement("Count");
                            int num = int.Parse(sw.ReadString());
                            sw.ReadEndElement();
                            if (num > 10) num = 10;
                            previousLogFiles.Clear();
                            if (num > 0)
                            {
                                for (int i = 0; i < num; i++)
                                {
                                    sw.ReadStartElement("File");
                                    previousLogFiles.Add(sw.ReadString());
                                    sw.ReadEndElement();
                                }
                            }
                            //sw.ReadEndElement();
                        }
                        else if (label.Contains("RegionFiles"))
                        {
                            sw.Read();
                            sw.ReadStartElement("Count");
                            int num = int.Parse(sw.ReadString());
                            sw.ReadEndElement();
                            if (num > 10) num = 10;
                            previousRegionDefinitionFiles.Clear();
                            if (num > 0)
                            {
                                for (int i = 0; i < num; i++)
                                {
                                    sw.ReadStartElement("File");
                                    previousRegionDefinitionFiles.Add(sw.ReadString());
                                    sw.ReadEndElement();
                                }
                            }
                            //sw.ReadEndElement();
                        }
                        else if(label.Contains("Theme"))
                        {
                            sw.Read();
                            sw.ReadStartElement("style");
                            int.TryParse(sw.ReadString(),out themeIndex);
                            
                            sw.ReadEndElement();
                        }

                    }
                }
                sw.Close();
            }
            catch//(Exception ex)
            {
                return false;
            }

            return true;
        }

        //inp: Application.CommonAppDataPath
        public static bool SaveSettings(string inp)
        {
            if (curSettingsPath == "")
            {
                curSettingsPath = inp.Substring(0, inp.LastIndexOf('\\'));
                curRegDirectory = inp + "\\";
            }

            try
            {
                XmlTextWriter sw = new XmlTextWriter(curSettingsPath + curSettingsFile, System.Text.ASCIIEncoding.UTF8);
                sw.WriteStartDocument();
                sw.WriteStartElement("CT");

                sw.WriteStartElement("MazeFiles");
                sw.WriteElementString("Count", previousMazeFiles.Count.ToString());
                foreach (string s in previousMazeFiles)
                {
                    sw.WriteElementString("File", s);
                }
                sw.WriteEndElement();


                sw.WriteStartElement("ProjectFiles");
                sw.WriteElementString("Count", previousProjectFiles.Count.ToString());
                foreach (string s in previousProjectFiles)
                {
                    sw.WriteElementString("File", s);
                }
                sw.WriteEndElement();

                sw.WriteStartElement("LogFiles");
                sw.WriteElementString("Count", previousLogFiles.Count.ToString());
                foreach (string s in previousLogFiles)
                {
                    sw.WriteElementString("File", s);
                }
                sw.WriteEndElement();

                sw.WriteStartElement("RegionFiles");
                sw.WriteElementString("Count", previousRegionDefinitionFiles.Count.ToString());
                foreach (string s in previousRegionDefinitionFiles)
                {
                    sw.WriteElementString("File", s);
                }
                sw.WriteEndElement();


                sw.WriteStartElement("Panes");
                sw.WriteElementString("properties", showPropertiesPane.ToString());
                sw.WriteElementString("list", showListPane.ToString());
                sw.WriteEndElement();

                sw.WriteStartElement("Theme");
                sw.WriteElementString("style", themeIndex.ToString());
                sw.WriteEndElement();


                //foreach (string s in curPaths)
                //{
                //    sw.WriteStartElement("Path");
                //    sw.WriteElementString("name", s);
                //    sw.WriteEndElement();
                //}
                sw.WriteEndElement();
                sw.WriteEndDocument();
                sw.Close();
            }
            catch//(Exception ex)
            {
                return false;
            }
            return true;
        }


        //public static bool AddPreviousFile(string str)
        //{
        //    try
        //    {
        //        if (previousFiles.Count == 0)
        //        {
        //            previousFiles.Enqueue(str);
        //        }
        //        else if (previousFiles.Contains(str) == false)
        //        {
        //            if(previousFiles.Count>=10)
        //                previousFiles.Dequeue();
        //            previousFiles.Enqueue(str);
        //        }                
        //        return true;
        //    }
        //    catch //(System.Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public static bool AddProjectFileToPrevious(string str)
        {
            try
            {
                if (previousProjectFiles.Contains(str))
                {
                    previousProjectFiles.Remove(str);
                }
                previousProjectFiles.Add(str);

                if (previousProjectFiles.Count > 10)
                    previousProjectFiles.RemoveAt(0);

                return true;
            }
            catch //(System.Exception ex)
            {
                return false;
            }
        }

        public static bool AddMazeFileToPrevious(string str)
        {
            try
            {
                if (previousMazeFiles.Contains(str))
                {
                    previousMazeFiles.Remove(str);
                }
                previousMazeFiles.Add(str);

                if (previousMazeFiles.Count > 10)
                    previousMazeFiles.RemoveAt(0);

                return true;
            }
            catch //(System.Exception ex)
            {
                return false;
            }
        }

        public static bool AddLogFileToPrevious(string str)
        {
            try
            {
                if (previousLogFiles.Contains(str))
                {
                    previousLogFiles.Remove(str);
                }
                previousLogFiles.Add(str);

                if (previousLogFiles.Count > 10)
                    previousLogFiles.RemoveAt(0);

                return true;
            }
            catch //(System.Exception ex)
            {
                return false;
            }
        }

        public static bool AddRegionFileToPrevious(string str)
        {
            try
            {
                if (previousRegionDefinitionFiles.Contains(str))
                {
                    previousRegionDefinitionFiles.Remove(str);
                }
                previousRegionDefinitionFiles.Add(str);

                if (previousRegionDefinitionFiles.Count > 10)
                    previousRegionDefinitionFiles.RemoveAt(0);

                return true;
            }
            catch //(System.Exception ex)
            {
                return false;
            }
        }
    }
}
