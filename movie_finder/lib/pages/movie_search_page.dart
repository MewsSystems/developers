import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_finder/components/movie_tile.dart';
import 'package:movie_finder/data/model/movies/movie.dart';
import 'package:movie_finder/pages/movie_details_page.dart';
import 'package:movie_finder/utils/constants.dart';
import 'package:movie_finder/widgets/error_text.dart';
import 'package:movie_finder/widgets/search_input_field.dart';

import '../bloc/movies_search_bloc/movies_search_bloc.dart';
import '../widgets/custom_app_bar.dart';

class MovieSearchPage extends StatefulWidget {
  const MovieSearchPage({Key? key}) : super(key: key);
  static final route = '\movie-details-page';

  @override
  _MovieSearchPageState createState() => _MovieSearchPageState();
}

class _MovieSearchPageState extends State<MovieSearchPage> {
  final ScrollController _scrollController = ScrollController();
  late final bloc = context.read<MoviesSearchBloc>();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: _buildAppBar(context),
      body: _buildBody(),
    );
  }

  CustomAppBar _buildAppBar(BuildContext context) => CustomAppBar(
        title: 'Movies finder',
        context: context,
      );

  Widget _buildBody() {
    return SafeArea(
      child: Column(
        children: [
          kVertical16,
          _buildSearchInput(),
          kVertical16,
          BlocConsumer<MoviesSearchBloc, MoviesSearchState>(
            listener: (context, state) {
              if (state is MoviesSearchStateLoading) {
                final text = 'Loading... ';

                _showLoadingSnackBar(text: text, includeLoadingIndicator: true);
              } else if (state is MoviesSearchStateSuccess || state is MoviesSearchStateError) {
                ScaffoldMessenger.of(context).hideCurrentSnackBar();
              } else if (state is MoviesSearchStateNoMorePages) {
                _showLoadingSnackBar(text: 'There is no more movies that match your search criteria');
              }
            },
            builder: (context, state) {
              if (state is MoviesSearchStateInitial) {
                return buildInitialView();
              } else if (state is MoviesSearchStateLoading && bloc.movies.value.isEmpty) {
                return _buildLoadingView();
              } else if (state is MoviesSearchStateError) {
                return _buildErrorView(state);
              } else {
                return _buildView();
              }
            },
          ),
        ],
      ),
    );
  }

  ScaffoldFeatureController<SnackBar, SnackBarClosedReason> _showLoadingSnackBar({
    required String text,
    bool? includeLoadingIndicator,
  }) {
    return ScaffoldMessenger.of(context).showSnackBar(
      _buildSnackBar(
        text: text,
        includeLoadingIndicator: includeLoadingIndicator,
      ),
    );
  }

  SnackBar _buildSnackBar({
    required String text,
    bool? includeLoadingIndicator,
  }) {
    return SnackBar(
      content: Row(
        children: [
          Expanded(
              child: Text(
            text,
            style: Theme.of(context).textTheme.bodySmall!.copyWith(
                  color: Colors.white,
                ),
          )),
          kHorizontal8,
          if (includeLoadingIndicator == true) ...[
            SizedBox(
              height: 16,
              width: 16,
              child: CircularProgressIndicator(),
            ),
          ],
        ],
      ),
    );
  }

  Widget _buildErrorView(MoviesSearchStateError state) {
    return Expanded(
      child: Center(
        child: ErrorText(error: state.error),
      ),
    );
  }

  Widget buildInitialView() {
    return SizedBox.shrink();
  }

  Widget _buildSearchInput() {
    return SearchInputField(
      onChanged: bloc.movieQuery.add,
      hintText: 'Type movie name to search',
    );
  }

  Widget _buildLoadingView() {
    return Expanded(
      child: Center(
        child: CircularProgressIndicator(),
      ),
    );
  }

  Widget _buildView() {
    final _movies = bloc.movies.value;

    if (_movies.isEmpty) {
      return _buildNoResultView();
    }

    return Expanded(
      child: ListView.separated(
        padding: EdgeInsets.symmetric(horizontal: 16),
        controller: _scrollController
          ..addListener(
            () {
              if (_scrollController.offset == _scrollController.position.maxScrollExtent &&
                  bloc.state is! MoviesSearchStateLoading &&
                  bloc.state is! MoviesSearchStateNoMorePages) {
                bloc.incrementPage();
                bloc.requestFetch();
              }
            },
          ),
        itemBuilder: (context, index) {
          final _movie = _movies[index];
          return _buildMovieTile(_movie, context);
        },
        separatorBuilder: (context, index) => kVertical16,
        itemCount: _movies.length,
      ),
    );
  }

  Widget _buildMovieTile(
    Movie _movie,
    BuildContext context,
  ) {
    return MovieTile(
      movie: _movie,
      onPressed: () {
        Navigator.of(context).pushNamed(
          MovieDetailsPage.route,
          arguments: _movie,
        );
      },
    );
  }

  Widget _buildNoResultView() {
    return Expanded(
      child: Center(
        child: Text(
          'Sorry there is no movies matching your search criteria',
          style: Theme.of(context).textTheme.bodyText2,
        ),
      ),
    );
  }
}
