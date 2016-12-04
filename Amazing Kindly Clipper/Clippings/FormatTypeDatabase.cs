using System.Collections.Generic;

namespace ClippingManager {

    public static class FormatTypeDatabase {
        public static List<FormatType> formatTypeList = new List<FormatType>();
        public static Dictionary<FormatType.KeyPositionLang, FormatType> FormatDictionary = new Dictionary<FormatType.KeyPositionLang, FormatType>();
        public static bool isLastFormatSafeMatch = false;

        /// <summary>
        /// This static class populates and returns formatTypes from a database. A format list is populated when the program launches
        /// by the instances of each parser (one per language), which creates formatTypes with a set of properties. After that, the
        /// GenerateFormatTypeDatabase() method creates a Dictionary KeyPosition/FormatTypes Key/Value pairs, iterating
        /// through the List and adding a number of entries to the dictionary. Single formats can be added manually using AddFormat().
        /// </summary>
        /// <param name="formatType">The formatType to add to the database</param>

        public static void PopulateFormatList(FormatType[] formatLanguagePack) {
            foreach (FormatType formatInstance in formatLanguagePack) {
                formatTypeList.Add(formatInstance);
            }
        }

        public static void GenerateFormatTypeDatabase() {
            //Reset
            FormatDictionary.Clear();
            //Repopulation from
            foreach (FormatType formatType in formatTypeList) {
                foreach (var keypair in formatType.keyPositions) {
                    FormatDictionary.Add(keypair, formatType);
                }
            }
        }

        public static void AddFormat(FormatType formatInstance) {
            foreach (var keypair in formatInstance.keyPositions) {
                FormatDictionary.Add(keypair, formatInstance);
            }
        }

        public static FormatType GetFormat(FormatType.KeyPositionLang KeyPosition, out bool isSafe) {
            /* This method compares the keyword and positions of a KeyPosition objects with the Keys (keyword + position)
             * in FormatDictionary. When it finds two coinciding keys (both values in each key are equal to both values in dict
             * it returns the correct FormatType. Otherwise returns null. */

            var importedKeyPos = KeyPosition;
            var importedKeyword = KeyPosition.Keyword;
            var importedPosition = KeyPosition.Position;
            var importedLanguage = KeyPosition.Language;

            bool possibleFormatFound = false;
            bool safeFormatFound = false;
            FormatType possibleMatch = null;
            FormatType safeMatch = null;

            foreach (var keywordPosLangKeyring in FormatDictionary.Keys) {
                if (safeFormatFound != true) {
                    var dictionaryKeyPos = keywordPosLangKeyring;
                    var dictionaryKeyword = dictionaryKeyPos.Keyword;
                    var dictionaryPosition = dictionaryKeyPos.Position;
                    var dictionaryLanguage = dictionaryKeyPos.Language;

                    if ((importedKeyword == dictionaryKeyword) && (importedPosition == dictionaryPosition)
                        && importedLanguage == dictionaryLanguage) {
                        switch (importedPosition) {
                            case 1: //Keywords in position 1 catch base formats, while position 2 are subtypes (safe).
                                possibleMatch = FormatDictionary[keywordPosLangKeyring];
                                possibleFormatFound = true;
                                break;

                            case 2:
                                safeMatch = FormatDictionary[keywordPosLangKeyring];
                                safeFormatFound = true;
                                break;
                        }
                    }

                    if (safeFormatFound) {
                        break;
                    }
                }
                /* Formerly used to detect last entry in dictionary, now unnecessary due to foreach loop breaks.
                if (keywordPosLangKeyring.Equals(FormatDictionary.Last().Value) && (safeMatch == null))
                {
                    isLastFormatSafeMatch = false;
                    return possibleMatch;
                }
                */
            }

            if (safeFormatFound == true) {
                isSafe = true;
                return safeMatch;
            }
            else if ((safeFormatFound == false) && (possibleFormatFound == true)) {
                isSafe = false;
                return possibleMatch;
            }
            else {
                isSafe = false;
                return null;
            }
        }
    }
}