// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'movies_search_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_$_MoviesSearchRequest _$$_MoviesSearchRequestFromJson(
        Map<String, dynamic> json) =>
    _$_MoviesSearchRequest(
      query: json['query'] as String,
      page: MoviesSearchRequest._stringToInt(json['page'] as String),
    );

Map<String, dynamic> _$$_MoviesSearchRequestToJson(
        _$_MoviesSearchRequest instance) =>
    <String, dynamic>{
      'query': instance.query,
      'page': MoviesSearchRequest._stringFromInt(instance.page),
    };
