import 'package:flutter/material.dart';

class DetailInfoCell extends StatelessWidget {
  const DetailInfoCell({super.key, required this.title, required this.text});

  final String title;
  final String text;

  @override
  Widget build(BuildContext context) => Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text('$title:'),
          const SizedBox(height: 2.0),
          Text(text),
          const SizedBox(height: 8.0),
        ],
      );
}
