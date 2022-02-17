import 'dart:convert';
import 'package:flutter/services.dart' show rootBundle;

const _kApiKey = 'apiKey';

class Config {
  Config._();
  static final instance = Config._();

  Future<void> init() async {
    apiKey = await _getApiKey();
  }

  late String apiKey;

  Future<String> _getApiKey() async {
    try {
      final configSource = await rootBundle.loadString('assets/config/config.json');
      final configJson = jsonDecode(configSource);
      final apiKey = configJson?[_kApiKey];
      if (apiKey == null) {
        throw 'ApiKey is not provided';
      }
      return apiKey;
    } catch (_) {
      throw Exception('Unable to load api key');
    }
  }
}
