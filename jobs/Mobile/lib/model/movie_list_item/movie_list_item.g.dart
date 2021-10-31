// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'movie_list_item.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_$_MovieListItem _$$_MovieListItemFromJson(Map<String, dynamic> json) =>
    _$_MovieListItem(
      adult: json['adult'] as bool,
      id: json['id'] as int,
      originalTitle: json['original_title'] as String,
      description: json['overview'] as String,
      releaseDate: json['release_date'] as String,
    );

Map<String, dynamic> _$$_MovieListItemToJson(_$_MovieListItem instance) =>
    <String, dynamic>{
      'adult': instance.adult,
      'id': instance.id,
      'original_title': instance.originalTitle,
      'overview': instance.description,
      'release_date': instance.releaseDate,
    };
