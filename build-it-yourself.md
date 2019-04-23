This library was built from the PJSIP source at [https://www.pjsip.org/download.htm](https://www.pjsip.org/download.htm)

We'll be harnessing the power and simplicity of the PJSUA2 project within this.

It uses the latest Windows 10 SDK version (10.0.17763.0) but you could build it against earlier or later versions.

The build machine was running Visual Studio (Community) 2017 with:
- .NET desktop development
- Desktop development with C++
- Universal Windows Platform development (not essential)
- Python development

SwigWin was also installed from [http://www.swig.org/download.html](http://www.swig.org/download.html) and the path to "swig.exe" was added to the system path variable.


The PJSIP source has a VS2015 solution "pjproject-vs14.sln" which can be opened and upgraded to later versions (e.g. VS2017) for building the source.

Once the solution is open, if you wish, you can disable "test" projects etc (as has been done to build this). You should also retarget the solution if you are using a newer build than the original source. The simplest way to do this is via the "Project" menu in Visual Studio.

The source came with a "swig_java_pjsua2" folder which was unloaded but is a variation of the key project we'll be adding to the solution.


Once you have the source downloaded, create a blank file "config_site.h" in the "pjlib/include/pj/" folder. This can be used to set specific properties but for simplicity at this stage a blank file is used to allow default build settings.

Now change the build type to "Debug Dynamic" or "Release Dynamic". You may not need this step, but I did.

Next, try building the solution. You may find you have issues with some of the projects. If so, try building specific projects on their own if the errors aren't obvious to trace.

Once the solution is built you can use "SWIG" to generate a wrapper for the PJSUA2 library.

Using a command prompt, change to the "pjsip-apps/src/swig/csharp" folder (or create the "csharp" sub-folder if it is missing).

If you've already added the path to "SWIG" to the system path, you can run the following command:

> swig -I../../../../pjlib/include -I../../../../pjlib-util/include -I../../../../pjmedia/include -I../../../../pjsip/include -I../../../../pjnath/include -w312 -c++ -csharp -o pjsua2_wrap.cpp ../pjsua2.i

This uses the "pjsua2.i" file and others as a template/instructions to generate the C# class files and C++ files to wrap the PJSUA2 library.
The relative paths should point the relavant source directories but you can experiment if these paths don't work for you.

One the process is complete you'll have a series of .cs class files in the "csharp" subfolder.

The next step is to create a new project in the solution which will build the managed DLL that you can use from a C# project.

Depending upon your version of Visual Studio, this might be a Win32 project or a C++ project. Use the Wizard option if it is available and select a project type of:
- Dynamic-Link Library
- Empty project
- Export symbols
- SDL checks (optional)

Once this is created, add references to "libpjproject" and "pjsua2_lib".
Then open "pjsip-apps/src/swig/csharp" and copy and paste into the project the following files:
- pjsua2_wrap.h
- pjsua2_wrap.cpp
- pjsua2.i

Next you want to amend the project properties in several places.
So first, open the Property Pages for your project and ensure the following values are set:

General
- Target Name = PJSUA2
- Target Extension = .dll
- Configuration Type = Dynamic Library (.dll)
- Common Language Runtime Support = Common Language Runtime Support (/clr)

C/C++ - General
- Additional Include Directories = ../pjsip/include;../pjlib/include;../pjlib-util/include;../pjmedia/include;../pjnath/include;%(AdditionalIncludeDirectories);

Linker - Input (or in All Options)
- Additional Dependencies = Iphlpapi.lib;dsound.lib;dxguid.lib;netapi32.lib;mswsock.lib;ws2_32.lib;odbc32.lib;odbccp32.lib;ole32.lib;user32.lib;gdi32.lib;advapi32.lib;%(AdditionalDependencies)
- Ignore Specific Default Libraries = msvcrt.lib;%(IgnoreSpecificDefaultLibraries)

Once again, the paths may need adjusting for your solution so if you get a file-not-found type of error, the include directories paths may need to be adjusted.

Now open the Project Properties and select the Project Dependencies. You can select pretty much all of the projects in the solution but the key ones are those referenced in the include paths above:
- libpjproject
- pjlib
- pjlib_util
- pjmedia
- pjnath
- pjsua2_lib

This is because these projects already reference others that they need.

You can now go ahead and try to build your new project. If it suceeds, you'll have a set of files in the output directory (as set in the project's Property Pages). By default this should be something like:
- $(SolutionDir)$(Configuration)\
which equates to the "Debug" folder in the the solution's main folder. It's the DLL file that you're most interested in.

That's you mostly done.

To use this now in a C# project, add the .cs files that were generated earlier into a new C# project and reference this DLL.
Don't include the sample.cs file if it is in the folder as it may have incorrect references, but you can use code within this file to test your work so far.

Good luck :)



