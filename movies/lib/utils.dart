import 'package:movies/src/model/movie/movie.dart';

/// Prefix to build an image link from the API
const apiImagePrefix = 'https://image.tmdb.org/t/p';

/// Format minutes to hours:minutes String
String formatMinutes(int value) {
  final int hour = value ~/ 60;
  final int minutes = value % 60;
  return '${hour.toString().padLeft(2, "0")}:${minutes.toString().padLeft(2, "0")}';
}