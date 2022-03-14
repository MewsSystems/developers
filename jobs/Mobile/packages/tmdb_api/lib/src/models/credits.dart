import 'package:json_annotation/json_annotation.dart';

import 'cast_member.dart';
import 'crew_member.dart';

part 'credits.g.dart';

@JsonSerializable()
class Credits {
  final int id;
  final List<CastMember> cast;
  final List<CrewMember> crew;
  Credits({
    required this.id,
    required this.cast,
    required this.crew,
  });

  factory Credits.fromJson(Map<String, dynamic> json) =>
      _$CreditsFromJson(json);

  Map<String, dynamic> toJson() => _$CreditsToJson(this);
}
