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
          previews: $checkedConvert(
              'previews',
              (v) => (v as List<dynamic>)
                  .map((e) => MoviePreview.fromJson(e))
                  .toList()),
          totalPages: $checkedConvert('total_pages', (v) => v as int),
          totalResults: $checkedConvert('total_results', (v) => v as int),
        );
        return val;
      },
      fieldKeyMap: const {
        'totalPages': 'total_pages',
        'totalResults': 'total_results'
      },
    );

Map<String, dynamic> _$SearchResultToJson(SearchResult instance) =>
    <String, dynamic>{
      'previews': instance.previews.map((e) => e.toJson()).toList(),
      'total_pages': instance.totalPages,
      'total_results': instance.totalResults,
    };
