import 'package:mobile/infrastructure/core/server_address.dart';

final String _address = const ServerAddress().relevant;
final String _apiKey = const ServerAddress().apiKey;

class Endpoints {
  static _Query get query => _Query();
}

class _Query {
  String search(String query, String pages) =>
      _address +
      '?api_key=$_apiKey&language=en-'
          'US&query=$query&page=$pages&include_adult=false';
}
