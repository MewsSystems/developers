// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'language.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Language _$LanguageFromJson(Map<String, dynamic> json) => $checkedCreate(
      'Language',
      json,
      ($checkedConvert) {
        final val = Language(
          iso: $checkedConvert('iso_639_1', (v) => v as String),
          name: $checkedConvert('name', (v) => v as String),
        );
        return val;
      },
      fieldKeyMap: const {'iso': 'iso_639_1'},
    );

Map<String, dynamic> _$LanguageToJson(Language instance) => <String, dynamic>{
      'iso_639_1': instance.iso,
      'name': instance.name,
    };
