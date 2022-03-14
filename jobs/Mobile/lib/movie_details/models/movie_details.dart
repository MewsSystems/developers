import 'package:equatable/equatable.dart';
import 'package:json_annotation/json_annotation.dart';

part 'movie_details.g.dart';

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

  static final empty = MovieDetails(
    adult: false,
    budget: 0,
    genres: [],
    id: 0,
    title: 'none',
    posterPath: null,
    releaseDate: DateTime(2017, 9, 7, 17, 30),
    voteAverage: 3.5,
    voteCount: 100,
    backdrop: 'something',
    overview: 'long very long text',
    runtime: 120,
  );

  factory MovieDetails.fromJson(Map<String, dynamic> json) =>
      _$MovieDetailsFromJson(json);

  Map<String, dynamic> toJson() => _$MovieDetailsToJson(this);

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
