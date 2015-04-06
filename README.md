# deja-vu
Desktop application replay manager for OBS instant replays.

### What does it do?
Deja-Vu allows you to monitor when video buffers are written to a directory and saves them into "replay-slots", so that you can organize and rapidly select them. This should be used in tandem with OBS. The program can save as many slots as you have hard disk space. Deja-Vu can also encode replays in slow motion by stretching the frame rate after converting to `.mkv` format. You can upload replays to [gfycat](https://gfycat.com/), as well as have a bot use the new gfycat link on creation (your bot must have an http listener for this to work).

### Install process
1. Clone repository
2. Open the project solution `deja-vu.sln` in Visual Studio 2013 or higher (depends on .NET 4.5.1+)
3. Download [MKVToolNix](http://www.fosshub.com/MKVToolNix.html) for slow motion encoding.
4. On first launch, be sure to change the settings within the Application `File -> Settings`.

### What's next?
+ Native replay viewing
+ Automatic updates once we start hosting builds of the application
