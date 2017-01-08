using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AKCCore;

namespace AKCWPF {
    /// <summary>
    /// Interaction logic for MainWindow.xaml. This window lets the user select their clipping file language,
    /// providing the GUI to fire off the parserController class, which handles parsing. 
    /// </summary>

    public partial class MainWindow : Window {

        private ParserController parserController;
        private Encoding encoding; //Using UTF8 encoding by default here as defined in OptionsDeprecate, but that can be changed.
        private string textSample;
        private string textPreview; //Text preview gets up to n lines, as defined in var maxLineCounter.
        private string defaultDirectory; //Variables to keep track of the directory in which the .txt are.
        private string lastUsedDirectory;

        private LoadingWindow LW;

        public MainWindow() {
            parserController = new ParserController();

            //GUI simple options and persistence
            encoding = parserController.options.FileEncoding;
            defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            lastUsedDirectory = null;
            parserController.options.Language = "NotALanguage";

            InitializeComponent();
        }

        private void BrowseFile() {
            /// <summary>
            /// Browsing folders to find formats, different options depending on current culture.
            /// </summary>

            // A) Fire off OFD, configure depending on culture.
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt";
            ofd.Filter = "TXT Files (*.txt)|*.txt";

            //Check culture, set up default file names accordingly. 
            if (parserController.options.SelectedCulture.Name == ("en-GB")) {
                ofd.FileName = "My Clippings - Kindle.txt"; 
            } else {
                ofd.FileName = "Mis recortes.txt"; 
            }

            //Get initial directory. 
            if (String.IsNullOrEmpty(lastUsedDirectory)) {
                ofd.InitialDirectory = defaultDirectory;
            } else {
                ofd.InitialDirectory = lastUsedDirectory;
            }

            if (ofd.ShowDialog() == true) {
                try {
                    string filePath = ofd.FileName;
                    string safeFilePath = ofd.SafeFileName;

                    textPreview = parserController.GeneratePreviewFromPath(filePath);
                    //Get critical line from textPreview, hardcoded here as first line. 
                    //Safe, since min. number lines is 4
                    textSample = textPreview.Replace("\r", "").Split('\n')[1];

                    //TODO if we add translation support in a future, this needs to be better.
                    try {
                        if (textSample.Contains("Añadido")) {
                            parserController.options.Language = "Spanish";
                            radioButtonB.IsChecked = true;
                        }

                        if (textSample.Contains("Added")) {
                            parserController.options.Language = "English";
                            radioButtonA.IsChecked = true;
                        }
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message, "Unable to complete language check.");
                    }

                    pathBox.Text = filePath; //Updates path in path textbox.
                    lastUsedDirectory = filePath; //Remembers last used directory for user convenience.
                    filePreview.Text = textPreview; //Updates preview of the file in text block.

                    parserController.options.TextToParsePath = filePath; //References preview in general text to parse.
                    previewScroll.UpdateLayout();

                } catch (IOException) {
                    MessageBox.Show("Sorry, file is not valid.");
                }
            }
        }
        
        private void browseButton_Click(object sender, RoutedEventArgs e) {
            BrowseFile();
        }


        private async void Parse() {
            if (parserController.options.TextToParsePath != null && parserController.options.Language != null) {

                bool correctParserConfirmed = parserController.ConfirmParserCompatibility(textSample, textPreview);

                try {
                    if (correctParserConfirmed == false) {
                        MessageBoxResult parsingProblemMessageBox = MessageBox.Show("Potential language incompatibilities detected. Are you sure you want to continue? \r\n \r\n Click 'Cancel' to go back and select the correct language (RECOMMENDED) or 'OK' to continue (WARNING: program might became inestable or crash.)",
                            "Parsing problem?", System.Windows.MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
                        if (parsingProblemMessageBox == MessageBoxResult.OK) {
                            correctParserConfirmed = true;
                        }
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Parsing problem");
                }

                if (correctParserConfirmed) {
                    /*
                    //TEST
                    var test = parserController.options.SelectedParser.DirectParse(
                        "A Dance With Dragons: Book 5 of A Song of Ice and Fire(Song of Ice & Fire 5)(Martin, George R.R.)\r\n- Mi subrayado en la página 41 | Posición 620 - 620 | Añadido el domingo 2 de septiembre de 2012, 23:54:20\r\n \r\n droll?”\r\n==========\r\nLa metamorfosis (Spanish Edition) (Kafka, Franz)\r\n- Mi subrayado Posición 13 - 13 | Añadido el jueves 18 de octubre de 2012, 19:18:28\r\ndespués\r\nhabía\r\nmonos\r\n \r\n==========\r\nA Dance With Dragons: Book 5 of A Song of Ice and Fire(Song of Ice & Fire 5)(Martin, George R.R.)\r\n- Mi marcador en la página 442 | Posición 6769 | Añadido el miércoles 28 de noviembre de 2012, 14:08:48\r\n \r\n==========",
                         parserController.options.SelectedFormat
                        );

                    var myResult = test.ToList();
                    */

                    //Async parsing
                    LW = new LoadingWindow();
                    await Task.Run(() => parserController.RunParser(parserController.options.TextToParsePath));
                    LW.CloseLoadingWindow();

                    //Result generation
                    dynamic result = parserController.ReportParsingResult(false);

                    if (result != null) {
                        MessageBox.Show(result.clippingCount + " clippings parsed.", "Parsing successful.");
                        MessageBox.Show(result.databaseEntries.ToString() + " clippings added to database. " +
                            result.removedClippings.ToString() + " empty or null clippings removed.", "Database created.");
                    }

                    //If you want to update UI from this task a dispatcher has to be used, since it has to be in the UI thread.
                    Dispatcher.Invoke((Action)delegate () {
                        LaunchDatabaseWindow();
                    });
                }
            }

            if (parserController.options.TextToParsePath == null) {
                MessageBox.Show("No path to .txt found, please select your Kindle clipping file and try again.");
            }

            if (parserController.options.Language == null) {
                MessageBox.Show("Problems detecting language, please select your language and try again.");
            }
        }

        private void buttonParse_Click(object sender, RoutedEventArgs e) {
            Parse();
        }
            

        private void radioButtonA_Checked(object sender, RoutedEventArgs e) {
            parserController.options.Language = "English";
            parserController.options.SelectedCulture = parserController.options.EngCulture;
        }

        private void radioButtonB_Checked(object sender, RoutedEventArgs e) {
            //radioButtonB.IsChecked = true;
            parserController.options.Language = "Spanish";
            parserController.options.SelectedCulture = parserController.options.SpaCulture;
        }

        private void LaunchDatabaseWindow() {
            var databaseWindow = new DatabaseWindow { Owner = this };
            databaseWindow.ShowDialog();
        }
    }
}