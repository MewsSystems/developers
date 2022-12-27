import 'package:http/http.dart' as http;

/// HTTP client used for the API calls
final client = http.Client();

/// Base API URL
const baseUrl = 'api.themoviedb.org';

/// API key to include as a request parameter
const apiKey = '03b8572954325680265531140190fd2a';

/// Generate the Uri for an endpoint's [path], including the query [parameters]
Uri getApiUri(String path, {Map<String, dynamic>? parameters}) => Uri.https(
      baseUrl,
      path,
      {'api_key': apiKey, if (parameters != null) ...parameters},
    );
