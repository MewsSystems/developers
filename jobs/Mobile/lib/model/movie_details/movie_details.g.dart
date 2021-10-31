// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'movie_details.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_$_MoviesDetails _$$_MoviesDetailsFromJson(Map<String, dynamic> json) =>
    _$_MoviesDetails(
      adult: json['adult'] as bool,
      id: json['id'] as int,
      originalTitle: json['original_title'] as String? ?? '',
      description: json['overview'] as String? ?? '',
      releaseDate: json['release_date'] as String? ?? '',
    );

Map<String, dynamic> _$$_MoviesDetailsToJson(_$_MoviesDetails instance) =>
    <String, dynamic>{
      'adult': instance.adult,
      'id': instance.id,
      'original_title': instance.originalTitle,
      'overview': instance.description,
      'release_date': instance.releaseDate,
    };
