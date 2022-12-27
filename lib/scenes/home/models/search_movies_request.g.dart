// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'search_movies_request.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_$_SearchMoviesRequest _$$_SearchMoviesRequestFromJson(
        Map<String, dynamic> json) =>
    _$_SearchMoviesRequest(
      key: json['api_key'] as String? ?? API_KEY,
      searchText: json['query'] as String,
      page: json['page'] as int,
    );

Map<String, dynamic> _$$_SearchMoviesRequestToJson(
        _$_SearchMoviesRequest instance) =>
    <String, dynamic>{
      'api_key': instance.key,
      'query': instance.searchText,
      'page': instance.page,
    };
