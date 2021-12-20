import 'package:json_annotation/json_annotation.dart';
import 'package:equatable/equatable.dart';
part 'movie_detail_raw.g.dart';

@JsonSerializable(fieldRename: FieldRename.snake)
class MovieDetailRaw extends Equatable {
  final int budget;
  final List<GenreRaw> genres;
  final String? overview;
  final String? posterPath;
  final String title;
  final String releaseDate;

  const MovieDetailRaw(  
    {required this.budget, 
    required this.genres,  
    required this.overview,
    required this.posterPath,
    required this.title,
    required this.releaseDate});
  
  factory MovieDetailRaw.fromJson(Map<String, dynamic> json) => _$MovieDetailRawFromJson(json);

   Map<String, dynamic> toJson() => _$MovieDetailRawToJson(this);

  @override
  List<Object?> get props => [budget, genres, overview, posterPath, title, releaseDate];
}

@JsonSerializable(fieldRename: FieldRename.snake)
class GenreRaw extends Equatable {
  final int id;
  final String name;

  const GenreRaw({ required this.id, required this.name});

  factory GenreRaw.fromJson(Map<String, dynamic> json) => _$GenreRawFromJson(json);

  Map<String, dynamic> toJson() => _$GenreRawToJson(this);

  @override
  List<Object?> get props => [id, name];
}

