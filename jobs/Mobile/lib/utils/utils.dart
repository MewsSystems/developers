import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';

class Utils {
  static Widget getImage(String? path) => path != null
      ? CachedNetworkImage(
          imageUrl: path,
          placeholder: (context, url) => const CircularProgressIndicator(),
          //ignore: implicit_dynamic_parameter
          errorWidget: (context, url, error) =>
              const Icon(Icons.image_not_supported),
        )
      : const SizedBox.shrink();
}
