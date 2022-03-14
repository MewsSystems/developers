// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'movie_base.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

MovieBase _$MovieBaseFromJson(Map<String, dynamic> json) => $checkedCreate(
      'MovieBase',
      json,
      ($checkedConvert) {
        final val = MovieBase(
          adult: $checkedConvert('adult', (v) => v as bool),
          backdropPath: $checkedConvert('backdrop_path',
              (v) => TMDbUtils.getFullPosterPath(v as String?)),
          id: $checkedConvert('id', (v) => v as int),
          originalLanguage:
              $checkedConvert('original_language', (v) => v as String),
          originalTitle: $checkedConvert('original_title', (v) => v as String),
          popularity: $checkedConvert('popularity', (v) => v as num),
          posterPath: $checkedConvert(
              'poster_path', (v) => TMDbUtils.getFullPosterPath(v as String?)),
          releaseDate: $checkedConvert(
              'release_date', (v) => TMDbUtils.convertToDateTime(v as String?)),
          title: $checkedConvert('title', (v) => v as String),
          video: $checkedConvert('video', (v) => v as bool),
          voteAverage: $checkedConvert('vote_average', (v) => v as num),
          voteCount: $checkedConvert('vote_count', (v) => v as int),
        );
        return val;
      },
      fieldKeyMap: const {
        'backdropPath': 'backdrop_path',
        'originalLanguage': 'original_language',
        'originalTitle': 'original_title',
        'posterPath': 'poster_path',
        'releaseDate': 'release_date',
        'voteAverage': 'vote_average',
        'voteCount': 'vote_count'
      },
    );

Map<String, dynamic> _$MovieBaseToJson(MovieBase instance) => <String, dynamic>{
      'adult': instance.adult,
      'backdrop_path': TMDbUtils.getAddedPosterPath(instance.backdropPath),
      'id': instance.id,
      'original_language': instance.originalLanguage,
      'original_title': instance.originalTitle,
      'popularity': instance.popularity,
      'poster_path': TMDbUtils.getAddedPosterPath(instance.posterPath),
      'release_date': instance.releaseDate?.toIso8601String(),
      'title': instance.title,
      'video': instance.video,
      'vote_average': instance.voteAverage,
      'vote_count': instance.voteCount,
    };
