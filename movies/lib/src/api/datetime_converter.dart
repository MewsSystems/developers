import 'package:json_annotation/json_annotation.dart';

/// DateTime converter that supports converting empty strings as null
class DateTimeConverter implements JsonConverter<DateTime?, String> {
  const DateTimeConverter();

  @override
  DateTime? fromJson(String json) => json.isEmpty ? null : DateTime.parse(json);

  @override
  String toJson(DateTime? object) => object?.toIso8601String() ?? '';
}
