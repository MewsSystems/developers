// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'movie.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_$_Movie _$$_MovieFromJson(Map<String, dynamic> json) => _$_Movie(
      id: json['id'] as int,
      title: json['title'] as String,
      overview: json['overview'] as String,
      popularity: (json['popularity'] as num).toDouble(),
      isAdult: json['adult'] as bool,
      isVideo: json['video'] as bool,
      releaseDate: json['release_date'] as String,
      posterUrl: json['poster_path'] as String?,
      voteAverage: (json['vote_average'] as num).toDouble(),
      voteCount: json['vote_count'] as int,
    );

Map<String, dynamic> _$$_MovieToJson(_$_Movie instance) => <String, dynamic>{
      'id': instance.id,
      'title': instance.title,
      'overview': instance.overview,
      'popularity': instance.popularity,
      'adult': instance.isAdult,
      'video': instance.isVideo,
      'release_date': instance.releaseDate,
      'poster_path': instance.posterUrl,
      'vote_average': instance.voteAverage,
      'vote_count': instance.voteCount,
    };
