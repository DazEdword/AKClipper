using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

        internal Clipping Defaults;

        //TODO Maybe in the process of changing and adapting this method to increase flexibility we should 
        //implement an async approach that tracks progress
        //http://stackoverflow.com/questions/19980112/how-to-do-progress-reporting-using-async-await


        //Public interfaces, Parse ("default", from path) or DirectParse(string content).
        //DirectParse can be tentatively used as an easy interface for the test framework.
        public virtual IEnumerable<Clipping> Parse(string path, FormatType format) {
            return ParseStreamFromPath(path, format);
        }

        public virtual IEnumerable<Clipping> DirectParse(string content, FormatType format) {
            return ParseFromString(content, format);
        }

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

        protected virtual IEnumerable<Clipping> ParseFromString(string content, FormatType format) {
            Clipping clipping = new Clipping();

            //TODO Hardcoded line break to solve?
            string[] result = content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
            string lines = null;

            int lineNumber = 0;
            int clippingLineNumber = 0;

            while (lineNumber < result.Length) {

                lineNumber++;
                clippingLineNumber++;

                if (result[lineNumber-1] == ClippingSeparator) {
                    yield return clipping;
                    clippingLineNumber = 0;
                    clipping = new Clipping();
                } else {
                    //TODO Simplify this foul smelling conditional logic
                    if (!(clippingLineNumber == 1 || clippingLineNumber == 2 || clippingLineNumber == 3) && result[lineNumber] != ClippingSeparator) {
                        lines += result[lineNumber - 1];
                    } else if (clippingLineNumber == 3) {
                        lines = null;
                    } else {
                        if (lines == null) {
                            lines = result[lineNumber - 1];
                        }
                        ParseClipping(clippingLineNumber, lines, clipping, format);
                        lines = null;
                    }
                }
            }
        }


        /* Calling to the different methods parsing the different lines. Line 3 is irrelevant 
        * (just white space acting as a separator) and thus is not included in the logic. */

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
                Match match = Regex.Match(line, Line1RegexPattern);
                if (match.Success) {
                    string bookName = match.Groups[1].Value.Trim();
                    string author = match.Groups[2].Value.Trim();

                    clipping.BookName = !string.IsNullOrEmpty(bookName) ? bookName : Defaults.BookName;
                    clipping.Author = !string.IsNullOrEmpty(author) ? author : Defaults.Author;
                }
            }
            catch (Exception) {
                clipping.BookName = Defaults.BookName;
                clipping.Author = Defaults.Author;
                System.Diagnostics.Debug.WriteLine("Clipping Line 1 did not match regex pattern, using default values for Author and Bookname.");
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

        //TODO Too many parameters, simplify
        protected virtual void ParseDateExact(string[] splitLine, Clipping clipping, int dateIndex, 
            string[] dateFormats, CultureInfo culture) {
            //Formatting help: https://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx
            
            string dateAdded = String.Join(" ", splitLine[dateIndex], splitLine[dateIndex + 1], 
                splitLine[dateIndex + 3], splitLine[dateIndex + 5], splitLine[dateIndex + 6]);
            //Removing single quotes. There might be other "noise" characters, this one is especially important in typeRub.
            string input = dateAdded.Replace("'", string.Empty);

            try {
                /*Dates have to be parsed and converted to a dateTime format. TryParseExact should do the 
                 * trick as long as the proper format is added to the dateFormats array.  */
                DateTime dt;
                if (DateTime.TryParseExact(input, dateFormats, culture, DateTimeStyles.None, out dt)) {
                    if (dt < DateTime.Now) {
                        clipping.DateAdded = dt;
                    }
                }
            }
            catch (Exception ex) {
                clipping.DateAdded = Defaults.DateAdded;
                new Exception("Error encountered adding date: " + ex.Message, ex);
            }
        }

        protected virtual void ParseDate(string[] splitLine, Clipping clipping, int dateIndex) {
            try {
                string[] filteredLine = splitLine.Where(item => !item.Contains("GMT")).ToArray();
                string dateAddedString;

                //Hackish, removing GMT to simplify parse. Indexes change and problems. Can be improved.
                if (splitLine.Length != filteredLine.Length) {
                    dateAddedString = String.Join(" ", splitLine[dateIndex], splitLine[dateIndex + 1], splitLine[dateIndex + 2], splitLine[dateIndex + 3], splitLine[dateIndex + 4]);
                } else {
                    dateAddedString = String.Join(" ", splitLine[dateIndex], splitLine[dateIndex + 1], splitLine[dateIndex + 2], splitLine[dateIndex + 3], splitLine[dateIndex + 4], splitLine[dateIndex + 5]);
                }
           
                DateTime dateAdded = DateTime.Parse(dateAddedString);
                clipping.DateAdded = dateAdded;
            } catch (Exception ex) {
                clipping.DateAdded = Defaults.DateAdded;
                new Exception("Error encountered adding date: " + ex.Message, ex);
            }
        }

        protected virtual void InitDefaults() {

        }

        protected virtual void InitFormats() {

        }
    }
}