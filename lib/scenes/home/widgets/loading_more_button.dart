import 'package:flutter/material.dart';

class LoadMoreButton extends StatelessWidget {
  final String title;
  final Function? onPressed;

  const LoadMoreButton({required this.title, this.onPressed, Key? key})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        Expanded(
          child: Padding(
            padding: const EdgeInsets.symmetric(horizontal: 20.0, vertical: 10),
            child: ElevatedButton(
              style: const ButtonStyle(
                backgroundColor: MaterialStatePropertyAll(Colors.red),
                padding:
                    MaterialStatePropertyAll(EdgeInsets.symmetric(vertical: 6)),
              ),
              onPressed: () => onPressed?.call(),
              child: Text(title),
            ),
          ),
        )
      ],
    );
  }
}
