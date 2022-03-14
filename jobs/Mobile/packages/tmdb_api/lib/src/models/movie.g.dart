// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'movie.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Movie _$MovieFromJson(Map<String, dynamic> json) => $checkedCreate(
      'Movie',
      json,
      ($checkedConvert) {
        final val = Movie(
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
          budget: $checkedConvert('budget', (v) => v as int),
          genres: $checkedConvert(
              'genres',
              (v) => (v as List<dynamic>)
                  .map((e) => Genre.fromJson(e as Map<String, dynamic>))
                  .toList()),
          homepage: $checkedConvert('homepage', (v) => v as String?),
          imdbId: $checkedConvert('imdb_id', (v) => v as String?),
          overview: $checkedConvert('overview', (v) => v as String?),
          productionCompanies: $checkedConvert(
              'production_companies',
              (v) => (v as List<dynamic>)
                  .map((e) => Company.fromJson(e as Map<String, dynamic>))
                  .toList()),
          productionCountries: $checkedConvert(
              'production_countries',
              (v) => (v as List<dynamic>)
                  .map((e) => Country.fromJson(e as Map<String, dynamic>))
                  .toList()),
          revenue: $checkedConvert('revenue', (v) => v as int),
          runtime: $checkedConvert('runtime', (v) => v as int?),
          spokenLanguages: $checkedConvert(
              'spoken_languages',
              (v) => (v as List<dynamic>)
                  .map((e) => Language.fromJson(e as Map<String, dynamic>))
                  .toList()),
          status: $checkedConvert('status', (v) => v as String),
          tagline: $checkedConvert('tagline', (v) => v as String?),
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
        'imdbId': 'imdb_id',
        'productionCompanies': 'production_companies',
        'productionCountries': 'production_countries',
        'spokenLanguages': 'spoken_languages'
      },
    );

Map<String, dynamic> _$MovieToJson(Movie instance) => <String, dynamic>{
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
      'budget': instance.budget,
      'genres': instance.genres.map((e) => e.toJson()).toList(),
      'homepage': instance.homepage,
      'imdb_id': instance.imdbId,
      'overview': instance.overview,
      'production_companies':
          instance.productionCompanies.map((e) => e.toJson()).toList(),
      'production_countries':
          instance.productionCountries.map((e) => e.toJson()).toList(),
      'revenue': instance.revenue,
      'runtime': instance.runtime,
      'spoken_languages':
          instance.spokenLanguages.map((e) => e.toJson()).toList(),
      'status': instance.status,
      'tagline': instance.tagline,
    };
