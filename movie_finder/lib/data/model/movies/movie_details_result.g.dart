// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'movie_details_result.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_$_MovieDetailsResult _$$_MovieDetailsResultFromJson(
        Map<String, dynamic> json) =>
    _$_MovieDetailsResult(
      title: json['title'] as String?,
      overview: json['overview'] as String?,
      originalTitle: json['original_title'] as String?,
      releaseDate: json['release_date'] as String?,
      posterPath: json['poster_path'] as String?,
      revenue: json['revenue'] as int?,
    );

Map<String, dynamic> _$$_MovieDetailsResultToJson(
        _$_MovieDetailsResult instance) =>
    <String, dynamic>{
      'title': instance.title,
      'overview': instance.overview,
      'original_title': instance.originalTitle,
      'release_date': instance.releaseDate,
      'poster_path': instance.posterPath,
      'revenue': instance.revenue,
    };
