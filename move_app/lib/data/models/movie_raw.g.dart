// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'movie_raw.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

MovieRaw _$MovieRawFromJson(Map<String, dynamic> json) => MovieRaw(
      id: json['id'] as int,
      title: json['title'] as String,
      releaseDate: json['release_date'] as String?,
      posterPath: json['poster_path'] as String?,
    );

Map<String, dynamic> _$MovieRawToJson(MovieRaw instance) => <String, dynamic>{
      'id': instance.id,
      'title': instance.title,
      'release_date': instance.releaseDate,
      'poster_path': instance.posterPath,
    };
