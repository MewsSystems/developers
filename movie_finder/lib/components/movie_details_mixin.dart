import 'package:flutter/material.dart';

mixin MovieDetailsMixin {
  Widget buildImage({
    required String posterPath,
    double? height,
    BoxFit? boxFit,
  }) {
    final _prefix = 'https://image.tmdb.org/t/p/original';
    final _url = _prefix + posterPath;
    return SizedBox(
      child: Image.network(
        _url,
        fit: boxFit,
      ),
      height: height,
    );
  }

  Widget buildReleaseDate(
    BuildContext context, {
    required String releaseDate,
  }) {
    return Text(
      '(${releaseDate.substring(0, 4)})',
      style: Theme.of(context).textTheme.bodySmall,
      textHeightBehavior: const TextHeightBehavior(
        applyHeightToFirstAscent: false,
        applyHeightToLastDescent: false,
      ),
    );
  }

  Widget buildTitle(BuildContext context, {required String title}) {
    return Text(
      title,
      style: Theme.of(context).textTheme.bodyText1,
      textHeightBehavior: TextHeightBehavior(
        applyHeightToFirstAscent: false,
        applyHeightToLastDescent: false,
      ),
    );
  }

  Widget buildOverview(BuildContext context, {required String overview, int? maxLines}) {
    return Text(
      overview,
      maxLines: maxLines,
      overflow: TextOverflow.ellipsis,
      style: Theme.of(context).textTheme.bodySmall,
      textHeightBehavior: const TextHeightBehavior(
        applyHeightToFirstAscent: false,
        applyHeightToLastDescent: false,
      ),
    );
  }
}
