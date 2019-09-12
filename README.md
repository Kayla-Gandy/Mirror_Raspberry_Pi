# Mirror_Raspberry_Pi
As a side project i have created a C# program to display news headlines, date, and time using a news API.
## Using
All code has been written in [Visual Studio 2017](https://www.nuget.org/packages/NewsAPI/) as well as [Visual Studio Code](https://code.visualstudio.com/) and compiled using the [.NET Core 2.2](https://dotnet.microsoft.com/download/dotnet-framework/net472). To install the API packages, I used [NuGet Package Manager](https://www.nuget.org/) and installed the [News API](https://www.nuget.org/packages/NewsAPI/) package using the following command. 
```bash
PM> Install-Package NewsAPI -Version 0.5.0
```
I have also included the following line in the project file (NEWS.csproj)
```bash
<PackageReference Include="NewsAPI" Version="0.5.0" />
```
## API Key
At line 115, the API key has been removed. This key was retreived at [NewsAPI.org](https://newsapi.org/)
