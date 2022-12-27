import 'package:flutter/material.dart';

class Spinner extends StatelessWidget {
  final bool isSpinning;
  final Widget child;

  const Spinner({
    required this.child,
    this.isSpinning = false,
    Key? key,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Stack(children: [
      child,
      if (isSpinning)
        Container(
          color: Colors.black.withAlpha(80),
          child: const Center(
            child: CircularProgressIndicator(),
          ),
        ),
    ]);
  }
}
