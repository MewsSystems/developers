import 'package:flutter/material.dart';


///Used to create Fade Transistions.
///
///usecase
///```dart
/// Navigator.push(
///   context,
///   FadeRoute(
///      transitionTime: const Duration(milliseconds: 1700),
///      reverseTransitionTime: const Duration(milliseconds: 1700),
///      page: NextPage()));
///```
///```dart
///Navigator.pop()
///```
///would make use of this Transistion.
class FadeRoute extends PageRouteBuilder {
  final Duration transitionTime;
  final Duration reverseTransitionTime;
  final Widget page;
  FadeRoute(
      {required this.page,
      required this.transitionTime,
      required this.reverseTransitionTime})
      : super(
          transitionDuration: transitionTime,
          reverseTransitionDuration: reverseTransitionTime,
          pageBuilder: (
            BuildContext context,
            Animation<double> animation,
            Animation<double> secondaryAnimation,
          ) =>
              page,
          transitionsBuilder: (
            BuildContext context,
            Animation<double> animation,
            Animation<double> secondaryAnimation,
            Widget child,
          ) =>
              FadeTransition(
            opacity: animation,
            child: child,
          ),
        );
}
