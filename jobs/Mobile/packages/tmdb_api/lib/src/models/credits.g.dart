// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'credits.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Credits _$CreditsFromJson(Map<String, dynamic> json) => $checkedCreate(
      'Credits',
      json,
      ($checkedConvert) {
        final val = Credits(
          id: $checkedConvert('id', (v) => v as int),
          cast: $checkedConvert(
              'cast',
              (v) => (v as List<dynamic>)
                  .map((e) => CastMember.fromJson(e as Map<String, dynamic>))
                  .toList()),
          crew: $checkedConvert(
              'crew',
              (v) => (v as List<dynamic>)
                  .map((e) => CrewMember.fromJson(e as Map<String, dynamic>))
                  .toList()),
        );
        return val;
      },
    );

Map<String, dynamic> _$CreditsToJson(Credits instance) => <String, dynamic>{
      'id': instance.id,
      'cast': instance.cast.map((e) => e.toJson()).toList(),
      'crew': instance.crew.map((e) => e.toJson()).toList(),
    };
