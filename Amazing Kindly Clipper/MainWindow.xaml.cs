using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Threading;

namespace ClippingManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml. This Window handles which file to use as a source for parsing, and
    /// lets the user select their language. It has several checks to prevent the user from using the wrong parser.
    /// </summary>

    public partial class MainWindow : Window
    {
        /// <summary> All parsers inherit from abstract class MyClippingsParserm and every inheriting parsers need to be instantiated prior to use 
        /// (due to the singleton pattern implementation only one instance of each parser can be instanced. At the moment only ENG and SPA parsers 
        /// are recognized and used, each one with various subtypes <seealso cref="FormatType"/> but the system should be easily extendable to other 
        /// subtypes and additional languages if needed.
        /// </summary>

        MyClippingsParserENG parserENG; 
        MyClippingsParserSPA parserSPA;
        MyClippingsParser parserToUse;
        FormatType formatToUse; 
        Encoding encoding; //Using UTF8 encoding by default here as defined in Options, but that can be changed.        

        private string textSample;  //Text sample only stores critical second line of text.
        private string textPreview; //Text preview gets up to n lines, as defined in var maxLineCounter. 
        private string defaultDirectory; //Variables to keep track of the directory in which the .txt are. 
        private string lastUsedDirectory;
        private string languageToDetect; //Additional language detection. 
        private int classwideRawCount; //Variable keeping count of raw clippings, declared on the class scope so that it can be used by several methods. 

        private LoadingWindow LW;

        public MainWindow()
        {
            parserENG = MyClippingsParserENG.MyParserENG; //Thread-safe, singleton instantiation of the two parsers implemented through the two subclasses. 
            parserSPA = MyClippingsParserSPA.MyParserSPA;

            FormatTypeDatabase.PopulateFormatList(parserENG.engFormats); 
            FormatTypeDatabase.PopulateFormatList(parserSPA.spaFormats);
            FormatTypeDatabase.GenerateFormatTypeDatabase(); //Methods generating a Dictionary of FormatTypes on execution. 

            parserToUse = null;
            formatToUse = null; //For debugging purposes you can manually change this to point to a given type, using parserInstance.Type. 

            encoding = Options.FileEncoding;

            defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            lastUsedDirectory = null;
            languageToDetect = "NotALanguage";

            InitializeComponent();
        }      

        private bool CheckParserLanguageAndType(MyClippingsParser parser, string sample, string preview)
        
        /// <summary> All parsers inherit from abstract class MyClippingsParser. Inheriting parsers need to be instantiated prior to use. At 
        /// the moment only ENG and SPA parsers are recognized and used, but the system should be easily extendable to other languages if needed.
        /// </summary>
     
        {
            try
            {
                if (Options.Language != null)
                {
                    string textSample = sample;  

                    List<string> engKeywords = new List<string>(); 
                    List<string> spaKeywords = new List<string>();                  

                    /* This lines hunt down an additional keywords (independent of the ones that will be 
                     * carried away later) to confirm language. */
                                  
                    string textPreview = preview;

                    PickFormatType(textPreview, languageToDetect);                

                    formatToUse = Options.FormatInUse;

                    //A last check that guarantees compatibility.

                    if ((languageToDetect == "Spanish") && (parserToUse == parserSPA) && (formatToUse != null))
                    {
                        return true;
                    }

                    if ((languageToDetect == "English") && (parserToUse == parserENG) && (formatToUse != null))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                else
                {
                    MessageBox.Show("Unable to find language. Have you selected your language?");
                    return false;
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Parser detection problem");
                return false;
            }
        }

        private void PickFormatType(string preview, string language)
        {
            /// <summary>
            /// FormatType detection that reads the .TXT preview (input is a text sample with a small number of clippings, separated by line jumps "\n"), 
            /// search for keywords, compare to formats in a dictionary and sets formatInUse accordingly. Note that said keywords are defined in each FormatType as 
            /// (a) Page, Location keyword arrays and (b) critical Keywords/Position/Language custom object. You get it once per .TXT file, and set the correct format 
            /// in Options, the same that will be later sent to Parser Line 2. Said format is standardized and its values taken from nice and neat parser types instances. 
            /// Parser in use is static, managed in options. Also note that both in Spanish and English formats there are two types defined by omission 
            /// (KeyValue <"Something", 1>) which signals base types (unsafe), and subtypes defined by the word in position 2, that are safe once recognized. 
            /// </summary>

            int maxLineCounter = preview.Split('\n').Length;

            using (var lineReader = new StringReader(preview))
            {
                lineReader.ReadLine(); //Skip first line, starts directly in line 1 where the critical keywords are.  

                string line = lineReader.ReadLine();
                var split = line.Split(' ');
                string keyWordPos1 = split[1];
                string keyWordPos2 = split[2];
                string detectedLanguage = language;
                FormatType format = null;

                FormatType.KeyPositionLang KeyPosition1 = new FormatType.KeyPositionLang(keyWordPos1, 1, language);
                FormatType.KeyPositionLang KeyPosition2 = new FormatType.KeyPositionLang(keyWordPos2, 2, language);
                FormatType.KeyPositionLang[] FormatKeyPosRead = new FormatType.KeyPositionLang[] {KeyPosition1, KeyPosition2};

                foreach (var KeyPos in FormatKeyPosRead)
                {
                    bool isSafe = false;
                    format = FormatTypeDatabase.GetFormat(KeyPos, out isSafe);

                    if (format != null)
                    {
                        if (!isSafe)
                        {
                            Options.FormatInUse = format;
                        }

                        if (isSafe)
                        {
                            Options.FormatInUse = format;
                            break;
                        }                       
                    }
                                
                    /* IMPORTANT: On its current state, the program just checks the second line and infers FormatType from there.
                     * Code here is easily modifiable so that in case of the first recognition try failing the second line of next clipping
                     * or successive lines are read. See use of line++, separator and Readline() in parser for inspiration.  */
                }
            }
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            /// <summary>
            /// Browsing folders to find formats, different options depending on current culture.  
            /// </summary>

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "TXT Files (*.txt)|*.txt";

            if (Options.CurrentCulture.Name == ("en-GB"))
            {
                ofd.FileName = "My Clippings - Kindle.txt"; // Default ENG file name
            }

            else
            {
                ofd.FileName = "Mis recortes.txt"; // Default SPA file name
            }

            if (String.IsNullOrEmpty(lastUsedDirectory))
            {
                ofd.InitialDirectory = defaultDirectory;
            }

            else
            {
                ofd.InitialDirectory = lastUsedDirectory;
            }


            if (ofd.ShowDialog() == true)
            {
                try
                {
                    string filePath = ofd.FileName;
                    string safeFilePath = ofd.SafeFileName;
                    textPreview = "";

                    var lineCounter = 0;
                    var maxLineCounter = 39; //A magic number here, number of lines that makes five clippings for most formats, but not necessarily. 
                    var languageDetectionLine = 1; //Line that contains the critical word used for language detection. 

                    /* Code generating a preview of the first lines of the document. Used for previewing purposes and for finding out language
                    of the file, using indicators in the second line*/

                    using (var reader = new StreamReader(ofd.FileName))
                    {
                        while (lineCounter < maxLineCounter)
                        {
                            string line = reader.ReadLine();

                            if (lineCounter == languageDetectionLine) //Critical line (usually second line) is a references to textSample to perform detection on it. 
                            {
                                textSample = line;
                            }

                            if (line == null)
                            {
                                break;
                            }
                            textPreview += line + " \n "; // Add line and jumps to a new line in preview.
                            lineCounter++;
                        }
                    }

                    try
                    {
                        if (textSample.Contains("Añadido"))
                        {
                            languageToDetect = "Spanish";
                            radioButtonB.IsChecked = true;
                        }

                        if (textSample.Contains("Added"))
                        {
                            languageToDetect = "English";
                            radioButtonA.IsChecked = true;
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Unable to complete language check.");
                    }

                    pathBox.Text = filePath; //Updates path in path textbox. 
                    lastUsedDirectory = filePath; //Remembers last used directory for user convenience.
                    filePreview.Text = textPreview; //Updates preview of the file in text block.

                    Options.TextToParsePath = filePath; //References preview in general text to parse. 
                    previewScroll.UpdateLayout();
                }

                catch (IOException)
                {
                    MessageBox.Show("Sorry, file is not valid.");
                }

            }
        }

        private async void buttonParse_Click(object sender, RoutedEventArgs e)
        {
            /// <summary>
            /// Parse button that kicks off the parsing process, carrying away a few compatibility test first. It checks for a general language configuration
            /// setup, then confirms compatibility format/language/FormatType, selects correct instances of parser and then, once all tests are passed, starts
            /// the process. 
            /// </summary>

            if (Options.TextToParsePath != null && Options.Language != null)
            {
                string path = Options.TextToParsePath;
                string language = Options.Language;
                bool correctParserConfirmed = false;

                switch (language)
                {
                    case "English":
                        parserToUse = parserENG;
                        break;
                    case "Spanish":
                        parserToUse = parserSPA;
                        break;
                }

                /* Checking .TXT language vs parser language and picking correct FormatType file. It offers the user some help to avoid exceptions  
                 * and allows new parsers to be added easily for full compatibility, even with custom or irregular .TXT files, on the dev side. */     
                                
                correctParserConfirmed = CheckParserLanguageAndType(parserToUse, textSample, textPreview);

                try
                {
                    if (correctParserConfirmed == false)
                    {
                        MessageBoxResult parsingProblemMessageBox = MessageBox.Show("Potential language incompatibilities detected. Are you sure you want to continue? \r\n \r\n Click 'Cancel' to go back and select the correct language (RECOMMENDED) or 'OK' to continue (WARNING: program might became inestable or crash.)", 
                            "Parsing problem?", System.Windows.MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
                        if (parsingProblemMessageBox == MessageBoxResult.OK)
                        {
                            correctParserConfirmed = true;
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Parsing problem");
                }

                if (correctParserConfirmed)
                {
                    parserToUse.Parse(path);

                    //Start the process (method), show pre-instantiated load window, wait for the task to finish and close the window. 

                    LW = new LoadingWindow();

                    await Task.Run(() => RunParser(path));                 

                    LW.CloseLoadingWindow();

                    MessageBox.Show(classwideRawCount + " clippings parsed.", "Parsing successful.");
                    var result = MessageBox.Show(ClippingDatabase.numberedClippings.Count.ToString() + " clippings added to database. " + 
                        (classwideRawCount - ClippingDatabase.numberedClippings.Count).ToString() + " empty or null clippings removed.", "Database created.");


                    Dispatcher.Invoke((Action)delegate () //If you want to update UI from this task a dispatcher has to be used, since it has to be in the UI thread.  
                    {
                        LaunchDatabaseWindow();
                    });          
                    
                }
            }

            if (Options.TextToParsePath == null)
            {
                MessageBox.Show("No path to .txt found, please select your Kindle clipping file and try again.");
            }

            if (Options.Language == null)
            {
                MessageBox.Show("Problems detecting language, please select your language and try again.");
            }
        }

        private void RunParser(string path)
        {
            string _path = path;

            try
            {
                var clippings = parserToUse.Parse(path);

                classwideRawCount = 0;
                foreach (var item in clippings)
                {
                    //Adding clippings to the currently used, dictionary database.
                    if (!Clipping.IsNullOrEmpty(item))
                    {
                        ClippingDatabase.AddClipping(item);
                    }
                    ++classwideRawCount;

                }

                //Now adding clippings to the layout'ed, list database. 
                int numberOfClippings = ClippingDatabase.numberedClippings.Count;

                for (int i = 0; i < numberOfClippings; i++)
                {
                    Clipping clippingToAdd = ClippingDatabase.GetClipping(i);
                    ClippingDatabase.finalClippingsList.Add(clippingToAdd);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Parsing Error");
            }
        }

        private void radioButtonA_Checked(object sender, RoutedEventArgs e)
        {
            Options.Language = "English";
            Options.CurrentCulture = Options.EngCulture;
        }

        private void radioButtonB_Checked(object sender, RoutedEventArgs e)
        {
            //radioButtonB.IsChecked = true; //Uncomment this option if you want English to be marked by default. 
            Options.Language = "Spanish";
            Options.CurrentCulture = Options.SpaCulture;
        }

        private void LaunchDatabaseWindow()
        {
            var databaseWindow = new DatabaseWindow { Owner = this };
            databaseWindow.ShowDialog();
        }
    }
}
