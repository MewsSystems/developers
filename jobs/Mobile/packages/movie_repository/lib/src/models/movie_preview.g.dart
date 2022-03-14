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
          posterPath: $checkedConvert('poster_path', (v) => v as String?),
          id: $checkedConvert('id', (v) => v as int),
          title: $checkedConvert('title', (v) => v as String),
          voteAverage:
              $checkedConvert('vote_average', (v) => (v as num).toDouble()),
          releaseDate: $checkedConvert('release_date',
              (v) => v == null ? null : DateTime.parse(v as String)),
          overview: $checkedConvert('overview', (v) => v as String),
        );
        return val;
      },
      fieldKeyMap: const {
        'posterPath': 'poster_path',
        'voteAverage': 'vote_average',
        'releaseDate': 'release_date'
      },
    );

Map<String, dynamic> _$MoviePreviewToJson(MoviePreview instance) =>
    <String, dynamic>{
      'poster_path': instance.posterPath,
      'id': instance.id,
      'title': instance.title,
      'vote_average': instance.voteAverage,
      'release_date': instance.releaseDate?.toIso8601String(),
      'overview': instance.overview,
    };
