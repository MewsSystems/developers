import 'package:json_annotation/json_annotation.dart';

part 'company.g.dart';

@JsonSerializable()
class Company {
  final String name;
  final int id;
  final String? logoPath;
  final String originCountry;
  Company({
    required this.name,
    required this.id,
    this.logoPath,
    required this.originCountry,
  });

  factory Company.fromJson(Map<String, dynamic> json) =>
      _$CompanyFromJson(json);

  Map<String, dynamic> toJson() => _$CompanyToJson(this);
}
