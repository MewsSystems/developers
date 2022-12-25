// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'movie_details.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_$_MovieDetails _$$_MovieDetailsFromJson(Map<String, dynamic> json) =>
    _$_MovieDetails(
      id: json['id'] as int,
      budget: (json['budget'] as num).toDouble(),
      homepage: json['homepage'] as String,
      imdbId: json['imdb_id'] as String?,
      productionCompanies: (json['production_companies'] as List<dynamic>)
          .map((e) => e as Map<String, dynamic>)
          .toList(),
      productionCountries: (json['production_countries'] as List<dynamic>)
          .map((e) => Map<String, String>.from(e as Map))
          .toList(),
      revenue: json['revenue'] as int,
      runtime: json['runtime'] as int?,
      spokenLanguages: (json['spoken_languages'] as List<dynamic>)
          .map((e) => Map<String, String>.from(e as Map))
          .toList(),
      status: json['status'] as String,
      tagline: json['tagline'] as String?,
    );

Map<String, dynamic> _$$_MovieDetailsToJson(_$_MovieDetails instance) =>
    <String, dynamic>{
      'id': instance.id,
      'budget': instance.budget,
      'homepage': instance.homepage,
      'imdb_id': instance.imdbId,
      'production_companies': instance.productionCompanies,
      'production_countries': instance.productionCountries,
      'revenue': instance.revenue,
      'runtime': instance.runtime,
      'spoken_languages': instance.spokenLanguages,
      'status': instance.status,
      'tagline': instance.tagline,
    };
