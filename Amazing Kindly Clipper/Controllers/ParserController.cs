using System;
using System.IO;
using System.Windows;

namespace ClippingManager {

    //TODO Refactor in progress, many public methods around in this public class. Reorganise later. 

    /// <summary>
    /// Parser Controller logic, used in MainWindow. It's concerns include to get and store which file to use as a source for parsing, 
    /// store the user selected language, initialise parsing process. It has several checks to prevent the user from using the wrong parser.
    /// </summary>
    public class ParserController {

        public MyClippingsParserENG parserENG;
        public MyClippingsParserSPA parserSPA;
        public MyClippingsParser setParser;
        public FormatType setFormat;

        //private Encoding encoding; //Using UTF8 encoding by default here as defined in Options, but that can be changed.

        //private string textSample;  //Text sample only stores critical second line of text.
        //private string textPreview; //Text preview gets up to n lines, as defined in var maxLineCounter.
        //private string defaultDirectory; //Variables to keep track of the directory in which the .txt are.
        //private string lastUsedDirectory;
        //private string languageToDetect; //Additional language detection.
        //private int classwideRawCount; //Variable keeping count of raw clippings, declared on the class scope so that it can be used by several methods.

        public ParserController() {
            parserENG = MyClippingsParserENG.MyParserENG; //Thread-safe, singleton instantiation of the two parsers implemented through the two subclasses.
            parserSPA = MyClippingsParserSPA.MyParserSPA;
        }

        //TODO This method passes a string. Overload to pass any other? Parser instance?

        public void SetParser(string language) {
            //TODO Maybe replace this switch with something a bit more elegant?
            //TODO Improve defaulting and error handling here. What if the string is not in the switch?
            switch (language) {
                case "English":
                    setParser = parserENG;
                    break;

                case "Spanish":
                    setParser = parserSPA;
                    break;
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
                            Options.FormatInUse = format;
                        }

                        if (isSafe) {
                            Options.FormatInUse = format;
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

 

    }
}
