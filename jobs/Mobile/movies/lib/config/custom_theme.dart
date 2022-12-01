import 'package:flutter/material.dart';

class CustomTheme {
  static const Color grey = Color.fromRGBO(231, 233, 234, 1);
  static const Color black = Color.fromRGBO(28, 33, 47, 1);
  static const Color blue = Color.fromRGBO(47, 56, 81, 1);
  static const Color darkBlue = Color.fromARGB(255, 10, 24, 44);

  static const borderRadius = BorderRadius.all(Radius.circular(12.0));

  static final darkTheme = ThemeData(
    scaffoldBackgroundColor: black,
    textTheme: const TextTheme(
      bodyText1: TextStyle(),
      bodyText2: TextStyle(),
      subtitle1: TextStyle(),
    ).apply(
      bodyColor: grey,
    ),
    appBarTheme: const AppBarTheme(
      backgroundColor: black,
      foregroundColor: grey,
      titleTextStyle: TextStyle(
        fontWeight: FontWeight.w500,
        fontSize: 32.0,
      ),
      iconTheme: IconThemeData(
        color: grey,
      ),
      elevation: .0,
    ),
    inputDecorationTheme: const InputDecorationTheme(
      filled: true,
      fillColor: blue,
      border: OutlineInputBorder(
        borderSide: BorderSide(color: darkBlue),
        borderRadius: borderRadius,
      ),
      enabledBorder: OutlineInputBorder(
        borderSide: BorderSide(color: darkBlue),
        borderRadius: borderRadius,
      ),
      focusedBorder: OutlineInputBorder(
        borderSide: BorderSide(color: darkBlue),
        borderRadius: borderRadius,
      ),
      labelStyle: TextStyle(
        color: CustomTheme.grey,
      ),
      hintStyle: TextStyle(
        color: CustomTheme.grey,
      ),
    ),
    iconTheme: const IconThemeData(color: grey),
    colorScheme: const ColorScheme.light().copyWith(
      primary: grey,
      secondary: grey,
    ),
  );
}
