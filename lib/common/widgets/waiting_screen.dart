import 'package:flutter/material.dart';

class WaitingScreen extends StatelessWidget {
  const WaitingScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Expanded(flex: 2, child: Image.asset('assets/images/popcorn.png')),
        const Expanded(
          child: Center(
            child: CircularProgressIndicator(),
          ),
        )
      ],
    );
  }
}
