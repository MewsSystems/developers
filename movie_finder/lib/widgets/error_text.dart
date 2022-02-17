import 'package:flutter/material.dart';
import 'package:movie_finder/data/model/api_error.dart';

class ErrorText extends StatelessWidget {
  const ErrorText({
    Key? key,
    this.error,
  }) : super(key: key);
  final Object? error;

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Text(
        _makeErrorDescription(),
        style: Theme.of(context).textTheme.bodyMedium,
      ),
    );
  }

  String _makeErrorDescription() {
    final _defaultErrorDesc = 'Something went wrong';

    if (error is ApiError) {
      return (error as ApiError).statusMessage ?? _defaultErrorDesc;
    } else {
      return _defaultErrorDesc;
    }
  }
}
