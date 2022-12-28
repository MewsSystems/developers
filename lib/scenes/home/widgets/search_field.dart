import 'package:flutter/material.dart';

class SearchField extends StatelessWidget {
  final Function(String)? onChange;
  final TextEditingController controller;
  const SearchField({required this.controller, this.onChange, Key? key})
      : super(key: key);

  OutlineInputBorder get defaultOutlineInputBorder => OutlineInputBorder(
        borderRadius: BorderRadius.circular(20),
        borderSide: const BorderSide(),
      );

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: 50,
      child: Row(
        children: [
          Expanded(
            child: Padding(
              padding:
                  const EdgeInsets.symmetric(horizontal: 8.0, vertical: 8.0),
              child: TextField(
                controller: controller,
                autocorrect: false,
                cursorColor: Colors.black,
                decoration: InputDecoration(
                  hintText: "Search for movies...",
                  filled: true,
                  prefixIcon: const Icon(Icons.search, color: Colors.black),
                  fillColor: Colors.grey,
                  contentPadding:
                      const EdgeInsets.symmetric(vertical: 4, horizontal: 8),
                  border: defaultOutlineInputBorder,
                  enabledBorder: defaultOutlineInputBorder,
                  focusedBorder: defaultOutlineInputBorder,
                ),
                onChanged: (value) => onChange?.call(value),
              ),
            ),
          ),
        ],
      ),
    );
  }
}
