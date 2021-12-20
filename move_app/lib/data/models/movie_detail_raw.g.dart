// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'movie_detail_raw.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

MovieDetailRaw _$MovieDetailRawFromJson(Map<String, dynamic> json) =>
    MovieDetailRaw(
      budget: json['budget'] as int,
      genres: (json['genres'] as List<dynamic>)
          .map((e) => GenreRaw.fromJson(e as Map<String, dynamic>))
          .toList(),
      overview: json['overview'] as String?,
      posterPath: json['poster_path'] as String?,
      title: json['title'] as String,
      releaseDate: json['release_date'] as String,
    );

Map<String, dynamic> _$MovieDetailRawToJson(MovieDetailRaw instance) =>
    <String, dynamic>{
      'budget': instance.budget,
      'genres': instance.genres,
      'overview': instance.overview,
      'poster_path': instance.posterPath,
      'title': instance.title,
      'release_date': instance.releaseDate,
    };

GenreRaw _$GenreRawFromJson(Map<String, dynamic> json) => GenreRaw(
      id: json['id'] as int,
      name: json['name'] as String,
    );

Map<String, dynamic> _$GenreRawToJson(GenreRaw instance) => <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
    };
