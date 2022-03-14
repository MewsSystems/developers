import 'package:equatable/equatable.dart';
import 'package:json_annotation/json_annotation.dart';

part 'movie.g.dart';

@JsonSerializable()
class MovieDetails extends Equatable {
  const MovieDetails({
    required this.adult,
    required this.budget,
    required this.genres,
    required this.id,
    this.posterPath,
    this.backdrop,
    required this.title,
    this.overview,
    required this.releaseDate,
    this.runtime,
    required this.voteAverage,
    required this.voteCount,
  });

  final bool adult;
  final int budget;
  final List<String> genres;
  final int id;
  final String? posterPath;
  final String? backdrop;
  final String title;
  final String? overview;
  final DateTime? releaseDate;
  final int? runtime;
  final double voteAverage;
  final int voteCount;

  factory MovieDetails.fromJson(json) => _$MovieFromJson(json);

  Map<String, dynamic> toJson() => _$MovieToJson(this);

  @override
  List<Object?> get props => [
        adult,
        budget,
        genres,
        id,
        posterPath,
        backdrop,
        title,
        overview,
        releaseDate,
        runtime,
        voteAverage,
        voteCount
      ];
}
