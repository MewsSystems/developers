import 'package:http/http.dart' as http;

final client = http.Client();
const baseUrl = 'api.themoviedb.org';
const apiKey = '03b8572954325680265531140190fd2a';

Uri getApiUri(String path, {Map<String, dynamic>? parameters}) => Uri.https(
      baseUrl,
      path,
      {'api_key': apiKey, if (parameters != null) ...parameters},
    );
