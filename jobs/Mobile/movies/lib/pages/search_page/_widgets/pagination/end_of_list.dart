import 'package:flutter/material.dart';

class EndOfList extends StatelessWidget {
  const EndOfList({super.key});

  @override
  Widget build(BuildContext context) => const SafeArea(
        child: SizedBox(
          height: 150.0,
          child: Center(
            child: Text('No more results'),
          ),
        ),
      );
}
