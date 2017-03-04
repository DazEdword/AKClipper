using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;


namespace AKCCore {

    //TODO Refactor in progress, many public methods around in this public class. Reorganise and change access 
    //levels later. 

    /// <summary>
    /// Parser Controller logic, used in MainWindow. Its concerns include to get and store which file to use as a source for parsing, 
    /// store the user selected language, initialise parsing process. It has several checks to prevent the user from using the wrong parser.
    /// All parsers inherit from abstract class MyClippingsParserm and every inheriting parsers need to be instantiated prior to use
    /// (due to the singleton pattern implementation only one instance of each parser can be instanced. At the moment only ENG and SPA parsers
    /// are recognized and used, each one with various subtypes <seealso cref="FormatType"/> but the system should be easily extendable to other
    /// subtypes and additional languages if needed.
    /// </summary>

    public class ParserController {
        private MyClippingsParserENG parserENG;
        private MyClippingsParserSPA parserSPA;
        public ParserOptions options;

        //Variable keeping count of raw clippings, declared on the class scope so that 
        //it can be used by several methods.
        public int rawClippingCount; 

        public ParserController() {
            parserENG = MyClippingsParserENG.MyParserENG; 
            parserSPA = MyClippingsParserSPA.MyParserSPA;
            options = new ParserOptions();

            //Methods generating a Dictionary of FormatTypes on instantiation.
            FormatTypeDatabase.PopulateFormatList(parserENG.engFormats);
            FormatTypeDatabase.PopulateFormatList(parserSPA.spaFormats);
            FormatTypeDatabase.GenerateFormatTypeDatabase();
        }

        public void SetParser(string id) {
            switch (id) {
                case "English":
                    options.SelectedParser = parserENG;
                    break;
                case "Spanish":
                    options.SelectedParser = parserSPA;
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Parser not recognised: unable to set parser");
                    break;
            }
        }

        //Instance type override, in case a parser instance is passed instead of a string with parser name
        public void SetParser(MyClippingsParser parser) {
            string t = parser.GetType().ToString();

            if (t == "MyClippingsParserENG") {
                options.SelectedParser = parserENG;
            } else if (t == "MyClippingsParserSPA") {
                options.SelectedParser = parserSPA;
            } else {
                System.Diagnostics.Debug.WriteLine("Parser instance not recognised: unable to set parser");
            }
        }
        
