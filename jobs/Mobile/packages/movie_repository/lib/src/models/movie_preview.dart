import 'package:equatable/equatable.dart';
import 'package:json_annotation/json_annotation.dart';

part 'movie_preview.g.dart';

@JsonSerializable()
class MoviePreview extends Equatable {
  final String? posterPath;
  final int id;
  final String title;
  final double voteAverage;
  final DateTime? releaseDate;
  final String overview;

  const MoviePreview({
    this.posterPath,
    required this.id,
    required this.title,
    required this.voteAverage,
    required this.releaseDate,
    required this.overview,
  });

  factory MoviePreview.fromJson(json) => _$MoviePreviewFromJson(json);

  Map<String, dynamic> toJson() => _$MoviePreviewToJson(this);

  @override
  List<Object?> get props =>
      [posterPath, id, title, voteAverage, releaseDate, overview];
}
