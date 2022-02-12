part of 'post_bloc.dart';

enum PostStatus { initial, success, failure }

class PostState extends Equatable {
  const PostState({
    this.status = PostStatus.initial,
    this.posts = const <Movie>[],
    this.hasReachedMax = false,
    this.queryText = '',
  });

  final PostStatus status;
  final List<Movie> posts;
  final bool hasReachedMax;
  final String queryText;

  PostState copyWith({
    PostStatus? status,
    List<Movie>? posts,
    bool? hasReachedMax,
    String? queryText,
  }) =>
      PostState(
        status: status ?? this.status,
        posts: posts ?? this.posts,
        hasReachedMax: hasReachedMax ?? this.hasReachedMax,
        queryText: queryText ?? this.queryText,
      );

  @override
  String toString() =>
      '''PostState { status: $status, hasReachedMax: $hasReachedMax, posts: ${posts.length} }''';

  @override
  List<Object> get props => [status, posts, hasReachedMax];
}
