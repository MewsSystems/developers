import 'dart:developer';

import 'package:dio/dio.dart';

/// {@template client}
/// A wrapper around the [Dio] package to reduce boilerplate codes.
/// {@endtemplate}
class Client {
  final Dio _dio = Dio();

  /// Handy method to make get request
  dynamic get(String endpoint) async {
    Response response = await _dio.get(endpoint);
    return response;
  }
}
