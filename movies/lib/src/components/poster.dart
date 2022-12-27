import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:movies/theme.dart';

/// Shows a movie poster using its [url]
class Poster extends StatelessWidget {
  const Poster({
    Key? key,
    required this.url,
    required this.heroTag,
    this.width,
    this.height,
  }) : super(key: key);

  final String url;
  final int heroTag;
  final double? width;
  final double? height;

  @override
  Widget build(BuildContext context) {
    Widget result;
    if (url.isEmpty) {
      result = AspectRatio(
        aspectRatio: 2 / 3,
        child: ClipRRect(
          borderRadius: const BorderRadius.all(Radius.circular(16)),
          child: Container(
            color: chipColor,
            alignment: Alignment.center,
            child: const Icon(Icons.broken_image),
          ),
        ),
      );
    } else {
      result = ClipRRect(
        borderRadius: const BorderRadius.all(Radius.circular(16)),
        child: Hero(
          tag: heroTag,
          child: CachedNetworkImage(
            imageUrl: url,
          ),
        ),
      );
    }

    return SizedBox(
      width: width,
      height: height,
      child: result,
    );
  }
}
