import 'package:flutter/material.dart';
import 'package:mobile/presentation/core/theming/colors.dart';

class AppTheme {
  static ThemeData darkTheme = _buildDarkAppTheme();

  static ThemeData _buildDarkAppTheme() {
    final ThemeData base = ThemeData.dark();
    return base.copyWith(
      textTheme: _darkTextTheme(base.textTheme),
      inputDecorationTheme:
          _darkInputDecorationTheme(base.inputDecorationTheme),
      appBarTheme: _darkAppBarTheme(base.appBarTheme),
      textSelectionTheme: TextSelectionThemeData(
        cursorColor: CinephileColors.white,
      ),
    );
  }

  static _darkTextTheme(TextTheme baseTextTheme) {
    return baseTextTheme.copyWith(
      headline5: const TextStyle(
        fontFamily: 'Manrope',
        fontWeight: FontWeight.w600,
        fontSize: 28,
      ),
      subtitle1: const TextStyle(
        fontFamily: 'Manrope',
        fontWeight: FontWeight.w600,
        fontSize: 17,
      ),
      caption: TextStyle(
          fontFamily: 'Manrope',
          fontWeight: FontWeight.w600,
          fontSize: 12,
          color: CinephileColors.white),
    );
  }

  static _darkInputDecorationTheme(InputDecorationTheme inputDecorationTheme) {
    return inputDecorationTheme.copyWith(
        hintStyle: TextStyle(
            fontWeight: FontWeight.w600,
            fontSize: 12,
            color: CinephileColors.backGrey),
        focusColor: Colors.black);
  }

  static _darkAppBarTheme(AppBarTheme appBarTheme) {
    return appBarTheme.copyWith(
      backgroundColor: CinephileColors.white,
      elevation: 0,
      centerTitle: true,
      titleTextStyle: TextStyle(
          fontFamily: 'Manrope',
          fontWeight: FontWeight.w600,
          fontSize: 17,
          color: CinephileColors.mainColor),
    );
  }
}
