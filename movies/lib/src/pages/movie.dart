/// A placeholder class that represents an entity or model.
class Movie {
  const Movie({
    required this.id,
    required this.title,
    required this.posterUrl,
    required this.releaseDate,
    required this.vote,
  });
  final String id;
  final String title;
  final String posterUrl;
  final DateTime releaseDate;
  final double vote;
}