        //TODO This method passes a path to the file. Overload it to accept a text chain directly. 
        public void RunParser(string path) {
            try {
                var clippings = options.SelectedParser.Parse(path, options.SelectedFormat);

                rawClippingCount = 0;
                foreach (var item in clippings) {
                    //Adding clippings to the currently used, dictionary database.
                    if (!Clipping.IsNullOrEmpty(item)) {
                        ClippingDatabase.AddClipping(item);
                    }
                    ++rawClippingCount;
                }

                //Now adding clippings to the layout'ed, list database.
                int numberOfClippings = ClippingDatabase.numberedClippings.Count;

                for (int i = 0; i < numberOfClippings; i++) {
                    Clipping clippingToAdd = ClippingDatabase.GetClipping(i);
                    ClippingDatabase.finalClippingsList.Add(clippingToAdd);
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("Parsing Error: " + ex.Message);
            }
        }

        public dynamic ReportParsingResult(bool consoleOnly){
            dynamic result = new ExpandoObject();
            result.clippingCount = rawClippingCount;
            result.databaseEntries = ClippingDatabase.numberedClippings.Count;
            result.removedClippings = result.clippingCount - result.databaseEntries;

            if (consoleOnly) {
                //Perhaps we want to create a console project in the future to have a command-line AKC. 
                //But for today's debugging and testing this is more than enough.  
                System.Diagnostics.Debug.WriteLine(">> Parsed clippings: {0}", (object)result.clippingCount);
                System.Diagnostics.Debug.WriteLine(">> Parsing successful");
                System.Diagnostics.Debug.WriteLine(">> Removed clippings: {0}" ,(object)result.removedClippings.ToString());

                return null;

            } else {
                return result;
            }
        }

        public void PickFormatType(string preview, string language) {
            /// <summary>
            /// FormatType detection that reads the .TXT preview (input is a text sample with a small number of clippings, separated by 
            /// line jumps "\n"), search for keywords, compare to formats in a dictionary and sets formatInUse accordingly. Note that said 
            /// keywords are defined in each FormatType as (a) Page, Location keyword arrays and (b) critical Keywords/Position/Language custom 
            /// object. You get it once per .TXT file, and set the correct format in Options, the same that will be later sent to Parser Line 2.
            /// Said format is standardized and its values taken from nice and neat parser types instances. Parser in use is static, managed in 
            /// options. Also note that both in Spanish and English formats there are two types defined by omission (KeyValue <"Something", 1>) 
            /// which signals base types (unsafe), and subtypes defined by the word in position 2, that are safe once recognized.
            /// </summary>

            int maxLineCounter = preview.Split('\n').Length;

            using (var lineReader = new StringReader(preview)) {
                lineReader.ReadLine(); //Skip first line, starts directly in line 1 where the critical keywords are.

                string line = lineReader.ReadLine();
                string[] split = line.Split(' ');
                string keyWordPos1 = split[1];
                string keyWordPos2 = split[2];
                string detectedLanguage = language;
                FormatType format = null;

                FormatType.KeyPositionLang KeyPosition1 = new FormatType.KeyPositionLang(keyWordPos1, 1, language);
                FormatType.KeyPositionLang KeyPosition2 = new FormatType.KeyPositionLang(keyWordPos2, 2, language);
                FormatType.KeyPositionLang[] FormatKeyPosRead = new FormatType.KeyPositionLang[] { KeyPosition1, KeyPosition2 };

                foreach (var KeyPos in FormatKeyPosRead) {
                    bool isSafe = false;
                    format = FormatTypeDatabase.GetFormat(KeyPos, out isSafe);

                    if (format != null) {
                        if (!isSafe) {
                            options.SelectedFormat = format;
                        }

                        if (isSafe) {
                            options.SelectedFormat = format;
                            break;
                        }
                    }

                /* IMPORTANT: On its current state, the program just checks the second line and infers FormatType from 
                 * there. Code here is easily modifiable so that in case of the first recognition try failing the second
                 *  line of next clipping or successive lines are read. See use of line++, separator and Readline() in 
                 *  parser for inspiration.  */
                }
            }
        }

        public bool ConfirmParserCompatibility(string textSample, string textPreview) {
            string path = options.TextToParsePath;
            string language = options.Language;
            bool correctParserConfirmed = false;

            //Parser was added to options previously, is now set and then confirmed.
            SetParser(language);

            /* Checking .TXT language vs parser language and picking correct FormatType file. It offers the user some help to avoid exceptions
             * and allows new parsers to be added easily for full compatibility, even with custom or irregular .TXT files. */

            return correctParserConfirmed = CheckParserLanguageAndType(options.SelectedParser, textSample, textPreview);
        }

        public bool CheckParserLanguageAndType(MyClippingsParser parser, string sample, string preview) {

            /// <summary> All parsers inherit from abstract class MyClippingsParser. Inheriting parsers need to be 
            /// instantiated prior to use. At the moment only ENG and SPA parsers are recognized and used, but the 
            /// system should be easily extendable to other languages if needed.
            /// </summary>

            try {
               if (options.Language != null) {
                    string textSample = sample;
                    List<string> engKeywords = new List<string>();
                    List<string> spaKeywords = new List<string>();

                    /* This lines hunt down an additional keywords (independent of the ones that will be
                     * carried away later) to confirm language. */

                    PickFormatType(preview, options.Language);

                    //A last check that guarantees compatibility.
                    if ((options.Language == "Spanish") && (parser == parserSPA) && (options.SelectedFormat != null)) {
                        return true;
                    }

                    if ((options.Language == "English") && (parser == parserENG) && (options.SelectedFormat != null)) {
                        return true;
                    }
                    else {
                        return false;
                    }
                } else {
                    System.Diagnostics.Debug.WriteLine("Unable to find language. Have you selected your language?");
                    return false;
                }
            }

            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("Unable to find language. Have you selected your language?" + ex.Message);
                return false;
            }
        }
 
        public string GeneratePreviewFromPath(string path, int lines = 39) {
            //Second parameter is optional, change it if a bigger or smaller preview is needed.
            int currentLine = 0;
            string preview = "";

            //Min 4 lines(1 clipping)
            if (lines < 3) {
                lines = 3;
            }

            //EG March'2017 Changes here regarding streams, keep an eye if problems happen. 
            var stream = new FileStream(path, FileMode.Open);

            using (var reader = new StreamReader(stream)) {
                while (currentLine < lines) {
                    string line = reader.ReadLine();

                    if (line == null) {
                        break;
                    }
                    // Add line and jumps to a new line in preview.
                    preview += line + " \n "; 
                    currentLine++;
                }
            }
            return preview;
        }

        public void RunParsingSequence(){
            /// <summary>
            /// Method running the whole parsing process, carrying away a few compatibility test first and
            /// running the parser only if check results are OK. It checks for a general language configuration
            /// setup, then confirms compatibility format/language/FormatType, selects correct instances of 
            /// parser and only then starts with parsing itself.
            /// </summary>
        }
    }
}
