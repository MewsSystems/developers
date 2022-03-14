import 'package:json_annotation/json_annotation.dart';
import 'package:tmdb_api/src/models/member.dart';
import 'package:tmdb_api/src/utils/tmdb_utils.dart';

part 'crew_member.g.dart';

@JsonSerializable()
class CrewMember extends Member {
  final String department;
  final String job;

  CrewMember({
    required bool adult,
    required int? gender,
    required int id,
    required String knownForDepartment,
    required String name,
    required String originalName,
    required num popularity,
    required String? profilePath,
    required String creditId,
    required this.department,
    required this.job,
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

  factory CrewMember.fromJson(Map<String, dynamic> json) =>
      _$CrewMemberFromJson(json);

  Map<String, dynamic> toJson() => _$CrewMemberToJson(this);
}
