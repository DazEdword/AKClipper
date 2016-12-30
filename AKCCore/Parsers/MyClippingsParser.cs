using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AKCCore {

    public abstract class MyClippingsParser {
        /// <summary>
        /// Base parser, all the other parsers should inherit from it. It's pretty fail safe, with every different variable of the clipping braced
        /// in try/catch statements, so that unrecognized fields can be assigned with a default value instead of throwing an exception. Empty fields
        /// are expected to be added, with default variables just used in the case of exceptions or unrecognised content.
        /// </summary>

        //String matching-related variables.

        private const string ClippingSeparator = "==========";
        private const string Line1RegexPattern = @"^(.+?)(?: \(([^)]+?)\))?$";


        //TODO Modify parser so as to have them receive format type and anything else important to them that is in options.
        //Parsersshould not be interested on options or not even in ParserController, but ParserController interested on parsers

        internal Clipping Defaults;
        internal ParserOptions options;


        //TODO Maybe in the process of changing and adapting this method to increase flexibility we should 
        //implement an async approach that tracks progress
        //http://stackoverflow.com/questions/19980112/how-to-do-progress-reporting-using-async-await


        public virtual IEnumerable<Clipping> Parse(string path, FormatType format) {
            return ParseStreamFromPath(path, format);
        }


        /*Calling to the different methods parsing the different lines. Line 3 is irrelevant 
         * (just white space acting as a separator) and thus is not included in the logic. */

        protected virtual IEnumerable<Clipping> ParseStreamFromPath(string path, FormatType format) {
            //Open stream via path to the .txt file.
            var stream = new FileStream(path, FileMode.Open);
            using (var sr = new StreamReader(stream)) {
                int lineNumber = 0;
                string line = null;
                int clippingLineNumber = 0;
                Clipping clipping = new Clipping();

                while ((line = sr.ReadLine()) != null) {
                    lineNumber++;

                    if (line == ClippingSeparator) {
                        yield return clipping;
                        clippingLineNumber = 0;
                        clipping = new Clipping();
                    } else {
                        clippingLineNumber++;
                    }

                    ParseClipping(clippingLineNumber, line, clipping, format);
                }
            }
        }

        //TODO: Ready to implement ParseFromString.

        /*
        protected virtual IEnumerable<Clipping> ParseFromString(string content, FormatType format) {
        } */

        public virtual void ParseClipping(int lineNumber, string line, Clipping clipping, FormatType format) {
            try {
                switch (lineNumber) {
                    case 1:
                        ParseLine1(line, clipping);
                        break;
                    case 2:
                        ParseLine2(line, clipping, format);
                        break;
                    case 4:
                        ParseLine4(line, clipping);
                        break;
                }
            } catch (Exception ex) {
                new Exception("Error encountered parsing line " + lineNumber + ": " + ex.Message, ex);
            }
        }

        protected virtual void ParseLine1(string line, Clipping clipping) {
            try {
                var match = Regex.Match(line, Line1RegexPattern);
                if (match.Success) {
                    var bookName = match.Groups[1].Value.Trim();
                    var author = match.Groups[2].Value.Trim();

                    clipping.BookName = bookName;
                    clipping.Author = author;
                }
            }
            catch (Exception) {
                clipping.BookName = Defaults.BookName;
                clipping.Author = Defaults.Author;
                Console.WriteLine("Clipping Line 1 did not match regex pattern, using default values for Author and Bookname.");
            }
        }

        protected abstract void ParseLine2(string line, Clipping clipping, FormatType format);

        protected virtual void ParseLine4(string line, Clipping clipping) {
            try {
                clipping.Text = line.Trim();
            }
            catch (Exception) {
                clipping.Text = Defaults.Text;
            }
        }
    }
}