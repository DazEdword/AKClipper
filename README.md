Introduction
------------

Amazing Kindly Clipper is a free tool created to make Amazon Kindle user’s life easier. Essentially, it is a parser that organizes the highlights, notes, and bookmarks from ‘My Clippings.txt’, letting you sort, search and filter your clippings, with exporting capabilities to CSV, TXT and HTML. 

Installation
------------

The program is available in two formats:

 * Installable, that also installs .NET 4.6.2 for you. Just install the file and look for a new Amazing Kindly Clipper entry in your Start Menu.
 * Executable, to be used only if you’re positive that you have .NET 4.6.2 installed. Unzip it into any folder of your choice and run it to go.

In both cases, your antivirus software might stop the program due to a false positive. Feel free to mark the program as safe or halt your antivirus during the installation, Amazing Kindly Clipper does nothing to your computer at any level, apart from writing files to your desktop on export (and only if you want to do that). 

How to use the program
----------------------
 * Copy your Kindle clippings file: Connect the Kindle to your computer via USB, find the device on the explorer, and then navigate to the Documents folder. There you should find a  ‘My Clippings.txt’ file (or, as stated, ‘Mis recortes.txt’ for Spanish models). Copy this file, and paste it on your desktop or any other folder at hand. 

 * Click the browse file button, and select your clipping’s file. You should be able to see a preview of your file in the preview text block if you picked a TXT file (even if it’s not a Clippings file, but I’d rather not do that!).

 * Your language should have been automatically selected, after browsing, my if it hasn't please select the language of your Kindle device. This step is very important: Spanish and English parsers are very different, and choosing the wrong parser might lead to errors and exceptions (AKA program crashing).

 * Click the Start Parsing button to start the process. For standard files in normal scenarios, the parsing should be carried out instantly, but you might need to wait a few seconds for especially heavy files (with tens of thousands of notes).

 * Clicked the parsing button you should see some dialog boxes with information, and a confirmation dialog with how many clippings have been added to the database. After that, the Browse Clippings window will appear. You’ll see now a grid with the Book Name, Author, Clipping Type, Page, Location, Date Added and Text of each clipping. Some of the cells might be missing (due to the clipping lacking said field, or to the parser not having been able to read it) so definitely don’t worry about that. In order to filter your results, select the category you want to filter in the dropdown called Filter by, type your search time in the Search field and click enter; your results should be showing immediately after. Hit the columns for automatic sorting. 

 * Export results: Copy and paste or use the built-in exporter. 

   - Copy and Paste: If you just wanted to search for a given keyword or find some detail of a single clipping, you can just select the columns or rows of your choice, use mouse right button and select “Copy”. Then you paste elsewhere! 

   - Export: Bear in mind that the program will export all the visible grid. You can export the whole database if you don’t type any search term, or a set of filtered results. In both cases, it’s as simple as ticking all the formats that you want to export, and then clicking the Export button to make them magically appear on your desktop. For the best results I recommend exporting .CSV to be used on LibreOffice Calc (a magnificent free and open source office suite), allowing you to manipulate the data in countless new ways. When openingthe CSV file, make sure to tick UTF-8 as character set, and separator options as 'Comma'. Other configurations could lead to weird layouts.

More info & Contact
-------------------

Ed Garabito - http://www.gottabegarabi.com/blog/amazing-kindly-clipper-for-kindle/

License
-------

This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org>.
