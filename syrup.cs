using System.Collections.Generic;
using PluginContracts;
using PluginLoader;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.IO.Compression;




namespace Syrup
{
    class syrup
    {

        static string archiveDir = "Archives";
        static string pluginDir = "Plugins";
        static string compiledDir = "../BUTTER_Client/bin/Release/Plugins/";

        static void Main(string[] args)
        {


            //populate our list of plugins
            Dictionary<string, Plugin> _Plugins = new Dictionary<string, Plugin>();
            Dictionary<string, string> PluginMD5s = new Dictionary<string, string>();
            SerializableDictionary<string, SerializableDictionary<string, string>> PluginDetails = new SerializableDictionary<string, SerializableDictionary<string, string>>();

            Dictionary<string, List<string>> pluginChanges = new Dictionary<string, List<string>>();
            pluginChanges.Add("Version", new List<string>());
            pluginChanges.Add("MD5", new List<string>());
            pluginChanges.Add("New", new List<string>());



            var dirToDel = new DirectoryInfo(archiveDir);
            if (dirToDel.Exists) Directory.Delete(archiveDir, true);
            while (Directory.Exists(archiveDir))
            {
                System.Threading.Thread.Sleep(100);
                dirToDel.Refresh();
            }

            Directory.CreateDirectory(archiveDir);
            while (!Directory.Exists(archiveDir))
            {
                System.Threading.Thread.Sleep(100);
                dirToDel.Refresh();
            }


            #region file details
            Dictionary<string, List<string>> pluginFiles = new Dictionary<string, List<string>>();
            pluginFiles.Add("aaa_IO_InputFilesCorpus.zip", new List<string>() { "aaa_IO_InputFilesCorpus.dll" });
            pluginFiles.Add("aaa_IO_InputFilesCSV.zip", new List<string>() { "aaa_IO_InputFilesCSV.dll" });
            pluginFiles.Add("aaa_IO_InputFilesDOCX.zip", new List<string>() { "aaa_IO_InputFilesDOCX.dll", "Dependencies/DocumentFormat.OpenXml.dll" });
            pluginFiles.Add("aaa_IO_InputFilesPDF.zip", new List<string>() { "aaa_IO_InputFilesPDF.dll", "Dependencies/itextsharp.dll" });
            pluginFiles.Add("aaa_IO_InputFilesTXT.zip", new List<string>() { "aaa_IO_InputFilesTXT.dll" });
            pluginFiles.Add("aaa_IO_OutputFileCSV.zip", new List<string>() { "aaa_IO_OutputFileCSV.dll" });
            pluginFiles.Add("aaa_IO_OutputFilesTXT.zip", new List<string>() { "aaa_IO_OutputFilesTXT.dll" });
            pluginFiles.Add("aaa_IO_OutputTXTBuildCorpus.zip", new List<string>() { "aaa_IO_OutputTXTBuildCorpus.dll" });
            pluginFiles.Add("aaa_IO_zGetTextEncoding.zip", new List<string>() { "aaa_IO_zGetTextEncoding.dll", "Dependencies/Ude.dll", "Licenses/Ude.txt" });
            pluginFiles.Add("CompareFrequencies.zip", new List<string>() { "CompareFrequencies.dll" });
            pluginFiles.Add("CorpusTools_FrequencyList.zip", new List<string>() { "CorpusTools_FrequencyList.dll" });
            pluginFiles.Add("CSVtoLIWCdic.zip", new List<string>() { "CSVtoLIWCdic.dll" });
            pluginFiles.Add("GroupDyad_aaa_GroupDataPackager.zip", new List<string>() { "GroupDyad_aaa_GroupDataPackager.dll", "Dependencies/GroupDataObj.dll" });
            pluginFiles.Add("GroupDyad_aaa_GroupDataUnPackager.zip", new List<string>() { "GroupDyad_aaa_GroupDataUnPackager.dll", "Dependencies/GroupDataObj.dll" });
            pluginFiles.Add("GroupDyad_aab_ConversationSplitter.zip", new List<string>() { "GroupDyad_aab_ConversationSplitter.dll", "Dependencies/GroupDataObj.dll" });
            pluginFiles.Add("GroupDyad_aab_DetectSpeakers.zip", new List<string>() { "GroupDyad_aab_DetectSpeakers.dll" });
            pluginFiles.Add("GroupDyad_LSM.zip", new List<string>() { "GroupDyad_LSM.dll", "Dependencies/GroupDataObj.dll" });
            pluginFiles.Add("GroupDyad_LSS.zip", new List<string>() { "GroupDyad_LSS.dll", "Dependencies/GroupDataObj.dll" });
            pluginFiles.Add("LangAnalysis_AverageWordVector.zip", new List<string>() { "LangAnalysis_AverageWordVector.dll" });
            pluginFiles.Add("LangAnalysis_ContentCoding.zip", new List<string>() { "LangAnalysis_ContentCoding.dll", "Licenses/FinancialSentimentDictionary.txt" });
            pluginFiles.Add("LangAnalysis_DDR.zip", new List<string>() { "LangAnalysis_DDR.dll" });
            pluginFiles.Add("LangAnalysis_DocTermMatrix.zip", new List<string>() { "LangAnalysis_DocTermMatrix.dll" });
            pluginFiles.Add("LangAnalysis_ExamineDictWords.zip", new List<string>() { "LangAnalysis_ExamineDictWords.dll" });
            pluginFiles.Add("LangAnalysis_LanguageIdentifier.zip", new List<string>() { "LangAnalysis_LanguageIdentifier.dll", "Dependencies/NCatLibModels/", "Dependencies/IvanAkcheurov.Commons.dll", "Dependencies/IvanAkcheurov.NClassify.dll", "Dependencies/IvanAkcheurov.NTextCat.Lib.dll", "Licenses/ncatlib.txt" });
            pluginFiles.Add("LangAnalysis_LexicalDiversity.zip", new List<string>() { "LangAnalysis_LexicalDiversity.dll" });
            pluginFiles.Add("LangAnalysis_NarrativeArc.zip", new List<string>() { "LangAnalysis_NarrativeArc.dll" });
            pluginFiles.Add("LangAnalysis_ReadabilityMetrics.zip", new List<string>() { "LangAnalysis_ReadabilityMetrics.dll", "Licenses/TextStatistics.NET.txt" });
            pluginFiles.Add("LangAnalysis_WeightedDictionary.zip", new List<string>() { "LangAnalysis_WeightedDictionary.dll", "Licenses/AFINN.txt", "Licenses/WRAD.md", "Licenses/WRRL.txt", "Licenses/MoralStrength.txt", });
            pluginFiles.Add("Lemmatizer_LemmaGen.zip", new List<string>() { "Lemmatizer_LemmaGen.dll", "Dependencies/LemmaSharp.dll", "Dependencies/LemmaSharpPrebuilt.dll", "Dependencies/LemmaSharpPrebuiltCompact.dll", "Dependencies/Lzma.dll" });
            pluginFiles.Add("Lemmatizer_Lookup.zip", new List<string>() { "Lemmatizer_Lookup.dll", "Licenses/lemmatization-lists.txt" });
            pluginFiles.Add("LIWCdicToCSV.zip", new List<string>() { "LIWCdicToCSV.dll" });
            pluginFiles.Add("POS_POSTagger_CoreNLP.zip", new List<string>() { "Dependencies/IKVM_8_1_5717", "Dependencies/stanford-postagger-full-2018-02-27", "POS_POSTagger_CoreNLP.dll", "Dependencies/POS_TaggerOutputObjectLibrary.dll", "Licenses/CoreNLP.txt" });
            pluginFiles.Add("POS_zPOSTagCategoryCounts.zip", new List<string>() { "POS_zPOSTagCategoryCounts.dll", "Dependencies/POS_TaggerOutputObjectLibrary.dll" });
            pluginFiles.Add("POS_zPOSTaggedTextToString.zip", new List<string>() { "POS_zPOSTaggedTextToString.dll", "Dependencies/POS_TaggerOutputObjectLibrary.dll" });
            pluginFiles.Add("POS_zPOSTaggedTextToTokens.zip", new List<string>() { "POS_zPOSTaggedTextToTokens.dll", "Dependencies/POS_TaggerOutputObjectLibrary.dll" });
            pluginFiles.Add("Preproc_Contextualizer.zip", new List<string>() { "Preproc_Contextualizer.dll", "Dependencies/ContextObj.dll" });
            pluginFiles.Add("Preproc_ContextualizerHelper.zip", new List<string>() { "Preproc_ContextualizerHelper.dll", "Dependencies/ContextObj.dll" });
            pluginFiles.Add("Preproc_OmitObservation.zip", new List<string>() { "Preproc_OmitObservation.dll" });
            pluginFiles.Add("Preproc_Phrasifier.zip", new List<string>() { "Preproc_Phrasifier" +
                ".dll" });
            pluginFiles.Add("Preproc_RegExReplace.zip", new List<string>() { "Preproc_RegExReplace.dll" });
            pluginFiles.Add("Preproc_SplitTextIntoChunks.zip", new List<string>() { "Preproc_SplitTextIntoChunks.dll" });
            pluginFiles.Add("Preproc_StopList.zip", new List<string>() { "Preproc_StopList.dll" });
            pluginFiles.Add("Sentiment_CoreNLPSentiment.zip", new List<string>() { "Sentiment_CoreNLPSentiment.dll", "Dependencies/IKVM_8_1_5717", "Dependencies/stanford-corenlp-full-2018-02-27", "Licenses/CoreNLP.txt" });
            pluginFiles.Add("Sentiment_VADER.zip", new List<string>() { "Sentiment_VADER.dll", "Dependencies/VaderSharp.dll", "Licenses/VADERSharp.txt" });
            pluginFiles.Add("Tokenize_aaa_TwitterAware.zip", new List<string>() { "Tokenize_aaa_TwitterAware.dll", "Licenses/HappyFunTokenizer.txt" });
            pluginFiles.Add("Tokenize_aab_ChineseCoreNLP.zip", new List<string>() { "Tokenize_aab_ChineseCoreNLP.dll", "Dependencies/ZhTokenData", "Licenses/CoreNLP.txt" });
            pluginFiles.Add("Tokenize_aab_ChineseJieba.zip", new List<string>() { "Tokenize_aab_ChineseJieba.dll", "Dependencies/JiebaNET", "Dependencies/JiebaNet.Segmenter.dll", "Dependencies/Newtonsoft.Json.dll", "Licenses/jiebaNET.txt" });
            pluginFiles.Add("Tokenize_aab_WhitespaceTokenizer.zip", new List<string>() { "Tokenize_aab_WhitespaceTokenizer.dll" });
            pluginFiles.Add("Tokenize_zTokens2String.zip", new List<string>() { "Tokenize_zTokens2String.dll" });
            pluginFiles.Add("WebAPI_ReceptivitiAPI.zip", new List<string>() { "WebAPI_ReceptivitiAPI.dll", "Dependencies/Newtonsoft.Json.dll", "Dependencies/RestSharp.dll" });
            pluginFiles.Add("WordEmb_FindSimilarWordsPlugin.zip", new List<string>() { "WordEmb_FindSimilarWordsPlugin.dll" });
            pluginFiles.Add("WordEmb_Word2Vec.zip", new List<string>() { "WordEmb_Word2Vec.dll", "Dependencies/Word2Vec.Net.dll", "Licenses/Word2Vec.NET.txt" });

            pluginFiles.Add("LangAnalysis_ConceptCatDiv.zip", new List<string>() { "LangAnalysis_ConceptCatDiv.dll", "Dependencies/ConceptCatDivDicts" });

            #endregion


            bool existingDat = false;

            if (File.Exists("BUTTER-Plugin-Dat.xml"))
            {
                existingDat = true;
                using (Stream reader = new FileStream("BUTTER-Plugin-Dat.xml", FileMode.Open))
                {
                    XmlSerializer serializerInput = new XmlSerializer(typeof(SerializableDictionary<string, SerializableDictionary<string, string>>));
                    PluginDetails = (SerializableDictionary<string, SerializableDictionary<string, string>>)serializerInput.Deserialize(reader);
                }

                FileInfo datInfo = new FileInfo("BUTTER-Plugin-Dat.xml");

                string newFilename = "BUTTER-Plugin-Dat_" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + datInfo.Extension;
                File.Move(datInfo.FullName, datInfo.Directory.FullName + "\\" + newFilename);

            }




            Console.WriteLine("Compressing files... " + pluginFiles.Keys.Count.ToString() + " plugins");
            CompressFiles(pluginFiles);

            Console.WriteLine("Getting plugin info...");


            //get a list of all zip files
            List<string> files = Directory.EnumerateFiles(archiveDir + "/", "*.*",
                                              SearchOption.TopDirectoryOnly)
                       .Where(n => Path.GetExtension(n) == ".zip").ToList();

            foreach(string file in files)
            {
                string md5hash = CalculateMD5(file);
                //we've calculated the hash, now we need to:
                // 1- extract the file
                // 2- load the plugin
                // 3- get the details for the plugin
                // 4- save the hash for the plugin name as well
                // 5- delete the extracted plugins



                //1- extract the file
                //ZipFile.ExtractToDirectory(file, "Archives/");

                using (ZipArchive zip = ZipFile.OpenRead(file)) { 
                    foreach(ZipArchiveEntry entry in zip.Entries)
                    {
                        if (!File.Exists(entry.FullName)) 
                        {
                            string dirToCreate = pluginDir + "/" + entry.FullName;
                            if (entry.FullName.Contains(entry.Name) && !String.IsNullOrEmpty(entry.Name)) dirToCreate = entry.FullName.Replace(entry.Name, "");
                            Directory.CreateDirectory(dirToCreate);

                            if (entry.Name != "") entry.ExtractToFile(entry.FullName, false);
                        }
                    
                    }
                }

                List<string> dllfiles = Directory.EnumerateFiles(pluginDir, "*.dll",
                                              SearchOption.TopDirectoryOnly)
                       .Where(n => Path.GetExtension(n) == ".dll").ToList();

                string dllfile = dllfiles[0];

                
                
                // 2- load the plugin
                ICollection<Plugin> plugins = GenericPluginLoader<Plugin>.UnsafeLoadPlugins(pluginDir);

                File.Delete(dllfile);
                



                foreach (var plug in plugins)
                {
                    if (!_Plugins.ContainsKey(plug.PluginName))
                    {
                        Console.WriteLine("\t" + plug.PluginName);
                        _Plugins.Add(plug.PluginName, plug);

                        if (!existingDat)
                        {
                            // 3- get the details for the plugin
                            PluginDetails = AddEntry(PluginDetails, plug, md5hash, file);
                        }
                        else
                        {
                            if (!PluginDetails.ContainsKey(plug.PluginName))
                            {
                                pluginChanges["New"].Add(plug.PluginName);
                                PluginDetails = AddEntry(PluginDetails, plug, md5hash, file);
                            }
                            else
                            {
                                if (PluginDetails[plug.PluginName]["Version"] != plug.PluginVersion)
                                {
                                    pluginChanges["Version"].Add(plug.PluginName);
                                    pluginChanges["MD5"].Add(plug.PluginName);
                                    PluginDetails = UpdateEntry(PluginDetails, plug, md5hash, file);
                                }

                                if (PluginDetails[plug.PluginName]["MD5checksum"] != md5hash)
                                {
                                    pluginChanges["MD5"].Add(plug.PluginName);
                                    PluginDetails = UpdateEntry(PluginDetails, plug, md5hash, file);
                                }

                            }
                        }


                    }
                }


            }


            



            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, SerializableDictionary<string,string>>));

