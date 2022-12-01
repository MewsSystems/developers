import 'package:equatable/equatable.dart';

class Movie extends Equatable {
  const Movie({
    required this.id,
    required this.backdropPath,
    required this.posterPath,
    required this.originalTitle,
    required this.releaseDate,
    required this.voteAverage,
    required this.voteCount,
  });

  factory Movie.fromJson(Map<String, dynamic> json) => Movie(
        id: json['id'] as int,
        backdropPath: json['backdrop_path'] as String?,
        posterPath: json['poster_path'] as String?,
        originalTitle: json['original_title'] as String,
        releaseDate: json['release_date'] as String?,
        voteAverage: double.parse(json['vote_average'].toString()),
        voteCount: json['vote_count'] as int,
      );

  final int id;
  final String? backdropPath;
  final String? posterPath;
  final String originalTitle;
  final String? releaseDate;
  final double voteAverage;
  final int voteCount;

  @override
  List<Object?> get props => [
        id,
        backdropPath,
        posterPath,
        originalTitle,
        releaseDate,
        voteAverage,
        voteCount,
      ];
}
