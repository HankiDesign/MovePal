# MovePal

Xamarin.Forms application to make it easier to define the target area for an apartment or a job in Helsinki based on maximum travel time and commute preferences such as the time of day and the mode of transport. This application uses the geocoding API and Itinerary API from HSL, as well as Mapbox SDK to show the maps.

## Building

### Development environment

The project was developed on macOS Mojave (10.14.6) with [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/) (the free Community version is enough) v. 8.7. However, this should work just fine on a Windows Machine using [Visual Studio](https://visualstudio.microsoft.com/vs/) as well.

You'll also need the following dependencies:

- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- [.NET Core Runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- Xamarin.Android 11.0.0.3 or newer (Xamarin can be installed as part of Visual Studio for Mac.
  - SDK Tools Version: 26.1.1
  - SDK Platform Tools Version: 30.0.4
  - SDK Build Tools Version: 29.0.3
- Microsoft OpenJDK for Mobile

### Compiling and running

The project contains an iOS project which is however NOT configured to work yet. Only the Android version works. Follow the steps below to build the project and run it on a real device (or an emulator):

1. Open the .sln file in the root of the project
2. Register an account on [mapbox.com](https://www.mapbox.com/)
    - Go to the [Account page](https://account.mapbox.com/)
    - Create a new access token by clicking the blue *Create a token* button in the *Access tokens* section
    - Copy the token
3. Open the *strings.xml* file in *HSLMapApp.Android > Resources > values > strings.xml*
4. Paste the newly created access token to the access token field (to replace the text *Add your own...*
5. Make sure that the Android project is set as the startup project and that there's a phone connected to the computer with [USB debugging enabled](https://developer.android.com/studio/debug/dev-options)
6. Click the run button on the top left corner (on Visual Studio for Mac).
7. The application should now install on the device and run once the installation is complete.

<img src="https://github.com/HankiDesign/MovePal/blob/master/Screenshots/1.jpeg" />
<img src="https://github.com/HankiDesign/MovePal/blob/master/Screenshots/2.jpeg" />
<img src="https://github.com/HankiDesign/MovePal/blob/master/Screenshots/3.jpeg" />
