import 'package:flutter/material.dart';

class MovieLoading extends StatelessWidget {
  const MovieLoading({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    return Column(
      mainAxisSize: MainAxisSize.min,
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        const Text('🍿', style: TextStyle(fontSize: 64)),
        Text(
          'Loading Move',
          style: theme.textTheme.headline5,
        ),
        const Padding(
          padding: EdgeInsets.all(16.0),
          child: CircularProgressIndicator(),
        ),
      ],
    );
  }
}