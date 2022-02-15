import 'package:flutter/material.dart';
import 'package:mobile/domain/search/model/movie/movie.dart';
import 'package:mobile/presentation/core/heros/hero_id.dart';
import 'package:mobile/presentation/core/heros/hero_poster_card.dart';

class Poster extends StatelessWidget {
  final Movie movie;
  const Poster({Key? key, required this.movie}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: 150.0,
      width: 100.0,
      child: Hero(
        tag: HeroTag.make(id: movie.id, title: movie.title),
        flightShuttleBuilder: (flightContext, animation, flightDirection,
            fromHeroContext, toHeroContext) {
          return HeroPreview(
            flightDirection: flightDirection,
            animation: animation,
            movie: movie,
          );
        },
        child: ClipRRect(
            borderRadius: const BorderRadius.all(Radius.circular(5.0)),
            child: FadeInImage.assetNetwork(
              image: movie.posterPath,
              fit: BoxFit.fill,
              placeholder: 'assets/img.png',
            )),
      ),
    );
  }
}
