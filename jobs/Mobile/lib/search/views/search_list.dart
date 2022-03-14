import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:mews_imdb/movie_details/movie_details.dart';
import 'package:mews_imdb/search/bloc/search_bloc.dart';
import 'package:mews_imdb/search/models/models.dart';
import 'package:mews_imdb/utils/utils.dart';
import 'package:movie_repository/movie_repository.dart'
    hide MoviePreview, SearchResult;

class SearchList extends StatefulWidget {
  const SearchList({Key? key}) : super(key: key);

  @override
  State<SearchList> createState() => _SearchListState();
}

class _SearchListState extends State<SearchList> {
  void _loadNext() {
    final SearchState state = context.read<SearchBloc>().state;
    if (state.status != SearchStatus.loading && !state.hasReachedMax) {
      context.read<SearchBloc>().add(
            NextPageSearchEvent(
              query: state.query,
              page: state.page + 1,
            ),
          );
    }
  }

  @override
  Widget build(BuildContext context) => BlocBuilder<SearchBloc, SearchState>(
        builder: (context, state) => Container(
          padding: const EdgeInsets.fromLTRB(5, 70, 5, 5),
          child: state.previews.isEmpty
              ? const Center(
                  child: Text('Start typing'),
                )
              : ListView.builder(
                  keyboardDismissBehavior:
                      ScrollViewKeyboardDismissBehavior.onDrag,
                  itemBuilder: (BuildContext context, int index) {
                    if (index == state.previews.length - 1) _loadNext();

                    return MovieListItem(
                      moviePreview: state.previews[index],
                    );
                  },
                  itemCount: state.previews.length,
                ),
        ),
      );
}

class MovieListItem extends StatelessWidget {
  const MovieListItem({
    Key? key,
    required this.moviePreview,
  }) : super(key: key);

  final MoviePreview moviePreview;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.all(5),
        child: Card(
          child: Column(
            children: <Widget>[
              ListTile(
                leading: Utils.getImage(moviePreview.posterPath),
                title: Text(
                  moviePreview.title,
                  style: const TextStyle(
                    fontSize: 24,
                    fontWeight: FontWeight.w600,
                  ),
                  maxLines: 2,
                  overflow: TextOverflow.ellipsis,
                ),
                subtitle: Text(_getReleaseYear(moviePreview.releaseDate)),
                trailing: Text('${moviePreview.voteAverage}/10'),
                onTap: () {
                  Navigator.of(context).push<void>(
                    MovieDetailsPage.route(
                      moviePreview.id,
                      context.read<MovieRepository>(),
                    ),
                  );
                },
              ),
            ],
          ),
        ),
      );

  String _getReleaseYear(DateTime? dateTime) =>
      dateTime == null ? '' : '(${dateTime.year})';
}
