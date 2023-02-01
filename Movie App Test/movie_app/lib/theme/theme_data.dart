import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:movie_app/theme/color_palette.dart';
import 'package:movie_app/theme/text_theme.dart';

class MovieAppThemeData {
  final MovieAppColorPalette colorPalette;
  final MovieAppTextTheme movieAppTextTheme;

  MovieAppThemeData({
    required this.colorPalette,
    required MovieAppTextTheme Function(MovieAppColorPalette)
        movieAppTextThemeBuilder,
  }) : movieAppTextTheme = movieAppTextThemeBuilder(colorPalette);

  static MovieAppThemeData light() => MovieAppThemeData(
        colorPalette: MovieAppColorPalette(),
        movieAppTextThemeBuilder: (colors) => MovieAppTextTheme(
          titleLarge: const TextStyle(
            fontWeight: FontWeight.w700,
            letterSpacing: 1.2,
            fontSize: 20,
          ),
          bodyText: const TextStyle(
            fontSize: 14.0,
            fontWeight: FontWeight.w400,
            letterSpacing: 1.2,
          ),
          durationText: TextStyle(
            color: colors.black,
            fontSize: 16.0,
            fontWeight: FontWeight.w500,
            letterSpacing: 1.2,
          ),
          genreText: TextStyle(
            color: colors.black,
            fontSize: 12.0,
            fontWeight: FontWeight.w500,
            letterSpacing: 1.2,
          ),
        ),
      );
}

extension MaterialThemeData on MovieAppThemeData {
  ThemeData get materialThemeData => ThemeData(
        primaryTextTheme: ThemeData.light().textTheme,
        appBarTheme: AppBarTheme(
            systemOverlayStyle: SystemUiOverlayStyle.dark,
            color: colorPalette.black,
            titleTextStyle: movieAppTextTheme.titleLarge),
        fontFamily: 'Axiforma Book',
      );
}
