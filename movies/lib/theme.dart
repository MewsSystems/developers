import 'package:flutter/material.dart';

const chipColor = Color(0xFF001B29);
const bgColor = Color(0xFF000D14);
const accentColor = Color(0xFFffe695);

final themeData = ThemeData(
  scaffoldBackgroundColor: bgColor,
  colorScheme: const ColorScheme.dark(
    primary: accentColor,
    secondary: accentColor,
    background: bgColor,
  ),
);