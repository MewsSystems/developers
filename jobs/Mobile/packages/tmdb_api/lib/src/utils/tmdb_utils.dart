class TMDbUtils {
  static const String imageServerUrl = 'https://image.tmdb.org/t/p/w500';

  static String? getFullPosterPath(String? urlString) {
    if (urlString == null || urlString.isEmpty) return null;
    return '$imageServerUrl$urlString';
  }

  static String? getAddedPosterPath(String? urlString) {
    if (urlString == null || urlString.isEmpty) return null;
    return urlString.replaceAll(imageServerUrl, '');
  }

  static DateTime? convertToDateTime(String? dateString) {
    if (dateString == null || dateString.isEmpty) return null;
    return DateTime.tryParse(dateString);
  }
}
