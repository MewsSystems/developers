import 'package:flutter/material.dart';

class VoteIndicator extends StatelessWidget {
  final double? progress;
  final int? voteCount;

  const VoteIndicator({this.progress, this.voteCount, Key? key})
      : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Padding(
          padding: const EdgeInsets.symmetric(horizontal: 8.0, vertical: 4.0),
          child: Stack(
            children: [
              const CircularProgressIndicator(
                color: Colors.black,
                value: 1,
              ),
              if (progress != null)
                CircularProgressIndicator(
                  value: progress,
                  color: Colors.red,
                ),
              if (progress != null)
                Positioned.fill(
                  child: Align(
                    alignment: Alignment.center,
                    child: Text((progress! * 100).toStringAsFixed(0)),
                  ),
                ),
            ],
          ),
        ),
        if (voteCount != null)
          Text('$voteCount votes',
              style: Theme.of(context)
                  .textTheme
                  .bodyText2
                  ?.copyWith(fontSize: 10)),
      ],
    );
  }
}
