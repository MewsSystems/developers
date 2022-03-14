part of 'search_bloc.dart';

class SearchEvent {
  SearchEvent({
    required this.query,
    this.page = 1,
  });

  final String query;
  int page;
}

class NewSearchEvent extends SearchEvent {
  NewSearchEvent({required String query}) : super(query: query);
}

class NextPageSearchEvent extends SearchEvent {
  NextPageSearchEvent({
    required String query,
    required int page,
  }) : super(
          query: query,
          page: page,
        );
}

class ResetSearchEvent extends SearchEvent {
  ResetSearchEvent({required String query}) : super(query: query);
}
