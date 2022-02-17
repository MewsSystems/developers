import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_finder/utils/constants.dart';
import 'package:movie_finder/widgets/custom_app_bar.dart';
import 'package:movie_finder/widgets/error_text.dart';

import '../bloc/movie_details_bloc/movie_details_bloc.dart';
import '../components/movie_details_mixin.dart';
import '../data/model/movies/movie.dart';
import '../data/model/movies/movie_details_result.dart';

class MovieDetailsPage extends StatefulWidget {
  const MovieDetailsPage({Key? key}) : super(key: key);
  static final route = '\movie-details-page';

  @override
  State<MovieDetailsPage> createState() => _MovieDetailsPageState();
}

class _MovieDetailsPageState extends State<MovieDetailsPage> with MovieDetailsMixin {
  late final bloc = context.read<MovieDetailsBloc>();

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance!.addPostFrameCallback((_) {
      final movie = ModalRoute.of(context)?.settings.arguments as Movie;

      bloc.add(MovieDetailsEvent.fetchRequested(movie.id!));
    });
  }

  @override
  Widget build(BuildContext context) {
    final movie = ModalRoute.of(context)?.settings.arguments as Movie;

    return Scaffold(
      appBar: CustomAppBar(
        context: context,
        title: movie.title ?? 'Movie Details',
        displayBackButton: true,
      ),
      body: _buildBody(),
    );
  }

  Widget _buildBody() {
    return BlocConsumer<MovieDetailsBloc, MovieDetailsState>(
      listener: (context, state) {},
      builder: (context, state) {
        if (state is MovieDetailsStateInitial) {
          return _buildInitialView();
        } else if (state is MovieDetailsStateLoading) {
          return _buildLoadingView();
        } else if (state is MovieDetailsStateError) {
          return _buildErrorView(state);
        } else if (state is MovieDetailsStateSuccess) {
          return _buildView(state.movieDetailsResult);
        } else {
          return SizedBox.shrink();
        }
      },
    );
  }

  SizedBox _buildInitialView() => SizedBox.shrink();

  Widget _buildLoadingView() {
    return Center(
      child: CircularProgressIndicator(),
    );
  }

  Widget _buildErrorView(MovieDetailsStateError state) {
    return Center(
      child: ErrorText(error: state.error),
    );
  }

  Widget _buildView(MovieDetailsResult details) {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          if (details.posterPath != null)
            buildImage(
              posterPath: details.posterPath!,
            ),
          ...[
            kVertical16,
            buildTitle(
              context,
              title: details.title ?? 'Dummy Title',
            ),
            if (details.releaseDate != null && details.releaseDate!.isNotEmpty) ...[
              kVertical8,
              buildReleaseDate(
                context,
                releaseDate: details.releaseDate!,
              ),
            ],
            if (details.revenue != null && details.revenue != 0) ...[
              kVertical8,
              _buildRevenue(
                details.revenue!,
              ),
            ],
            if (details.overview != null && details.overview!.isNotEmpty) ...[
              kVertical16,
              buildOverview(
                context,
                overview: details.overview!,
                maxLines: 32,
              ),
            ],
          ].map(
            (e) => Padding(
              padding: EdgeInsets.symmetric(
                horizontal: 16,
              ),
              child: e,
            ),
          ),
          kVertical60,
        ],
      ),
    );
  }

  Widget _buildRevenue(int revenue) {
    return Text(
      'Revenue: ${revenue.toString()}',
      style: Theme.of(context).textTheme.bodySmall,
      textHeightBehavior: TextHeightBehavior(
        applyHeightToFirstAscent: false,
        applyHeightToLastDescent: false,
      ),
    );
  }
}
