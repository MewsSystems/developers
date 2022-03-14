// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'search_result.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

SearchResult _$SearchResultFromJson(Map<String, dynamic> json) =>
    $checkedCreate(
      'SearchResult',
      json,
      ($checkedConvert) {
        final val = SearchResult(
          page: $checkedConvert('page', (v) => v as int),
          results: $checkedConvert(
              'results',
              (v) => (v as List<dynamic>)
                  .map((e) => MoviePreview.fromJson(e as Map<String, dynamic>))
                  .toList()),
          totalResults: $checkedConvert('total_results', (v) => v as int),
          totalPages: $checkedConvert('total_pages', (v) => v as int),
        );
        return val;
      },
      fieldKeyMap: const {
        'totalResults': 'total_results',
        'totalPages': 'total_pages'
      },
    );

Map<String, dynamic> _$SearchResultToJson(SearchResult instance) =>
    <String, dynamic>{
      'page': instance.page,
      'results': instance.results.map((e) => e.toJson()).toList(),
      'total_results': instance.totalResults,
      'total_pages': instance.totalPages,
    };