            using (TextWriter textWriter = new StreamWriter("BUTTER-Plugin-Dat.xml"))
            {
                serializer.Serialize(textWriter, PluginDetails);

                textWriter.Close();
            }


            Console.WriteLine(Environment.NewLine + Environment.NewLine + "Changes: New Plugins");
                foreach (string item in pluginChanges["New"]) Console.WriteLine("\t" + item);
                if (pluginChanges["New"].Count == 0) Console.WriteLine("\tNone.");
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "Changes: Version Changes");
                foreach (string item in pluginChanges["Version"]) Console.WriteLine("\t" + item);
                if (pluginChanges["Version"].Count == 0) Console.WriteLine("\tNone.");
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "Changes: MD5");
                foreach (string item in pluginChanges["MD5"]) Console.WriteLine("\t" + item);
                if (pluginChanges["MD5"].Count == 0) Console.WriteLine("\tNone.");




        }



        static DateTimeOffset dto = new DateTimeOffset(1980, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0));

        private static void CleanFileMeta(string filename)
        {
            File.SetCreationTimeUtc(filename, dto.UtcDateTime);
            File.SetLastWriteTimeUtc(filename, dto.UtcDateTime);
            File.SetLastAccessTimeUtc(filename, dto.UtcDateTime);
        }



        private static void CompressFiles(Dictionary<string, List<string>> pluginFiles)
        {
            foreach (string pluginToCompress in pluginFiles.Keys)
            {
                using (FileStream zipToOpen = new FileStream(archiveDir + "/" + pluginToCompress, FileMode.CreateNew))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {

                        Console.WriteLine("\t" + pluginToCompress);
                        foreach (string fileToCompress in pluginFiles[pluginToCompress])
                        {

                            FileAttributes attr = File.GetAttributes(compiledDir + fileToCompress);
                            if (attr.HasFlag(FileAttributes.Directory))
                            {
                                List<string> subFiles = Directory.EnumerateFiles(compiledDir + fileToCompress, "*.*",
                                              SearchOption.AllDirectories).ToList();

                                foreach (string subFile in subFiles)
                                {
                                    CleanFileMeta(subFile);
                                    archive.CreateEntryFromFile(subFile, "Plugins/" + subFile.Replace(compiledDir, ""), CompressionLevel.Optimal);
                                }

                            }
                            else
                            {
                                CleanFileMeta(compiledDir + fileToCompress);
                                archive.CreateEntryFromFile(compiledDir + fileToCompress, "Plugins/" + fileToCompress, CompressionLevel.Optimal);

                            }

                        }

                    }
                }

                CleanFileMeta(archiveDir + "/" + pluginToCompress);

            }


            return;


        }




        static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }



        static SerializableDictionary<string, SerializableDictionary<string, string>> AddEntry(SerializableDictionary<string, SerializableDictionary<string, string>> PluginDetails,
            Plugin plug,
            string md5Hash,
            string file)
        {
            PluginDetails.Add(plug.PluginName, new SerializableDictionary<string, string>());
            PluginDetails[plug.PluginName].Add("File", file);
            PluginDetails[plug.PluginName].Add("Name", plug.PluginName);
            PluginDetails[plug.PluginName].Add("Version", plug.PluginVersion);
            //PluginDetails[plug.PluginName].Add("Type", plug.PluginType);
            //PluginDetails[plug.PluginName].Add("Description", plug.PluginDescription);
            PluginDetails[plug.PluginName].Add("MD5checksum", md5Hash);
            return PluginDetails;
        }

        static SerializableDictionary<string, SerializableDictionary<string, string>> UpdateEntry(SerializableDictionary<string, SerializableDictionary<string, string>> PluginDetails,
            Plugin plug,
            string md5Hash,
            string file)
        {

            PluginDetails[plug.PluginName]["File"] = file;
            PluginDetails[plug.PluginName]["Name"] = plug.PluginName;
            PluginDetails[plug.PluginName]["Version"] = plug.PluginVersion;
            //PluginDetails[plug.PluginName]["Type"] = plug.PluginType;
            //PluginDetails[plug.PluginName]["Description"] = plug.PluginDescription;
            PluginDetails[plug.PluginName]["MD5checksum"] = md5Hash;
            return PluginDetails;
        }














    }





}


