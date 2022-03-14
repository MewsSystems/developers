import 'package:equatable/equatable.dart';
import 'package:json_annotation/json_annotation.dart';

part 'member.g.dart';

@JsonSerializable()
class Member extends Equatable {
  Member({
    this.posterPath,
    required this.name,
  });

  final String? posterPath;
  final String name;

  factory Member.fromJson(Map<String, dynamic> json) => _$MemberFromJson(json);

  Map<String, dynamic> toJson() => _$MemberToJson(this);

  @override
  List<Object?> get props => [posterPath, name];
}
