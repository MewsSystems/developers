import 'package:flutter/material.dart';
import 'package:movies/config/custom_theme.dart';
import 'package:movies/pages/search_page/_widgets/loader.dart';

class NetworkImageLoader extends StatelessWidget {
  const NetworkImageLoader({
    super.key,
    required this.height,
    required this.width,
    required this.path,
  });

  final double height;
  final double width;
  final String path;

  @override
  Widget build(BuildContext context) => SizedBox(
        height: height,
        width: width,
        child: ClipRRect(
          borderRadius: const BorderRadius.all(Radius.circular(8.0)),
          child: Image.network(
            path,
            height: 100,
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
        ),
      );
}
