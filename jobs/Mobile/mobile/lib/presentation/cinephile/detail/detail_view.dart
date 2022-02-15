import 'package:flutter/material.dart';
import 'package:mobile/domain/search/model/movie/movie.dart';
import 'package:mobile/presentation/cinephile/detail/widget/poster_card.dart';
import 'package:mobile/presentation/core/heros/hero_id.dart';
import 'package:mobile/presentation/core/theming/colors.dart';
import 'package:mobile/presentation/core/theming/size_config.dart';
import 'package:mobile/presentation/cinephile/detail/widget/custom_back_button.dart';

class Detail extends StatefulWidget {
  final Movie movie;

  const Detail({Key? key, required this.movie}) : super(key: key);

  @override
  State<StatefulWidget> createState() {
    return _DetailState();
  }
}

class _DetailState extends State<Detail> {
  @override
  Widget build(BuildContext context) {
    return SafeArea(
      child: Scaffold(
        backgroundColor: Colors.black,
        body: Stack(
          children: <Widget>[
            ShaderMask(
              shaderCallback: (rect) {
                return const LinearGradient(
                  begin: Alignment.topCenter,
                  end: Alignment.bottomCenter,
                  colors: [Colors.black, Colors.transparent],
                ).createShader(Rect.fromLTRB(0, 0, rect.width, rect.height));
              },
              blendMode: BlendMode.dstIn,
              child: Hero(
                tag: HeroTag.make(
                    id: widget.movie.id, title: widget.movie.title),
                child: FadeInImage(
                  width: double.infinity,
                  height: 0.8.defaultHeight(),
                  image: NetworkImage(widget.movie.posterPath),
                  fit: BoxFit.cover,
                  placeholder: const AssetImage('assets/img.png'),
                ),
              ),
            ),
            SingleChildScrollView(
              physics: const BouncingScrollPhysics(),
              child: Padding(
                  padding: EdgeInsets.only(
                    top: 0.8.defaultHeight(),
                  ),
                  child: Container(
                    padding: const EdgeInsets.only(left: 20, top: 20),
                    color: CinephileColors.mainColor,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: <Widget>[
                        Text(
                          widget.movie.title,
                          style: Theme.of(context).textTheme.bodyText2,
                        ),
                        SizedBox(
                          height: 10.h(),
                        ),
                        DetailPosterCard(widget.movie),
                        SizedBox(
                          height: 20.h(),
                        ),
                        Text(widget.movie.overview,
                            style: Theme.of(context).textTheme.bodyText2),
                        SizedBox(
                          height: 20.h(),
                        ),
                      ],
                    ),
                  )),
            ),
            const Padding(
                padding: EdgeInsets.only(left: 5, top: 10),
                child: CustomBackButton()),
          ],
        ),
      ),
    );
  }
}
