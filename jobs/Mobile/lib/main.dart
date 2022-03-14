import 'package:flutter/material.dart';
import 'package:mews_imdb/app.dart';
import 'package:movie_repository/movie_repository.dart';

void main() => runApp(
      IMDbApp(
        movieRepository: MovieRepository(),
      ),
    );
