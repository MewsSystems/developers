import 'package:flutter/material.dart';
import 'package:mobile/domain/search/model/movie/movie.dart';
import 'package:mobile/presentation/core/theming/colors.dart';
import 'package:mobile/presentation/core/theming/size_config.dart';

class MovieInfoView extends StatelessWidget {
  final Movie movie;
  const MovieInfoView({Key? key, required this.movie}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      alignment: Alignment.centerLeft,
      padding: EdgeInsets.only(left: 20.0.w()),
      height: 150.0.h(),
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: <Widget>[
          Row(
            children: <Widget>[
              Text(
                movie.title,
                maxLines: 3,
                softWrap: true,
                overflow: TextOverflow.ellipsis,
                style: const TextStyle(color: Colors.white),
              ),
              SizedBox(
                width: 10.w(),
              ),
            ],
          ),
          const SizedBox(
            height: 5.0,
          ),
          Text(
            movie.releaseDate,
            style: TextStyle(
              color: CinephileColors.white,
              fontSize: 18.0,
            ),
            textAlign: TextAlign.left,
            overflow: TextOverflow.ellipsis,
          ),
          Row(
            children: <Widget>[
              Icon(
                Icons.stars,
                color: CinephileColors.white,
              ),
              const SizedBox(
                width: 5.0,
              ),
              Text(
                movie.voteAverage.toString(),
                style: TextStyle(
                  color: CinephileColors.white,
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }
}
