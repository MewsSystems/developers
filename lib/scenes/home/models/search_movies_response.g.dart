// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'search_movies_response.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_$_SearchMoviesResponse _$$_SearchMoviesResponseFromJson(
        Map<String, dynamic> json) =>
    _$_SearchMoviesResponse(
      page: json['page'] as int,
      totalPages: json['total_pages'] as int,
      totalResults: json['total_results'] as int,
      movies: (json['results'] as List<dynamic>)
          .map((e) => Movie.fromJson(e as Map<String, dynamic>))
          .toList(),
    );

Map<String, dynamic> _$$_SearchMoviesResponseToJson(
        _$_SearchMoviesResponse instance) =>
    <String, dynamic>{
      'page': instance.page,
      'total_pages': instance.totalPages,
      'total_results': instance.totalResults,
      'results': instance.movies,
    };
