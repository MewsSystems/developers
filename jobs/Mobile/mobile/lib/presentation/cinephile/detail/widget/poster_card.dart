import 'package:flutter/material.dart';
import 'package:mobile/domain/search/model/movie/movie.dart';

class DetailPosterCard extends StatefulWidget {
  final Movie movie;

  const DetailPosterCard(this.movie, {Key? key}) : super(key: key);

  @override
  State<StatefulWidget> createState() {
    return DetailPosterCardState();
  }
}

class DetailPosterCardState extends State<DetailPosterCard> {
  @override
  Widget build(BuildContext context) {
    TextTheme theme = Theme.of(context).textTheme;

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: <Widget>[
        Text('${widget.movie.releaseDate} '),
        const SizedBox(
          height: 20,
        ),
      ],
    );
  }
}
