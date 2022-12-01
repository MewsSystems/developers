import 'package:flutter/material.dart';
import 'package:movies/config/custom_theme.dart';
import 'package:movies/pages/_widgets/loader.dart';

class NetworkImageLoader extends StatelessWidget {
  const NetworkImageLoader({
    super.key,
    required this.path,
  });

  final String path;

  @override
  Widget build(BuildContext context) => SizedBox(
        child: Image.network(
          path,
          fit: BoxFit.cover,
          frameBuilder: (context, child, frame, _) {
            if (frame == null) {
              return const DecoratedBox(
                decoration: BoxDecoration(
                  color: CustomTheme.blue,
                ),
                child: Loader(),
              );
            }

            return child;
          },
        ),
      );
}
