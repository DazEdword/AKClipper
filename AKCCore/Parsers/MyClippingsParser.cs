using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AKCCore {

    public abstract class MyClippingsParser {
        /// <summary>
        /// Base parser, all the other parsers should inherit from it. It's pretty fail safe, with every different variable of the clipping braced
        /// in try/catch statements, so that unrecognized fields can be assigned with a default value instead of throwing an exception. Empty fields
        /// are expected to be added, with default variables just used in the case of exceptions.
        /// </summary>

        //String matching-related variables.

        private const string ClippingSeparator = "==========";
        private const string Line1RegexPattern = @"^(.+?)(?: \(([^)]+?)\))?$";

        
        //TODO Modify parser so as to have them receive format type and anything else important to them that is in options,
        //they should not be interested on options or not even in ParserController, but ParserController interested on parsers
        //Clipping type defaults variables.
        internal string defaultBookName;
        internal string defaultAuthor;
        internal ClippingTypeEnum defaultClippingType;
        internal string defaultPage;
        internal string defaultLocation;
        internal DateTime defaultDateAdded;
        internal string defaultText;
        internal ParserOptions options;


        //TODO Overload this to accept text directly, thus skipping the reading part (which should be isolated btw)
        //Directly parse the stream matching the format of the .txt file.
        //It might help to differentiate path pathToContent/string Content 
        // https://msdn.microsoft.com/en-us/library/system.io.path(v=vs.110).aspx
        public virtual IEnumerable<Clipping> Parse(string path, FormatType format) {
            var stream = new FileStream(path, FileMode.Open); //Open stream via path to the .txt file.
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
                    }
                    else {
                        clippingLineNumber++;
                    }

                    /*Calling to the different methods parsing the different lines.
                    Line 3 is irrelevant (just white space acting as a separator) and thus is not included
                    in the logic. */

                    try {
                        switch (clippingLineNumber) {
                            case 1:
                                ParseLine1(line, clipping);
                                break;

                            case 2:
                                ParseLine2(line, clipping, format); //Modified so as to include FileType too
                                break;

                            case 4:
                                ParseLine4(line, clipping);
                                break;
                        }
                    }
                    catch (Exception ex) {
                        new Exception("Error encountered parsing line " + lineNumber + ": " + ex.Message, ex);
                    }
                }
            }
        }

        public virtual void ParseLine1(string line, Clipping clipping) {
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
                clipping.BookName = defaultBookName;
                clipping.Author = defaultAuthor;
                Console.WriteLine("Clipping Line 1 did not match regex pattern, using default values for Author and Bookname.");
            }
        }

        public abstract void ParseLine2(string line, Clipping clipping, FormatType format);

        public virtual void ParseLine4(string line, Clipping clipping) {
            try {
                clipping.Text = line.Trim();
            }
            catch (Exception) {
                clipping.Text = defaultText;
            }
        }
    }
}