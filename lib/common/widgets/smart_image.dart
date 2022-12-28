import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';

class SmartImage extends StatelessWidget {
  final String? imageUrl;
  final double? height;
  const SmartImage({this.imageUrl, this.height, Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    if (imageUrl == null) {
      return Image.asset(
        'assets/images/image_not_available.png',
        height: height,
      );
    }
    return CachedNetworkImage(
      imageUrl: imageUrl!,
      fit: BoxFit.fill,
      height: height,
      progressIndicatorBuilder: (context, url, progress) => Center(
        child: CircularProgressIndicator(value: progress.progress),
      ),
    );
  }
}
