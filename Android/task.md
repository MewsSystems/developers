Create original native application for Android OS that fullfils following requirements:

# Your task
Download sample data from public REST API. Show results in list of cards on screen with ability to select one card. Show detail view based on selected card. Create fork of this repository. When finished with task create pull request.

## Application design
Create responsive design, that is able to run on mobile phones of different sizes and small and big tablets. You can ignore more obscure devices like TV and watches.
Allow device rotation and create layout able to show detail view on top of the screen and list of photos below it in portrait orientation, and photo list on the left and detail view on the right side of screen in landscape mode.
During design process, follow general guiedlines of Material design for Android.

Every card in photo list should show photo title and thumbnail, should be selectable and should feature ability to expand and collapse to show or hide photo id and albumId provided by API.

Selected card should show its photo in detail view. URL path to photo is provided by API.

## API handling, JSON processing
After application starts, download initial portion of photos from following REST endpoint(http://jsonplaceholder.typicode.com/photos?_start=0&_limit=30). Keep all data processing in memory only. Handle all possible outcomes of data load properly (network exceptions, no internet access etc.)
Notify user properly about errors or running networks requests. Design network request in way you can donwload aditional data if user list is scrolled at very bottom.

## Requirements
- Application has to be made to be compileable in Android Studio IDE and runable on standard Android emulator of latest stable release of Andorid OS provided by IDE.
- You are allow to use any and all libraries and resources you wish.
- Application has to be able to run on Android 4.4 (API level 19)
