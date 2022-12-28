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
        height: 100,
      );
    }
    return CachedNetworkImage(
      imageUrl: imageUrl!,
      fit: BoxFit.fill,
      height: height,
      errorWidget: (context, url, error) =>
          Image.asset('assets/images/image_not_loaded.jpeg'),
      progressIndicatorBuilder: (context, url, progress) => Center(
        child: CircularProgressIndicator(
          value: progress.progress,
          color: Colors.black,
        ),
      ),
    );
  }
}
