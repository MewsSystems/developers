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
          adult: $checkedConvert('adult', (v) => v as bool),
          gender: $checkedConvert('gender', (v) => v as int?),
          id: $checkedConvert('id', (v) => v as int),
          knownForDepartment:
              $checkedConvert('known_for_department', (v) => v as String),
          name: $checkedConvert('name', (v) => v as String),
          originalName: $checkedConvert('original_name', (v) => v as String),
          popularity: $checkedConvert('popularity', (v) => v as num),
          profilePath: $checkedConvert(
              'profile_path', (v) => TMDbUtils.getFullPosterPath(v as String?)),
          creditId: $checkedConvert('credit_id', (v) => v as String),
        );
        return val;
      },
      fieldKeyMap: const {
        'knownForDepartment': 'known_for_department',
        'originalName': 'original_name',
        'profilePath': 'profile_path',
        'creditId': 'credit_id'
      },
    );

Map<String, dynamic> _$MemberToJson(Member instance) => <String, dynamic>{
      'adult': instance.adult,
      'gender': instance.gender,
      'id': instance.id,
      'known_for_department': instance.knownForDepartment,
      'name': instance.name,
      'original_name': instance.originalName,
      'popularity': instance.popularity,
      'profile_path': TMDbUtils.getAddedPosterPath(instance.profilePath),
      'credit_id': instance.creditId,
    };
