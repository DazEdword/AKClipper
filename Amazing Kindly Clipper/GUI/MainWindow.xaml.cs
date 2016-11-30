using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ClippingManager {
    /// <summary>
    /// Interaction logic for MainWindow.xaml. This Window handles which file to use as a source for parsing, and
    /// lets the user select their language. It has several checks to prevent the user from using the wrong parser.
    /// </summary>

    public partial class MainWindow : Window {
        /// <summary> All parsers inherit from abstract class MyClippingsParserm and every inheriting parsers need to be instantiated prior to use
        /// (due to the singleton pattern implementation only one instance of each parser can be instanced. At the moment only ENG and SPA parsers
        /// are recognized and used, each one with various subtypes <seealso cref="FormatType"/> but the system should be easily extendable to other
        /// subtypes and additional languages if needed.
        /// </summary>

        private MyClippingsParserENG parserENG;
        private MyClippingsParserSPA parserSPA;
        private MyClippingsParser setParser;
        private FormatType setFormat;
        private ParserController parserController;
        private Encoding encoding; //Using UTF8 encoding by default here as defined in Options, but that can be changed.

        private string textSample;  //Text sample only stores critical second line of text.
        private string textPreview; //Text preview gets up to n lines, as defined in var maxLineCounter.
        private string defaultDirectory; //Variables to keep track of the directory in which the .txt are.
        private string lastUsedDirectory;
        private int classwideRawCount; //Variable keeping count of raw clippings, declared on the class scope so that it can be used by several methods.

        private LoadingWindow LW;

        public MainWindow() {
            parserController = new ParserController();
            //TODO Deprecate after refactoring
            parserENG = parserController.parserENG;
            parserSPA = parserController.parserSPA; 

            FormatTypeDatabase.PopulateFormatList(parserENG.engFormats);
            FormatTypeDatabase.PopulateFormatList(parserSPA.spaFormats);
            FormatTypeDatabase.GenerateFormatTypeDatabase(); //Methods generating a Dictionary of FormatTypes on execution.

            setParser = parserController.setParser;
            setFormat = parserController.setFormat; //For debugging purposes you can manually change this to point to a given type, using parserInstance.Type.

            encoding = Options.FileEncoding;

            defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            lastUsedDirectory = null;
            parserController.languageToDetect = "NotALanguage";

            InitializeComponent();
        }

        //TODO OMG, THE HORROR. Extract many methods here, separate UI from logic. Cry.
        private void browseButton_Click(object sender, RoutedEventArgs e) {
            /// <summary>
            /// Browsing folders to find formats, different options depending on current culture.
            /// </summary>

            //A) Fire off OFD, configure depending on culture.
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "TXT Files (*.txt)|*.txt";

            if (Options.CurrentCulture.Name == ("en-GB")) {
                ofd.FileName = "My Clippings - Kindle.txt"; // Default ENG file name
            }
            else {
                ofd.FileName = "Mis recortes.txt"; // Default SPA file name
            }

            if (String.IsNullOrEmpty(lastUsedDirectory)) {
                ofd.InitialDirectory = defaultDirectory;
            }
            else {
                ofd.InitialDirectory = lastUsedDirectory;
            }

            if (ofd.ShowDialog() == true) {
                try {
                    string filePath = ofd.FileName;
                    string safeFilePath = ofd.SafeFileName;
                    textPreview = "";

                    var lineCounter = 0;
                    var maxLineCounter = 39; //A magic number here, number of lines that makes five clippings for most formats, but not necessarily.
                    var languageDetectionLine = 1; //Line that contains the critical word used for language detection.

                    /* Code generating a preview of the first lines of the document. Used for previewing purposes and for finding out language
                    of the file, using indicators in the second line*/

                    using (var reader = new StreamReader(ofd.FileName)) {
                        while (lineCounter < maxLineCounter) {
                            string line = reader.ReadLine();

                            if (lineCounter == languageDetectionLine) //Critical line (usually second line) is a references to textSample to perform detection on it.
                            {
                                textSample = line;
                            }

                            if (line == null) {
                                break;
                            }
                            textPreview += line + " \n "; // Add line and jumps to a new line in preview.
                            lineCounter++;
                        }
                    }

                    try {
                        if (textSample.Contains("Añadido")) {
                            parserController.languageToDetect = "Spanish";
                            radioButtonB.IsChecked = true;
                        }

                        if (textSample.Contains("Added")) {
                            parserController.languageToDetect = "English";
                            radioButtonA.IsChecked = true;
                        }
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message, "Unable to complete language check.");
                    }

                    pathBox.Text = filePath; //Updates path in path textbox.
                    lastUsedDirectory = filePath; //Remembers last used directory for user convenience.
                    filePreview.Text = textPreview; //Updates preview of the file in text block.

                    Options.TextToParsePath = filePath; //References preview in general text to parse.
                    previewScroll.UpdateLayout();
                }
                catch (IOException) {
                    MessageBox.Show("Sorry, file is not valid.");
                }
            }
        }

        //TODO refactor: Abstract browsing and checking logic, separate from parse button logic. 
        /* What is this guy really doing? 
         *  Are the option controls checked?
         *  Confirm parser
         *  Launch parser
         *  Launch loading window
         *  Do parse shit
         *  Close loading window
         *  Launch database window
         */
           
        private async void buttonParse_Click(object sender, RoutedEventArgs e) {
            /// <summary>
            /// Parse button that kicks off the parsing process, carrying away a few compatibility test first. It checks for a general language configuration
            /// setup, then confirms compatibility format/language/FormatType, selects correct instances of parser and then, once all tests are passed, starts
            /// the process.
            /// </summary>

            if (Options.TextToParsePath != null && Options.Language != null) {

                string path = Options.TextToParsePath;
                string language = Options.Language;
                bool correctParserConfirmed = false;

                parserController.SetParser(language);
                setParser = parserController.setParser;

                /* Checking .TXT language vs parser language and picking correct FormatType file. It offers the user some help to avoid exceptions
                 * and allows new parsers to be added easily for full compatibility, even with custom or irregular .TXT files, on the dev side. */

                correctParserConfirmed = parserController.CheckParserLanguageAndType(setParser, textSample, textPreview);
                //correctParserConfirmed = CheckParserLanguageAndType(setParser, textSample, textPreview);

                try {
                    if (correctParserConfirmed == false) {
                        MessageBoxResult parsingProblemMessageBox = MessageBox.Show("Potential language incompatibilities detected. Are you sure you want to continue? \r\n \r\n Click 'Cancel' to go back and select the correct language (RECOMMENDED) or 'OK' to continue (WARNING: program might became inestable or crash.)",
                            "Parsing problem?", System.Windows.MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
                        if (parsingProblemMessageBox == MessageBoxResult.OK) {
                            correctParserConfirmed = true;
                        }
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Parsing problem");
                }

                if (correctParserConfirmed) {
                    setParser.Parse(path);


                    //Start the process (method), show pre-instantiated load window, wait for the task to finish and close the window.
                    //TODO This is nuts, change the way the window is handled completely. 

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

            if (Options.TextToParsePath == null) {
                MessageBox.Show("No path to .txt found, please select your Kindle clipping file and try again.");
            }

            if (Options.Language == null) {
                MessageBox.Show("Problems detecting language, please select your language and try again.");
            }
        }

        private void RunParser(string path) {

            try {
                var clippings = setParser.Parse(path);

                classwideRawCount = 0;
                foreach (var item in clippings) {
                    //Adding clippings to the currently used, dictionary database.
                    if (!Clipping.IsNullOrEmpty(item)) {
                        ClippingDatabase.AddClipping(item);
                    }
                    ++classwideRawCount;
                }

                //Now adding clippings to the layout'ed, list database.
                int numberOfClippings = ClippingDatabase.numberedClippings.Count;

                for (int i = 0; i < numberOfClippings; i++) {
                    Clipping clippingToAdd = ClippingDatabase.GetClipping(i);
                    ClippingDatabase.finalClippingsList.Add(clippingToAdd);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Parsing Error");
            }
        }

        private void radioButtonA_Checked(object sender, RoutedEventArgs e) {
            Options.Language = "English";
            Options.CurrentCulture = Options.EngCulture;
        }

        private void radioButtonB_Checked(object sender, RoutedEventArgs e) {
            //radioButtonB.IsChecked = true; //Uncomment this option if you want English to be marked by default.
            Options.Language = "Spanish";
            Options.CurrentCulture = Options.SpaCulture;
        }

        private void LaunchDatabaseWindow() {
            var databaseWindow = new DatabaseWindow { Owner = this };
            databaseWindow.ShowDialog();
        }
    }
}