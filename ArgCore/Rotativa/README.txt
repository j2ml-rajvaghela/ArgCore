﻿This folder contains file necessary to Rotativa to generate Pdf files

Please don't move or rename them or the folder itself

If you need to you should add a key in appsettings.json with the path to this files:

  <appSettings>
	
    <add key="WkhtmltopdfPath" value="c:\pathtothefolder"/>
	{
	  "WkhtmltopdfPath" : "c:\pathtothefolder"
	  "WkhtmltopdfPath" : "path"
	}

  </appSettings>