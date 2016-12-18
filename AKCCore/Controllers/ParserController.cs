using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Windows;

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

        //Hmmmm... bad smell
        //public string textSample;  //Text sample only stores critical second line of text.
        //public string textPreview; //Text preview gets up to n lines, as defined in var maxLineCounter.

        //TODO Temporary var for refactor. 
        public string path;
        public string languageToDetect; //Additional language detection.
        //Variable keeping count of raw clippings, declared on the class scope so that it can be used by several methods.
        public int rawClippingCount; 

        public ParserController() {
            parserENG = MyClippingsParserENG.MyParserENG; 
            parserSPA = MyClippingsParserSPA.MyParserSPA;
            path = ""; //TEMP
            options = new ParserOptions();

            //Methods generating a Dictionary of FormatTypes on instantiation.
            FormatTypeDatabase.PopulateFormatList(parserENG.engFormats);
            FormatTypeDatabase.PopulateFormatList(parserSPA.spaFormats);
            FormatTypeDatabase.GenerateFormatTypeDatabase();
        }

        public void SetParser(string language) {
            switch (language) {
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

        //TODO This method passes a path to the file. Modify it to accept a text chain directly. 
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
                MessageBox.Show(ex.Message, "Parsing Error");
            }
        }

        public dynamic ReportParsingResult(bool consoleOnly){
            dynamic result = new ExpandoObject();
            result.clippingCount = rawClippingCount;
            result.databaseEntries = ClippingDatabase.numberedClippings.Count;
            result.removedClippings = result.clippingCount - result.databaseEntries;
            if (consoleOnly) {
                //TODO Write console interface (console project) to use this properly as a secondary 
                //interface. For now, debug will do. 
                System.Diagnostics.Debug.WriteLine(">> Parsed clippings: {0}", (object)result.clippingCount);
                System.Diagnostics.Debug.WriteLine(">> Parsing successful");
                System.Diagnostics.Debug.WriteLine(">> Removed clippings: {0}" ,(object)result.removedClippings.ToString());

                //Console.WriteLine(">> " + result.clippingCount + " clippings parsed.");
                //Console.WriteLine(">> Parsing successfull");
                //Console.WriteLine(">> " + result.removedClippings.ToString() + 
                //    " empty or null clippings removed weren't added to database.");

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

        public bool CheckParserLanguageAndType(MyClippingsParser parser, string sample, string preview) {

            /// <summary> All parsers inherit from abstract class MyClippingsParser. Inheriting parsers need to be 
            /// instantiated prior to use. At the moment only ENG and SPA parsers are recognized and used, but the 
            /// system should be easily extendable to other languages if needed.
            /// </summary>
            /// 

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
                    MessageBox.Show("Unable to find language. Have you selected your language?");
                    return false;
                }
            }

            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Parser detection problem");
                return false;
            }
        }

        public bool ConfirmParserCompatibility(string textSample, string textPreview) {
            //TODO method is very dependant of options, are we sure of this?
            string path = options.TextToParsePath;
            string language = options.Language;
            bool correctParserConfirmed = false;
            //TODO setting parser just before confirmation? just doesn't feel right anymore
            SetParser(language);

            /* Checking .TXT language vs parser language and picking correct FormatType file. It offers the user some help to avoid exceptions
             * and allows new parsers to be added easily for full compatibility, even with custom or irregular .TXT files, on the dev side. */

            return correctParserConfirmed = CheckParserLanguageAndType(options.SelectedParser, textSample, textPreview);
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
