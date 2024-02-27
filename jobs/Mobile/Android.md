# Mews mobile developer task (Android)

You should start with creating a fork of the repository. When you're finished with the task, you should create a pull request. Alternatively, you can share your repo with Mews interviewers.

Your task will be to create a simple movie search application as a native Android app. The application will have 2 screens - search and movie detail. The search screen is the default screen, and should contain search input and display a paginated list of found movies with a way to load additional batch. Search should start automatically after typing into the input is finished - there is no need for a search button. Clicking on a movie gets you to the movie details screen where detailed information about the movie should be listed.

To retrieve information about movies, use [TheMovieDb API][1]. You can use our api key to authorize requests:

```
03b8572954325680265531140190fd2a
```

## Required technologies

Even though our native Android apps are written mostly in Java and on top of the standard Android View layout, we keep a close eye on new trends like Kotlin and [Jetpack Compose][2]. In general, you have the freedom to choose the tech stack, as we're mainly looking for clean and maintainable code, but be ready to discuss pros and cons of your decisions. :)

[1]: https://developers.themoviedb.org/3/getting-started/introduction
[2]: https://developer.android.com/jetpack/compose
