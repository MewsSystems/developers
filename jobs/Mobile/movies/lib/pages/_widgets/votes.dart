import 'package:flutter/material.dart';

class Votes extends StatelessWidget {
  const Votes({super.key, required this.count});

  final int count;

  @override
  Widget build(BuildContext context) => Text(
        'Votes: $count',
        style: const TextStyle(fontSize: 12.0),
      );
}
