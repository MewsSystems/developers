/// Prefix to build an image link from the API
const _apiImagePrefix = 'https://image.tmdb.org/t/p';

/// Builds an API image URL according to the given [path] and [size]
String getApiImageUrl(String path, int size) => '$_apiImagePrefix/w$size$path';

/// Format the minutes [value] to an hours:minutes String
String formatMinutes(int value) {
  final int hour = value ~/ 60;
  final int minutes = value % 60;

  return '${hour.toString().padLeft(2, "0")}:${minutes.toString().padLeft(2, "0")}';
}
