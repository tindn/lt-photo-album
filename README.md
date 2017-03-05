## About

This repository contains the source code for a technical assessment. The requirement for the challenge is as follows

>Create a console application that displays photo ids and titles in an album. The photos are available in this online web service (https://jsonplaceholder.typicode.com/photos).

>Hint: Photos are filtered with a query string. This will return photos within albumId=3 (https://jsonplaceholder.typicode.com/photos?albumId=3)

>You can use any language
Any open source libraries
Unit tests are encouraged
Post your solution on any of the free code repositories and send us a link:
https://github.com/
https://gitlab.com
https://bitbucket.org/
Provide a README that contains instructions on how to build and run your application

>Spend as much (or little) time as you'd like on this. Feel free to use any resources available.

>Example: 
>> photo-album 2

>>\>[53] soluta et harum aliquid officiis ab omnis consequatur

>>\>[54] ut ex quibusdam dolore mollitia
>…

>> photo-album 3

>>\>[101] incidunt alias vel enim

>>\>[102] eaque iste corporis tempora vero distinctio consequuntur nisi nesciun

## Assumptions 

The instruction is generic to allow for flexibility. However, as I thought about this, I decided to impose certain constraints and assumptions to help make decisions along the way.

1. I'm choosing to use C# language and .NET framework to display my profiency in .NET. I also want to write a similar application in JavaScript and NodeJs in the future.

1. There's no specification on the run time environment. I assume this means it should be cross-platform. Given this, .NET Core is the obvious framework of choice. However, at the time of writing this (March 3-5 2017), there are big changes in .NET Core. The project.json style of .NET Core that has been available since 2015 has been scraped and replaced by an improved csproj version as of <a href="https://blogs.msdn.microsoft.com/dotnet/2016/11/16/announcing-net-core-tools-msbuild-alpha" target="_blank">.NET Core Preview 3</a> (released Nov 2016). This means running a .NET Core project will require downloading the right SDK. While this isn't challenging, with the upcoming release of VS 2017 (March 7th), things are a little unclear. As such, I have made the decision to write the application targeting .NET framework 4.6.1.

1. There are a number of choices for unit testing frameworks, including NUnit, xUnit, and MSTest. NUnit was chosen for familiarity with the package. This allows for faster development.

1. NewtonsoftJson is used to deserialize the json response received from endpoint provided. Again, this package is used based on my own familiarity with it.

1. Usage: the usage illustrated by the instruction is quite simple. I would need to add a help function, as well as a way to control the number of photos returned, and a way to scroll through the photos.

## Source code and run instructions

The .NET code is <a href="https://github.com/tindn/lt-photo-album/tree/master/dotnet/photo-album" target="_blank">here</a>.

You can use Visual studio to open, build, and run the project. I used VS 2015, but it can be opened with previous versions of VS as well. 
 
When running the program in Visual studio, the default argument is -h to display the help text. From there, you can then use the right command. 

I have also uploaded the zip files so it can be run from any command prompt on Windows without using Visual Studio. 

1. Just download the zipped files <a href="https://github.com/tindn/lt-photo-album/tree/master/dotnet/photo-album" target="_blank">here</a> and extract them to a specific folder. The files include the executable, the config which has the source url, and the NewtonsoftJson dll.

1. Open up command prompt, powershell, or your favorite command line tool (mine is <a href="http://cmder.net/" target="_blank">cmder</a>). Go to the directory where you extracted the files (or even the bin folder if you build the source code in VS). You can use `cd path\to\file` or `pushd path\to\file` to get there.

1. Simply type `photo-album` to run the executable and watch the magic happen.

If you can't run the program, please open an issue on Github and I can assist.

*Note: this program only runs on Windows*

## Evaluations and Improvements

1. This program is written to fulfil a simple requirement of displaying photos from a specific album. There should be a validation of album id input.

1. While the users of this program probably know what command to use, I felt that it was important to have some help text.

1. Unit tests were written to cover the logic for processing arguments, retrieving photos, and displaying photo count. No unit tests were written to verify console output, as this would require decomposing the simple app into smaller components, going beyond its purpose. This means only 67% code coverage.