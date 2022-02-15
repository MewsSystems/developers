import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:infinite_scroll_pagination/infinite_scroll_pagination.dart';
import 'package:mobile/application/core/enum.dart';
import 'package:mobile/application/search/search_bloc.dart';
import 'package:mobile/domain/search/model/movie/movie.dart';
import 'package:mobile/presentation/core/theming/colors.dart';
import 'package:mobile/presentation/core/theming/size_config.dart';
import 'package:mobile/presentation/cinephile/search/widgets/first_page_error_view.dart';
import 'package:mobile/presentation/cinephile/search/widgets/new_page_error_view.dart';
import 'package:mobile/presentation/cinephile/search/widgets/results.dart';

class SearchView extends StatefulWidget {
  const SearchView({Key? key}) : super(key: key);

  @override
  State<SearchView> createState() => _SearchViewState();
}

class _SearchViewState extends State<SearchView> {
  final TextEditingController searchController = TextEditingController();
  final PagingController<int, Movie> _pageController =
      PagingController(firstPageKey: 1, invisibleItemsThreshold: 1);
  String previousQuery = '';
  int nextPageKey = 1;

  @override
  void initState() {
    super.initState();
    searchController.addListener(() {
      searchListener();
    });

    _pageController.addPageRequestListener((pageKey) {
      pageListener();
    });
  }

  void pageListener() {
    nextPageKey = nextPageKey + 1;
    context.read<SearchBloc>().add(
        SearchEvent.loadMore(nextPageKey.toString(), searchController.text));
    previousQuery = searchController.text;
  }

  void searchListener() {
    if (previousQuery.trim() != searchController.text.trim()) {
      _pageController.itemList = null;
      context.read<SearchBloc>().add(SearchEvent.search(searchController.text));
    }
    previousQuery = searchController.text;
    setState(() {});
  }

  @override
  void dispose() {
    super.dispose();
    _pageController.dispose();
    searchController.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.black,
      appBar: AppBar(
        backgroundColor: CinephileColors.scaffoldDarkBack,
        automaticallyImplyLeading: false,
        title: TextFormField(
          style: TextStyle(
            color: CinephileColors.white,
          ),
          autofocus: true,
          controller: searchController,
          decoration: InputDecoration(
            filled: true,
            hintText: 'Search Movie',
            hintStyle: TextStyle(
              color: CinephileColors.backGrey,
            ),
            prefixIcon: Icon(
              Icons.search,
              size: 28.0.h(),
            ),
            suffix: IconButton(
              color: CinephileColors.white,
              icon: Icon(
                Icons.clear,
                color: CinephileColors.white,
              ),
              onPressed: () {
                searchController.clear();
              },
            ),
          ),
        ),
      ),
      body: RefreshIndicator(
          onRefresh: () => Future.sync(
                () {
                  _pageController.itemList = null;
                  context
                      .read<SearchBloc>()
                      .add(SearchEvent.search(searchController.text));
                },
              ),
          child: (searchController.text == '' || searchController.text.isEmpty)
              ? const Center(
                  child: Text('Search for Your Favorite Movie.'),
                )
              : BlocConsumer<SearchBloc, SearchState>(
                  listener: (context, state) {
                  if (append(state).status == Status.success) {
                    determineAppend(state);
                  } else if (state.status == Status.failed) {
                    _pageController.error = state.err;
                  } else if (state.status == Status.loading) {}
                }, builder: (context, state) {
                  if (state.status == Status.loading) {
                    return const Center(
                        child: CircularProgressIndicator.adaptive());
                  }
                  return PagedListView.separated(
                    physics: const BouncingScrollPhysics(),
                    pagingController: _pageController,
                    separatorBuilder: (context, index) {
                      return SizedBox(
                        height: 26.h(),
                      );
                    },
                    padding: const EdgeInsets.all(16),
                    builderDelegate: PagedChildBuilderDelegate<Movie>(
                        itemBuilder: (context, item, index) {
                      return SearchResult(
                        movie: item,
                      );
                    }, newPageErrorIndicatorBuilder: (context) {
                      return NewPageErrorIndicator(
                        onTap: () {
                          _pageController.retryLastFailedRequest();
                        },
                        message: state.err,
                      );
                    }, firstPageErrorIndicatorBuilder: (context) {
                      return FirstPageErrorIndicator(
                        message: state.err,
                        onTryAgain: () {
                          _pageController.itemList = null;
                          context
                              .read<SearchBloc>()
                              .add(SearchEvent.search(searchController.text));
                        },
                      );
                    }),
                  );
                })),
    );
  }

  SearchState append(SearchState state) => state;

  void determineAppend(SearchState state) {
    if (state.lastPage > (_pageController.nextPageKey ?? 0 + 1)) {
      _pageController.appendPage(state.result, _pageController.nextPageKey);
    } else {
      _pageController.appendLastPage(state.result);
    }
  }
}
