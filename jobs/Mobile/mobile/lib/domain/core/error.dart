import 'package:freezed_annotation/freezed_annotation.dart';

part 'error.freezed.dart';

/// {@template cinephile_error}
/// Custom Implemtation of bass [Error] class.
///
/// A [CinephileError] is intended to convey information
/// to the user about a failure,
/// so that the error can be addressed programmatically.
/// {@endtemplate}
@freezed
class CinephileError with _$CinephileError {
  const CinephileError._();

  const factory CinephileError(String cinephileError) = _CinephileError;

  factory CinephileError.empty() {
    return const CinephileError('Unknown Error.');
  }
}
