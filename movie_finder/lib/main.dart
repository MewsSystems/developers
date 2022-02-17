import 'package:flutter/material.dart';
import 'package:movie_finder/app.dart';
import 'package:movie_finder/data/config.dart';

void main() async {
  await _runPreTasks();

  runApp(const App());
}

Future<void> _runPreTasks() async {
  WidgetsFlutterBinding.ensureInitialized();
  final config = Config.instance;
  await config.init();
}
