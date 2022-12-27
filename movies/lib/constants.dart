import 'package:flutter/material.dart';
import 'package:movies/src/model/movie/movie.dart';

const apiImagePrefix = 'https://image.tmdb.org/t/p';

const chipColor = Color(0xFF001B29);
const bgColor = Color(0xFF000D14);
const accentColor = Color(0xFFffe695);


final List<Movie> items = [
  Movie(
    id: 1234,
    title: 'Avatar 2',
    originalTitle: 'Avatar 2',
    originalLanguage: 'French',
    backdropPath:
        'https://fr.web.img4.acsta.net/pictures/22/11/02/14/49/4565071.jpg',
    posterPath:
        'https://fr.web.img4.acsta.net/pictures/22/11/02/14/49/4565071.jpg',
    releaseDate: DateTime(2022, 12, 14),
    popularity: 9,
    voteAverage: 8.3,
    overview: 'Lorem ipsum',
    genreIds: [1, 2],
  ),
];

/// Format minutes to hours:minutes String
String formatMinutes(int value) {
  final int hour = value ~/ 60;
  final int minutes = value % 60;
  return '${hour.toString().padLeft(2, "0")}:${minutes.toString().padLeft(2, "0")}';
}