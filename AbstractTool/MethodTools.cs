using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AbstractTool
{
    /* Class MethodTools that contains all variables and all methods of the program */
    public static class MethodTools
    {
        /* VARIABLES */
        static string[] listBlockWords;
        static string[] nFileByParagraphs;
        static List<string> list = new List<string>();//Test
        static string nPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/AbstractTool/";
        static string nPathDirName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/";
        static string newDirName = "AbstractTool";
        static string nFileData = "";
        static string nFileBlockWord = "";
        static string  nFile  = "LoremIpsum.txt";
        static string  nFileBlock = "BlockWords.txt";
        static int x = 0;
        static int nMenu = 0;
        static string choiceUser;
        // Delta
        // Delta 1 --> 0,90
        // Delta 2 --> 0,85
        // Delta 3 --> 0,75
        static double choiceUserDelta = 0;

        /* OTHERS VARIABLES */
        static string NewLine = Environment.NewLine;
        static string trash;

        /* COLLECTIONS */
        static Dictionary<string, int> dic = new Dictionary<string, int>();
        static Dictionary<string, int> doc = new Dictionary<string, int>();

        /* METHODS */

        // Run the main Menu
        public static void RunAbstractTool()
        {            
            Console.WriteLine("---------------------------");
            Console.WriteLine("-      ABSTRACT TOOL      -");
            Console.WriteLine("---------------------------");

            //Thread.Sleep(3000);

            if (Directory.Exists(nPathDirName + newDirName))
            {
                Console.WriteLine(NewLine+"The directory is already created!");
                trash = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("The directory isn't created yet!");
                //Thread.Sleep(3000);
                Console.WriteLine("Enter the name of the new directory");
                newDirName = Console.ReadLine();
                if (newDirName != String.Empty)
                {
                    Directory.CreateDirectory(nPathDirName + newDirName);
                    if (Directory.Exists(nPathDirName + newDirName))
                    {
                        Console.WriteLine("The directory was created!");
                        Console.ReadKey();
                    }
                }
            }


            do {
                Console.Read();
                //Thread.Sleep(5000);
                Console.Clear();

                Console.WriteLine("-------------");
                Console.WriteLine("- Main Menu -");
                Console.WriteLine("-------------" + NewLine);

                // Thread.Sleep(2000);

                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1 --> Get the info from default file");
                Console.WriteLine("2 --> Get the info from default file by paragraphs");
                Console.WriteLine("3 --> Enter the data and get the info");
                Console.WriteLine("4 --> Enter the data and get the info by paragraphs");
                Console.WriteLine("5 --> Enter the delta");         
                Console.WriteLine("0 --> Exit" + NewLine);
                Console.WriteLine("NOTE: Delta by default = 0, you can choose between 0 and 1"+ NewLine+
                                  "      Delta 1 --> 0,90 | " + "Delta 2 --> 0,85 | " +
                                  "Delta 3 --> 0,75 | Delta x ..." + NewLine);

                // Default Dictionaries
                dic.Clear();
                doc.Clear();


                try {
                    choiceUser = Console.ReadLine();
                    nMenu = Convert.ToInt32(choiceUser);
                }
                catch (FormatException) { Console.WriteLine("{0} is not an integer", choiceUser); }

                switch (nMenu)
                {
                    case 1:
                        CheckFiles(ref nFile, ref nFileBlock, 1);
                        break;
                    case 2:
                        CheckFiles(ref nFile, ref nFileBlock, 2);
                        break;
                    case 3:
                        InputFilesName(1);
                        break;
                    case 4:
                        InputFilesName(2);
                        break;
                    case 5:
                        InputDelta();
                        break;
                    case 0:
                        Console.WriteLine("Prem Enter per a sortir...");
                        Console.ReadLine();
                        Environment.Exit(0);
                        break;
                }
            } while (nMenu !=0);

        }

        // Input text of user
        public static void InputFilesName(int n)
        {

            Console.WriteLine("Enter the name of the file: ");
            nFile = Console.ReadLine();
            nFile += ".txt";

            Console.WriteLine("Enter the name of the file with the data that you don't want: ");
            nFileBlock = Console.ReadLine();
            nFileBlock += ".txt";

            if (n==1) CheckFiles(ref nFile, ref nFileBlock,1);
            if(n==2) CheckFiles(ref nFile, ref nFileBlock,2);
        }

        //Input delta of user
        public static void InputDelta()
        {
            try
            {
                Console.WriteLine("Delta? [Use comma]");
                choiceUser = Console.ReadLine();
                choiceUserDelta = Convert.ToDouble(choiceUser);
            }
            catch (FormatException) { Console.WriteLine("{0} is not an double", choiceUser); }

            Console.WriteLine(choiceUserDelta);

        }

        // Check if exists the files, if are .txt and run the methods
        public static void CheckFiles(ref string nFile, ref string nFileBlock, int x)
        {

            if (File.Exists(nPath + nFileBlock))
            {
                Console.WriteLine("File found");

                FileInfo info = new FileInfo(nPath + nFileBlock);

                if (info.Extension == ".txt")
                {
                    Console.WriteLine("This file have extension .txt");

                    try
                    {
                        using (StreamReader sw2 = new StreamReader(nPath + nFileBlock, Encoding.Default)) nFileBlockWord = sw2.ReadToEnd();
                    }
                    catch(ArgumentNullException) { Console.WriteLine("Stream is null"); }
                    catch (Exception) { Console.WriteLine("Stream does not support reading"); }                    

                    MethodTools.GetBlockWord(nFileBlockWord);

                    Console.WriteLine("File read successfully");
                    
                }
                else Console.WriteLine("This file not have extension .txt");

            }
            else Console.WriteLine("File not found");

            switch (x)
            {
                case 1:
                    // File
                    if (File.Exists(nPath + nFile))
                    {
                        Console.WriteLine("File found");

                        FileInfo info = new FileInfo(nPath + nFile);

                        if (info.Extension == ".txt")
                        {
                            Console.WriteLine("This file have extension .txt");

                            try
                            {
                                using (StreamReader sw = new StreamReader(nPath + nFile, Encoding.Default)) nFileData = sw.ReadToEnd();
                            }
                            catch (ArgumentNullException) { Console.WriteLine("Stream is null"); }
                            catch (Exception) { Console.WriteLine("Stream does not support reading"); }

                            try
                            {
                                File.WriteAllText(nPath + info.Name.Replace(info.Extension, "") + "_info" + ".txt", MethodTools.InfoFile(nPath, nFile, nFileData));
                            }
                            catch (ArgumentNullException) { Console.WriteLine("Path is null or contents is empty"); }
                            catch (NotSupportedException) { Console.WriteLine("Path is in an invalid format."); }
                            catch (ArgumentException) { Console.WriteLine("Path is a zero-length string, contains only white space, or contains one or more invalid characters"); }

                            CreateXML();

                            Console.WriteLine("File created successfully");

                        }
                        else Console.WriteLine("This file not have extension .txt");

                    }
                    else Console.WriteLine("File not found");

                    break;

                case 2:
                    // By paragraphs
                    if (File.Exists(nPath + nFile))
                    {
                        Console.WriteLine("File found");

                        FileInfo info = new FileInfo(nPath + nFile);

                        if (info.Extension == ".txt")
                        {
                            Console.WriteLine("This file have extension .txt");

                            try
                            {
                                using (StreamReader sw = new StreamReader(nPath + nFile, Encoding.Default)) nFileData = sw.ReadToEnd();
                            }
                            catch (ArgumentNullException) { Console.WriteLine("Stream is null"); }
                            catch (Exception) { Console.WriteLine("Stream does not support reading"); }

                            string[] str = InfoFiles(nPath, nFile, nFileData);

                            for (x = 0; x < str.Length; x++)
                            {
                                try {

                                    File.WriteAllText(nPath + info.Name.Replace(info.Extension, "") + "_info_" + (x + 1) + ".txt", str[x]);
                                }
                                catch (ArgumentNullException) { Console.WriteLine("Path is null or contents is empty"); }
                                catch (NotSupportedException) { Console.WriteLine("Path is in an invalid format."); }
                                catch (ArgumentException) { Console.WriteLine("Path is a zero-length string, contains only white space, or contains one or more invalid characters"); }

                            }

                            Console.WriteLine("File created successfully");

                        }
                        else Console.WriteLine("This file not have extension .txt");

                    }
                    else Console.WriteLine("File not found");

                    break;
            }
            
        }

        // Extraction info from file
        // return string
        public static string InfoFile(string nPath, string nFile, string nFileData)
        {

            FileInfo info = new FileInfo(nPath + nFile);

            string nFileInfo = "Nom del fixer: " + info.Name.Replace(info.Extension, "") + NewLine +
                                "Extensió: " + info.Extension + NewLine +
                                "Data: " + info.CreationTimeUtc.ToString("dd/MM/yyyy") + NewLine +
                                "Número de paraules: " + CountWord(nFileData) + NewLine +
                                "Temàtica: " + CountRepeatWords(nFileData) + NewLine;

            return nFileInfo;
        }

        // Extraction info of block words from file
        public static void GetBlockWord(string nFileBlockWords)
        {
            listBlockWords = nFileBlockWords.Split(new [] {"\r\n","\r","\n","]"},StringSplitOptions.RemoveEmptyEntries);

            //foreach (string n in listBlockWords) Console.WriteLine(n);

        }
        
        // Words Repeat Counter
        // return string
        public static string CountRepeatWords(string nFileData)
        {
   
            byte[] utf8 = Encoding.UTF8.GetBytes(nFileData);
            nFileData = Encoding.UTF8.GetString(utf8);

            string[] nFileContentStr = nFileData.Split(new[] { ".", "?", "!", " ", ";", ":", ",", "\n", "\r\n", "\r","'" }, StringSplitOptions.RemoveEmptyEntries);
            dic.Clear();
            doc.Clear();
            
           
            foreach (string n in nFileContentStr)
            {
                if (!listBlockWords.Contains(n.ToLower()))
                {
                    if (dic.ContainsKey(n.ToLower())) dic[n.ToLower()] += 1;
                    else dic.Add(n.ToLower(), 1);
                }
            }

            ///////////////////
            //Delta 1 --> 0,80
            //Delta 2 --> 0,70
            //Delta 3 --> 0,60
            if (choiceUserDelta !=0) {
                string[] dicKeys = new string[dic.Count];
                x = 0;
                foreach (KeyValuePair<string, int> str in dic) doc.Add(str.Key, str.Value);
                foreach (KeyValuePair<string, int> str in dic)
                {
                    dicKeys[x] = str.Key;
                    x++;
                }

                double valor = 0;
                string nAlta;


                foreach (KeyValuePair<string, int> str in dic)
                {
                    for (x = 0; x < dicKeys.Length; x++)
                    {
                        valor = CalculateSimilarity(dicKeys[x], str.Key);
                        //Console.WriteLine(dicKeys[x] + " -- " + str.Key + " --> " + valor);
                        nAlta = (str.Key.Length < dicKeys[x].Length) ? dicKeys[x] : str.Key;

                        if (!str.Key.Equals(dicKeys[x]))
                        {
                            if (doc.ContainsKey(nAlta) && doc.ContainsKey(dicKeys[x])) {
                                if (valor >= choiceUserDelta)
                                {
                                    if (!nAlta.Equals(dicKeys[x])) {
                                        doc[nAlta] += doc[dicKeys[x]];
                                        //Console.WriteLine(doc[dicKeys[x]]);
                                        doc.Remove(dicKeys[x]);
                                    }
                                }
                            }
                        }
                    }
                }

                var List = doc.ToList();
                List.Sort((par1, par2) => par2.Value.CompareTo(par1.Value));

                dic.Clear();
                foreach (KeyValuePair<string, int> str in List) dic.Add(str.Key,str.Value);


                return " " + List[0].Key + ", " + List[1].Key +
                    ", " + List[2].Key + ", " + List[3].Key + ", " + List[4].Key;

            } else {

                var List = dic.ToList();
                List.Sort((par1, par2) => par2.Value.CompareTo(par1.Value));

                return " " + List[0].Key + ", " + List[1].Key +
                    ", " + List[2].Key + ", " + List[3].Key + ", " + List[4].Key;
            }
           
        }

        // Paragraph Counter
        // return string[]
        public static string[] CountParagraphs()
        {
            string[] prueba = nFileData.Split('\n');
            return prueba;
        }

        // Extraction info from file by paragraphs
        // return string[]
        public static string[] InfoFiles(string nPath, string nFile, string nFileData)
        {
            string[] str = CountParagraphs();

            nFileByParagraphs = new string[str.Length];

            FileInfo info = new FileInfo(nPath + nFile);

            for(int z=0;z<str.Length;z++)
            {
                Console.WriteLine("Entra");
                nFileByParagraphs[z] =  "Nom del fitxer: " + info.Name.Replace(info.Extension, "") + NewLine +
                                        "Paràgraf: "+(z+1) + NewLine +
                                        "Extensió: " + info.Extension + NewLine +
                                        "Data: " + info.CreationTimeUtc.ToString("dd/MM/yyyy") + NewLine +
                                        "Número de paraules: " + CountWord(str[z]) + NewLine +
                                        "Temàtica: " + CountRepeatWords(str[z]) + NewLine;
                
                CreateXML(z + 1);
            }

            return nFileByParagraphs;
        }

        // Delta
        public static void Delta(string nFileData)
        {

            char[] takeWhiteSpaceSeparators = null;
            //  Count words
            int wordCount = nFileData.Split(takeWhiteSpaceSeparators, StringSplitOptions.RemoveEmptyEntries).Length;
            // Count repeat words
            string[] wordCount2 = nFileData.Split(takeWhiteSpaceSeparators, StringSplitOptions.RemoveEmptyEntries);
            //
            int v = 0;
            foreach (string str in wordCount2)
            {
                              
                if (str.Length >= 6)
                {
                    list.Add(str.Substring(0, str.Length - 2));
                }
                else list.Add(str);
            }
            Console.WriteLine(list.Count);
            foreach (string str in list) Console.WriteLine(str);
            
            foreach (string str in list)
            {
                //Console.WriteLine("WORD --> " + str);
                if (str.Length >= 6)
                {
                    if (wordCount2.Contains(str))
                    {
                        // Console.WriteLine("ENTRA");
                    }
                    else v++;

                }
              
                Console.WriteLine(str);

            }
            Console.WriteLine(list.Count);
            Console.WriteLine(v);
            
        }

        // Create file xml
        public static void CreateXML(int n = 0)
        {
            FileInfo info = new FileInfo(nPath + nFile);

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode rootNode = xmlDoc.CreateElement(info.Name.Replace(info.Extension, ""));
                xmlDoc.AppendChild(rootNode);

                XmlNode userNode;
                XmlAttribute attribute;

                var dicList = dic.ToList();
                dicList.Sort((par1, par2) => par2.Value.CompareTo(par1.Value));

                foreach (KeyValuePair<string, int> str in dicList)
                {
                    userNode = xmlDoc.CreateElement("word");
                    attribute = xmlDoc.CreateAttribute("occurence");
                    attribute.Value = (str.Value).ToString();
                    userNode.Attributes.Append(attribute);
                    userNode.InnerText = str.Key;
                    rootNode.AppendChild(userNode);
                }

                if (n != 0) xmlDoc.Save(nPath + info.Name.Replace(info.Extension, "") +"_"+ n + ".xml");
                else xmlDoc.Save(nPath + info.Name.Replace(info.Extension, "") + ".xml");

            }
            catch (XmlException) { Console.WriteLine("Could not load or create the XML file"); }
            catch (FileNotFoundException) { Console.WriteLine("XML file not found"); }

            }

        // Word  Counter
        // return int
        public static int CountWord(string nFileData, int n=0)
        {
            char[] takeWhiteSpaceSeparators = null;
            //  Count words
            int wordCount = nFileData.Split(takeWhiteSpaceSeparators, StringSplitOptions.RemoveEmptyEntries).Length;       

            return wordCount;
        }

        /* OTHERS METHODS */

        // Word Repeat Counter
        // return int
        public static int CountRepeatWord(string searchTerm, string nFileData)
        {
            // Converteix a array de String el text llegit 
            string[] source = nFileData.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Query
            var matchQuery = from word in source
                             where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                             select word;

            // Compta les occurences amb el Query  
            int wordCount = matchQuery.Count();

            return wordCount;
        }

        // Method that return the distance between two words
        // return int
        public static int LevenshteinDistance(string source, string target)
        {
            // degenerate cases
            if (source == target) return 0;
            if (source.Length == 0) return target.Length;
            if (target.Length == 0) return source.Length;

            // create two work vectors of integer distances
            int[] v0 = new int[target.Length + 1];
            int[] v1 = new int[target.Length + 1];

            // initialize v0 (the previous row of distances)
            // this row is A[0][i]: edit distance for an empty s
            // the distance is just the number of characters to delete from t
            for (int i = 0; i < v0.Length; i++)
                v0[i] = i;

            for (int i = 0; i < source.Length; i++)
            {
                // calculate v1 (current row distances) from the previous row v0

                // first element of v1 is A[i+1][0]
                //   edit distance is delete (i+1) chars from s to match empty t
                v1[0] = i + 1;

                // use formula to fill in the rest of the row
                for (int j = 0; j < target.Length; j++)
                {
                    var cost = (source[i] == target[j]) ? 0 : 1;
                    v1[j + 1] = Math.Min(v1[j] + 1, Math.Min(v0[j + 1] + 1, v0[j] + cost));
                }

                // copy v1 (current row) to v0 (previous row) for next iteration
                for (int j = 0; j < v0.Length; j++)
                    v0[j] = v1[j];
            }

            return v1[target.Length];
        }

        // Methods that return the similarity between two words
        // return double
        public static double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = LevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }
    }
}