import 'package:flutter/material.dart';
import 'package:mobile/domain/search/model/movie/movie.dart';
import 'package:mobile/presentation/cinephile/search/widgets/movie_info_view.dart';
import 'package:mobile/presentation/cinephile/search/widgets/poster.dart';
import 'package:mobile/presentation/core/routes/fade_transition.dart';
import 'package:mobile/presentation/core/theming/colors.dart';
import 'package:mobile/presentation/cinephile/detail/detail_view.dart';

class SearchResult extends StatelessWidget {
  final Movie movie;
  const SearchResult({Key? key, required this.movie}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
        Navigator.push(
            context,
            FadeRoute(
                transitionTime: const Duration(milliseconds: 1700),
                reverseTransitionTime: const Duration(milliseconds: 1700),
                page: Detail(
                  movie: movie,
                )));
      },
      child: Padding(
        padding: const EdgeInsets.only(left: 10.0),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          crossAxisAlignment: CrossAxisAlignment.center,
          children: <Widget>[
            Row(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: <Widget>[
                Poster(movie: movie),
                MovieInfoView(movie: movie),
              ],
            ),
            Divider(
              color: CinephileColors.white,
            ),
          ],
        ),
      ),
    );
  }
}
