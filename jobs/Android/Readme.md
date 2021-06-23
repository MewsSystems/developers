# Mews Android developer task

Download sample data from a given public REST API. Display the results in a list of cards on a screen with ability to select one card. Show a detail view based on the selected card.

## Application design

Make the design of the app responsive (i.e. such that it is able to run on mobile phones of different sizes and small and big tablets). You can ignore more obscure devices like TVs and watches.

Allow device rotation and create a layout which is following these rules:
- in portrait mode there is a list of items. Clicking on the item opens a new page with details view;
- in landscape mode there is a list view on the left side, and details view on the right side of the page.

Every card in the photo list should display a photo title and a thumbnail. It should be selectable and it should feature the ability to expand and collapse to show or hide a photo id and an albumId provided by API.

Design is up to you, but it should follow general guidelines of Material design for Android.

## API handling, JSON processing

After the application starts, it downloads initial portion of photos from the following REST endpoint (http://jsonplaceholder.typicode.com/photos?_start=0&_limit=30). All data processing should be in-memory. Handle all the possible cases of data loading properly (network exceptions, no internet access etc.)
Notify the user properly about errors or running networks requests. Design the network request in a way you can download additional data if the user list is scrolled at its very bottom.

## Requirements

- The application has to be made compileable in Android Studio IDE and runnable on the standard Android emulator of latest stable release of Andorid OS provided by the IDE.
- You are allowed to use any libraries and resources you wish.
- The application has to be able to run on Android 4.4 (API level 19).
- Application should be written in Kotlin.

## How to submit your task

Create a fork of this repository. When finished with the task, create a pull request.
