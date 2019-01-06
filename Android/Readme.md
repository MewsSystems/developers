# Mews Android developer task

Download sample data from a given public REST API. Display the results in a list of cards on a screen with ability to select one card. Show a detail view based on the selected card. Create a fork of this repository. When finished with the task, create a pull request.

## Application design
Make the design of the app responsive (i.e. such that is able to run on mobile phones of different sizes and small and big tablets). You can ignore more obscure devices like TVs and watches.
Allow device rotation and create a layout which is able to show the detail view on top of the screen as well as a list of photos below it in the portrait orientation and photo list on the left and detail view on the right side of the screen in landscape mode.
During the design process, follow general guiedlines of Material design for Android.

Every card in the photo list should display a photo title and a thumbnail. It should be selectable and it should feature the ability to expand and collapse to show or hide a photo id and an albumId provided by API.

A selected card should display its photo in the detail view. An URL path to photo is provided by the API.

## API handling, JSON processing
After the application starts, it downloads initial portion of photos from the following REST endpoint (http://jsonplaceholder.typicode.com/photos?_start=0&_limit=30). All data processing should be in-memory. Handle all the possible cases of data loading properly (network exceptions, no internet access etc.)
Notify the user properly about errors or running networks requests. Design the network request in a way you can download additional data if the user list is scrolled at its very bottom.

## Requirements
- The application has to be made compileable in Android Studio IDE and runnable on the standard Android emulator of latest stable release of Andorid OS provided by the IDE.
- You are allowed to use any libraries and resources you wish.
- The application has to be able to run on Android 4.4 (API level 19).
