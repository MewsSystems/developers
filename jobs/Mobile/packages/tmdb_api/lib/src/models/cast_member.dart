import 'package:json_annotation/json_annotation.dart';
import 'package:tmdb_api/src/models/member.dart';
import 'package:tmdb_api/src/utils/tmdb_utils.dart';

part 'cast_member.g.dart';

@JsonSerializable()
class CastMember extends Member {
  final int castId;
  final String character;
  final int order;

  CastMember({
    required bool adult,
    required int? gender,
    required int id,
    required String knownForDepartment,
    required String name,
    required String originalName,
    required num popularity,
    required String? profilePath,
    required String creditId,
    required this.castId,
    required this.character,
    required this.order,
  }) : super(
            adult: adult,
            gender: gender,
            id: id,
            knownForDepartment: knownForDepartment,
            name: name,
            originalName: originalName,
            popularity: popularity,
            profilePath: profilePath,
            creditId: creditId);

  factory CastMember.fromJson(Map<String, dynamic> json) =>
      _$CastMemberFromJson(json);

  Map<String, dynamic> toJson() => _$CastMemberToJson(this);
}
