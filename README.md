Steam to Epic Games Store Achievements converter
=============
With this tool, you can convert Steam Achievements (in VDF format) to CSV format compatible with Epic Games Store.
It will notify you if there are any missing locales.
It will automatically download achievement icons from Steam.

How to use
-----
How to obtain Raw Metadata for Steam Achievements:
* Go to https://partner.steamgames.com/
* Go to Apps & Packages→All Applications
* Select Steamworks Admin
* Go to Misc→View Raw Settings
* Select "Stats" in the dropdown menu
* Copy the text contents and save to a text file (this is a VDF file) with UTF8 Encoding

How to convert:
* There is an Input folder near the tool executable. Place the text VDF file there. It must contain the Raw Metadata from Steam of the Stats section (copy it completely to this new file)
* Open Settings.ini file near the tool executable. Here you need to configure the name format for the achievement icon files
* Launch the tool, ensure that the "Job is done" message is displayed
* You will find the conversion results in the Output folder near the tool executable. Place the achievement icon files there to it and everything is ready to use with Epic Games Store

In the case there is a locale mapping missing, please edit LocaleMapping.ini file to update it according to the [locale information table from Epic](https://dev.epicgames.com/docs/services/en-US/GameServices/BulkImporterExporterTool/index.html).

Implementation limitations
-----
* Currently it doesn't handle stats, only achievement definitions and localization are generated.

Prerequisites
-----
If you wish to build the project from the source code:
* [Visual Studio 2019](https://www.visualstudio.com/), any edition (JetBrains Rider works fine too)

Contributing
-----
Pull requests are welcome.

Acknowledgments
-----
This tool is using the following open-source libraries to read Steam VDF format and write CSV files:
* [VdfConverter](https://github.com/GerhardvanZyl/Steam-VDF-Converter)
* [KBCsv](https://github.com/kentcb/KBCsv)

License
-----
The code is provided under MIT License. Please read [LICENSE.md](LICENSE.md) for details.
