// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'country.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Country _$CountryFromJson(Map<String, dynamic> json) => $checkedCreate(
      'Country',
      json,
      ($checkedConvert) {
        final val = Country(
          iso: $checkedConvert('iso_3166_1', (v) => v as String),
          name: $checkedConvert('name', (v) => v as String),
        );
        return val;
      },
      fieldKeyMap: const {'iso': 'iso_3166_1'},
    );

Map<String, dynamic> _$CountryToJson(Country instance) => <String, dynamic>{
      'iso_3166_1': instance.iso,
      'name': instance.name,
    };
