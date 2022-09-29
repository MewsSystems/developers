# Mews mobile developer task (Flutter)

You should start with creating a fork of the repository. When you're finished
with the task, you should create a pull request.

Your task will be to create a simple movie search application in Flutter. The
application will have 2 screens - search and movie detail. The search screen is
the default screen, and should contain search input and display paginated list
of found movies with a way to load additional batch. Search should start
automatically after typing into the input is finished - there is no need for a
search button. Clicking on a movie gets you to the movie details screen where
detailed information about the movie should be listed.

To retrieve information about movies, use [TheMovieDb API][1]. You can use our
api key to authorize requests:

```
03b8572954325680265531140190fd2a
```

## Required technologies

We use BLoC-inspired architecture with [flutter_bloc][2] library, so it's
recommended to use it for this task. You can read more about our approach
[here][3]. In general, you're free to choose other libraries, we're mainly
looking for a clean and maintainable code. But be ready to discuss pros and
contras of your solution :)

[1]: https://developers.themoviedb.org/3/getting-started/introduction
[2]: https://pub.dev/packages/flutter_bloc
[3]: https://developers.mews.com/one-year-in-production-with-flutter/
