import 'package:flutter/material.dart';
import 'package:mobile/domain/search/model/movie/movie.dart';
import 'package:mobile/presentation/core/theming/size_config.dart';

class HeroPreview extends StatelessWidget {
  const HeroPreview({
    Key? key,
    required this.animation,
    required this.flightDirection,
    required this.movie,
  }) : super(key: key);

  final Animation animation;
  final HeroFlightDirection flightDirection;
  final Movie movie;

  @override
  Widget build(BuildContext context) {
    final isPop = flightDirection == HeroFlightDirection.pop;
    return AnimatedBuilder(
        animation: animation,
        builder: (context, child) {
          final value = isPop
              ? Curves.easeInBack.transform(animation.value)
              : Curves.ease.transform(animation.value);
          return SizedBox(
              width: (1.defaultHeight()) * (1 + (0.8 * (value))),
              child: ClipRRect(
                  borderRadius: const BorderRadius.all(Radius.circular(5.0)),
                  child: FadeInImage.assetNetwork(
                    image: movie.posterPath,
                    fit: BoxFit.fill,
                    placeholder: 'assets/img.png',
                  )));
        });
  }
}
