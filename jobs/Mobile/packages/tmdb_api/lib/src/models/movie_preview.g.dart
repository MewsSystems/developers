// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'movie_preview.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

MoviePreview _$MoviePreviewFromJson(Map<String, dynamic> json) =>
    $checkedCreate(
      'MoviePreview',
      json,
      ($checkedConvert) {
        final val = MoviePreview(
          adult: $checkedConvert('adult', (v) => v),
          backdropPath: $checkedConvert('backdrop_path',
              (v) => TMDbUtils.getFullPosterPath(v as String?)),
          id: $checkedConvert('id', (v) => v),
          originalLanguage: $checkedConvert('original_language', (v) => v),
          originalTitle: $checkedConvert('original_title', (v) => v),
          popularity: $checkedConvert('popularity', (v) => v),
          posterPath: $checkedConvert(
              'poster_path', (v) => TMDbUtils.getFullPosterPath(v as String?)),
          releaseDate: $checkedConvert(
              'release_date', (v) => TMDbUtils.convertToDateTime(v as String?)),
          title: $checkedConvert('title', (v) => v),
          video: $checkedConvert('video', (v) => v),
          voteAverage: $checkedConvert('vote_average', (v) => v),
          voteCount: $checkedConvert('vote_count', (v) => v),
          overview: $checkedConvert('overview', (v) => v as String),
          genreIds: $checkedConvert('genre_ids',
              (v) => (v as List<dynamic>).map((e) => e as int).toList()),
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
        'voteCount': 'vote_count',
        'genreIds': 'genre_ids'
      },
    );

Map<String, dynamic> _$MoviePreviewToJson(MoviePreview instance) =>
    <String, dynamic>{
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
      'overview': instance.overview,
      'genre_ids': instance.genreIds,
    };
