// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'movies_search_result.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_$_MoviesSearchResult _$$_MoviesSearchResultFromJson(
        Map<String, dynamic> json) =>
    _$_MoviesSearchResult(
      page: json['page'] as int?,
      results: (json['results'] as List<dynamic>?)
          ?.map((e) => Movie.fromJson(e as Map<String, dynamic>))
          .toList(),
    );

Map<String, dynamic> _$$_MoviesSearchResultToJson(
        _$_MoviesSearchResult instance) =>
    <String, dynamic>{
      'page': instance.page,
      'results': instance.results,
    };
