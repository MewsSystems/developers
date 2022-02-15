/// Library to handle responsive layout.
library responsive_layout;

import 'package:flutter/material.dart';

extension ReponsiveSize on num {
  static const double _defaultSize = 21;
  static late MediaQueryData _mediaQueryData;
  static late double _pixelRatio,
      _screenWidth,
      _screenHeight,
      _widthInPx,
      _heightInPx;

  init(BuildContext context, {double? width, double? height}) {
    _mediaQueryData = MediaQuery.of(context);
    _pixelRatio = _mediaQueryData.devicePixelRatio;
    _screenWidth = _mediaQueryData.size.width;
    _screenHeight = _mediaQueryData.size.height;
    _heightInPx = height ?? 1920;
    _widthInPx = width ?? 1080;
  }

  double h() => (this / _heightInPx) * _screenHeight;
  double w() => (this / _widthInPx) * _screenWidth;
  double defaultHeight() => _screenHeight * this;
  double defaultWeight() => _screenWidth * this;
}

/// Wraper that handles responsive layout.
///
/// wrap root widget below [MaterialApp] with [Responsive] widget.
/// [screenHeight] and [screenWidth] refers to the height and width of the figma design.
/// A default value of 1920 and 1080 was set.
///
/// Api usage
///
/// ```dart
/// Container(
///  height: 30.h(),
///  width: 30.w())
///```
///to get a percentage of the default screen height
///```dart
///height: 1.defaultHeight(),
///```
///where the height return is the default height divided by [this].
///
class Responsive extends StatelessWidget {
  final Widget child;
  final double? screenWidth, screenHeight;
  const Responsive(
      {Key? key, required this.child, this.screenHeight, this.screenWidth})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    ReponsiveSize(0).init(context, width: screenWidth, height: screenHeight);

    return child;
  }
}
