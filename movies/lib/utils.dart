/// Prefix to build an image link from the API
const _apiImagePrefix = 'https://image.tmdb.org/t/p';

String getApiImageUrl(String path, int size) => '$_apiImagePrefix/w$size$path';

/// Format minutes to hours:minutes String
String formatMinutes(int value) {
  final int hour = value ~/ 60;
  final int minutes = value % 60;

  return '${hour.toString().padLeft(2, "0")}:${minutes.toString().padLeft(2, "0")}';
}
