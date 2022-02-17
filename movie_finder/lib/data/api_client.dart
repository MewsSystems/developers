import 'dart:async';
import 'dart:convert';

import 'package:http/http.dart' as http;
import 'package:movie_finder/data/config.dart';

import 'http_client.dart';
import 'model/api_error.dart';

typedef ApiResultFactory<T> = T Function(Map<String, dynamic> json);

class ApiClient {
  ApiClient._()
      : httpClient = HttpClient(),
        config = Config.instance;

  static final instance = ApiClient._();

  final HttpClient httpClient;
  final Config config;

  Future<T> get<T>(
    String url, {
    Map<String, String>? headers,
    Map<String, dynamic>? queryParams,
    required ApiResultFactory<T> resultFactory,
  }) async {
    http.Response response;

    try {
      final uri = _makeUri(
        url: url,
        queryParams: queryParams ?? {},
      );

      response = await httpClient.get(
        uri,
        headers: headers,
      );
    } catch (e) {
      return Future.error(e);
    }

    if (response.statusCode == 200) {
      return resultFactory(
        jsonDecode(
          response.body,
        ),
      );
    } else {
      return Future<T>.error(_makeResponseError<T>(
        response,
      ));
    }
  }

  Uri _makeUri({
    required String url,
    required Map<String, dynamic> queryParams,
  }) {
    final _apiKeyAsParam = _getApiKeyAsQueryParam();

    return Uri.parse(url).replace(
      queryParameters: queryParams
        ..addAll(
          _apiKeyAsParam,
        ),
    );
  }

  Map<String, dynamic> _getApiKeyAsQueryParam() => {'api_key': config.apiKey};

  ApiError<T> _makeResponseError<T>(
    http.Response response,
  ) {
    try {
      return ApiError.fromJson(
        jsonDecode(
          response.body,
        ),
      );
    } catch (_) {
      return ApiError<T>(
        statusCode: 0,
        statusMessage: 'Unexpected error',
      );
    }
  }
}
