import 'package:flutter/material.dart';

class NoMovies extends StatelessWidget {
  const NoMovies({super.key});

  @override
  Widget build(BuildContext context) => const SafeArea(
        child: SizedBox(
          height: 150.0,
          child: Center(
            child: Text('This movie does not exists'),
          ),
        ),
      );
}
