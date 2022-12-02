import 'package:equatable/equatable.dart';
import 'package:json_annotation/json_annotation.dart';

part 'detailed_movie_model.g.dart';

@JsonSerializable(explicitToJson: true, createToJson: false)
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

  factory DetailedMovie.fromJson(Map<String, dynamic> json) =>
      _$DetailedMovieFromJson(json);

  final int id;
  @JsonKey(name: 'backdrop_path')
  final String? backdropPath;
  @JsonKey(name: 'poster_path')
  final String? posterPath;
  @JsonKey(name: 'original_title')
  final String originalTitle;
  final String? tagline;
  final String? overview;
  @JsonKey(name: 'release_date')
  final String? releaseDate;
  final int budget;
  final int revenue;
  @JsonKey(name: 'vote_average')
  final double voteAverage;
  @JsonKey(name: 'vote_count')
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
