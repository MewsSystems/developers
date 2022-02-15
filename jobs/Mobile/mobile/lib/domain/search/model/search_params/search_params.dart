import 'package:freezed_annotation/freezed_annotation.dart';

part 'search_params.freezed.dart';

/// {@template search_params}
/// A delegate for data transfers.
/// {@endtemplate}
@freezed
class SearchParams with _$SearchParams {
  const SearchParams._();

  const factory SearchParams({required String query, required String page}) =
      _SearchParams;
}
