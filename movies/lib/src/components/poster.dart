import 'package:flutter/material.dart';
import 'package:cached_network_image/cached_network_image.dart';

class Poster extends StatelessWidget {
  const Poster(
      {Key? key,
      required this.url,
      required this.heroTag,
      this.width,
      this.height})
      : super(key: key);

  final String url;
  final int heroTag;
  final double? width;
  final double? height;

  @override
  Widget build(BuildContext context) {
    Widget result;
    if (url.isEmpty) {
      result = const AspectRatio(
        aspectRatio: 4 / 6,
        child: Placeholder(),
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
