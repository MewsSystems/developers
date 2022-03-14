// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'member.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Member _$MemberFromJson(Map<String, dynamic> json) => $checkedCreate(
      'Member',
      json,
      ($checkedConvert) {
        final val = Member(
          posterPath: $checkedConvert('poster_path', (v) => v as String?),
          name: $checkedConvert('name', (v) => v as String),
        );
        return val;
      },
      fieldKeyMap: const {'posterPath': 'poster_path'},
    );

Map<String, dynamic> _$MemberToJson(Member instance) => <String, dynamic>{
      'poster_path': instance.posterPath,
      'name': instance.name,
    };
