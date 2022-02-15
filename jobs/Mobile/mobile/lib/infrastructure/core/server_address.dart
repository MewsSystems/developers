class ServerAddress {
  const ServerAddress();
  static const String _address = "https://api.themoviedb.org";
  static const String _apiVer = "3";
  static const String _function = 'search/movie';
  String get relevant => "$_address/$_apiVer/$_function/";
  String get apiKey => '36f7cc5d234b2643857ef13954f96f94';
  String get baseUrl => 'https://image.tmdb.org/t/p/w500';

  /// Provides a backUp Image url if api posterPath is null.
  String get backUpImage => 'https://picsum.photos/250?image=9';
}
