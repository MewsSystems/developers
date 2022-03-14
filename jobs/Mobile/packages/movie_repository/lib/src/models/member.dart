import 'package:json_annotation/json_annotation.dart';

part 'member.g.dart';

@JsonSerializable()
class Member {
  final String? posterPath;
  final String name;
  Member({
    this.posterPath,
    required this.name,
  });
}
