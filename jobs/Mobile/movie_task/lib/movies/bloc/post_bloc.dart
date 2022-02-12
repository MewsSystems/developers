import 'dart:async';
import 'dart:convert';
import 'dart:developer';

import 'package:bloc/bloc.dart';
import 'package:bloc_concurrency/bloc_concurrency.dart';
import 'package:equatable/equatable.dart';
import 'package:http/http.dart' as http;
import 'package:movie_task/movies/movies.dart';
import 'package:stream_transform/stream_transform.dart';

part 'post_event.dart';
part 'post_state.dart';

const throttleDuration = Duration(milliseconds: 100);

EventTransformer<E> throttleDroppable<E>(Duration duration) =>
    (events, mapper) => droppable<E>().call(events.throttle(duration), mapper);

class PostBloc extends Bloc<PostEvent, PostState> {
  PostBloc({required this.httpClient}) : super(const PostState()) {
    on<PostFetched>(
      _onPostFetched,
      transformer: throttleDroppable(throttleDuration),
    );
  }

  final http.Client httpClient;
  int currentPage = 1;
  int totalPages = 1;
  String currentQuery = '';

  Future<void> _onPostFetched(
    PostFetched event,
    Emitter<PostState> emit,
  ) async {
    final String query = event.query;
    bool clearList = false;
    if (query.isNotEmpty) {
      if (query.compareTo(currentQuery) != 0) {
        currentQuery = event.query;
        currentPage = 1;
        totalPages = 1;
        clearList = true;
      }
    }
    if (state.hasReachedMax) return;
    if (query.isEmpty) return;
    try {
      if (state.status == PostStatus.initial) {
        final posts = await _fetchPosts();

        return emit(
          state.copyWith(
            status: PostStatus.success,
            posts: posts,
            hasReachedMax: false,
          ),
        );
      }
      if (currentPage > totalPages) {
        emit(state.copyWith(hasReachedMax: true));
      } else {
        final posts = await _fetchPosts(currentPage);
        if (clearList) {
          emit(
            state.copyWith(
              status: PostStatus.success,
              posts: posts,
              hasReachedMax: false,
            ),
          );
        } else {
          emit(
            state.copyWith(
              status: PostStatus.success,
              posts: List.of(state.posts)..addAll(posts),
              hasReachedMax: false,
            ),
          );
        }
      }
    } on Exception catch (_) {
      emit(state.copyWith(status: PostStatus.failure));
    }
  }

  Future<List<Movie>> _fetchPosts([int page = 1]) async {
    final response = await httpClient.get(
      Uri.https(
        'api.themoviedb.org',
        '/4/search/movie',
        <String, String>{
          'api_key': '03b8572954325680265531140190fd2a',
          'query': currentQuery,
          'page': page.toString(),
        },
      ),
    );
    log('response');
    log(response.body.toString());
    if (response.statusCode == 200) {
      currentPage++;
      final Map<dynamic, dynamic> body = json.decode(response.body) as Map;
      totalPages = body['total_pages'] as int;
      final List<dynamic> movieList = body['results'] as List;

      return movieList.map((dynamic json) {
        final Map<dynamic, dynamic> jsonMap = json as Map;

        return Movie(
          id: jsonMap['id'] as int,
          title: jsonMap['title'] as String,
          body: jsonMap['overview'] as String,
        );
      }).toList();
    }
    throw Exception('error fetching posts');
  }
}
