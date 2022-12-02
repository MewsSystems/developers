import 'package:equatable/equatable.dart';
import 'package:json_annotation/json_annotation.dart';

part 'movie_model.g.dart';

@JsonSerializable(explicitToJson: true, createToJson: false)
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

  factory Movie.fromJson(Map<String, dynamic> json) => _$MovieFromJson(json);

  final int id;
  @JsonKey(name: 'backdrop_path')
  final String? backdropPath;
  @JsonKey(name: 'poster_path')
  final String? posterPath;
  @JsonKey(name: 'original_title')
  final String originalTitle;
  @JsonKey(name: 'release_date')
  final String? releaseDate;
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
        releaseDate,
        voteAverage,
        voteCount,
      ];
}
