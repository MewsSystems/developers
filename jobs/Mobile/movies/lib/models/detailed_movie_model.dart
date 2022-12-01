import 'package:equatable/equatable.dart';

class DetailedMovie extends Equatable {
  const DetailedMovie({
    required this.id,
    required this.backdropPath,
    required this.posterPath,
    required this.originalTitle,
    required this.tagline,
    required this.overview,
    required this.releaseDate,
    required this.budget,
    required this.revenue,
    required this.voteAverage,
    required this.voteCount,
  });

  factory DetailedMovie.fromJson(Map<String, dynamic> json) => DetailedMovie(
        id: json['id'] as int,
        backdropPath: json['backdrop_path'] as String?,
        posterPath: json['poster_path'] as String?,
        originalTitle: json['original_title'] as String,
        tagline: json['tagline'] as String?,
        overview: json['overview'] as String?,
        releaseDate: json['release_date'] as String?,
        budget: json['budget'] as int,
        revenue: json['revenue'] as int,
        voteAverage: double.parse(json['vote_average'].toString()),
        voteCount: json['vote_count'] as int,
      );

  final int id;
  final String? backdropPath;
  final String? posterPath;
  final String originalTitle;
  final String? tagline;
  final String? overview;
  final String? releaseDate;
  final int budget;
  final int revenue;
  final double voteAverage;
  final int voteCount;

  @override
  List<Object?> get props => [
        id,
        backdropPath,
        posterPath,
        originalTitle,
        tagline,
        overview,
        releaseDate,
        revenue,
        budget,
        voteAverage,
        voteCount,
      ];
}
