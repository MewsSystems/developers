// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'movie_details.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

MovieDetails _$MovieDetailsFromJson(Map<String, dynamic> json) =>
    $checkedCreate(
      'MovieDetails',
      json,
      ($checkedConvert) {
        final val = MovieDetails(
          adult: $checkedConvert('adult', (v) => v as bool),
          budget: $checkedConvert('budget', (v) => v as int),
          genres: $checkedConvert('genres',
              (v) => (v as List<dynamic>).map((e) => e as String).toList()),
          id: $checkedConvert('id', (v) => v as int),
          posterPath: $checkedConvert('poster_path', (v) => v as String?),
          backdrop: $checkedConvert('backdrop', (v) => v as String?),
          title: $checkedConvert('title', (v) => v as String),
          overview: $checkedConvert('overview', (v) => v as String?),
          releaseDate: $checkedConvert('release_date',
              (v) => v == null ? null : DateTime.parse(v as String)),
          runtime: $checkedConvert('runtime', (v) => v as int?),
          voteAverage:
              $checkedConvert('vote_average', (v) => (v as num).toDouble()),
          voteCount: $checkedConvert('vote_count', (v) => v as int),
        );
        return val;
      },
      fieldKeyMap: const {
        'posterPath': 'poster_path',
        'releaseDate': 'release_date',
        'voteAverage': 'vote_average',
        'voteCount': 'vote_count'
      },
    );

Map<String, dynamic> _$MovieDetailsToJson(MovieDetails instance) =>
    <String, dynamic>{
      'adult': instance.adult,
      'budget': instance.budget,
      'genres': instance.genres,
      'id': instance.id,
      'poster_path': instance.posterPath,
      'backdrop': instance.backdrop,
      'title': instance.title,
      'overview': instance.overview,
      'release_date': instance.releaseDate?.toIso8601String(),
      'runtime': instance.runtime,
      'vote_average': instance.voteAverage,
      'vote_count': instance.voteCount,
    };
