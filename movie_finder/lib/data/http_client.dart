import 'package:http/http.dart' as http;

class HttpClient {
  Future<http.Response> get(
    Uri uri, {
    Map<String, String>? headers,
  }) async {
    return http.get(
      uri,
      headers: headers,
    );
  }
}
