// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'crew_member.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

CrewMember _$CrewMemberFromJson(Map<String, dynamic> json) => $checkedCreate(
      'CrewMember',
      json,
      ($checkedConvert) {
        final val = CrewMember(
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
          department: $checkedConvert('department', (v) => v as String),
          job: $checkedConvert('job', (v) => v as String),
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

Map<String, dynamic> _$CrewMemberToJson(CrewMember instance) =>
    <String, dynamic>{
      'adult': instance.adult,
      'gender': instance.gender,
      'id': instance.id,
      'known_for_department': instance.knownForDepartment,
      'name': instance.name,
      'original_name': instance.originalName,
      'popularity': instance.popularity,
      'profile_path': TMDbUtils.getAddedPosterPath(instance.profilePath),
      'credit_id': instance.creditId,
      'department': instance.department,
      'job': instance.job,
    };
