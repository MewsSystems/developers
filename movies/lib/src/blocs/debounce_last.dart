import 'dart:async';

import 'package:flutter_bloc/flutter_bloc.dart';

/// EventTransformer that keeps the latest event and debounces it for a [duration]
EventTransformer<Event> debounceLast<Event>(Duration duration) =>
    (events, mapper) =>
        events.transform(_DebounceLastStreamTransformer(mapper, duration));

class _DebounceLastStreamTransformer<T> extends StreamTransformerBase<T, T> {
  _DebounceLastStreamTransformer(this.mapper, this.duration);

  final EventMapper<T> mapper;
  final Duration duration;

  @override
  Stream<T> bind(Stream<T> stream) {
    late StreamSubscription<T> subscription;
    StreamSubscription<T>? mappedSubscription;
    T? latestEvent;

    final controller = StreamController<T>(
      onCancel: () async {
        await mappedSubscription?.cancel();

        return subscription.cancel();
      },
      sync: true,
    );

    subscription = stream.listen(
      (data) {
        latestEvent = data;
        if (mappedSubscription != null) {
          // Cancel the current mapped subscription
          // if there is a new event while it is still active
          mappedSubscription?.cancel();
        }

        Timer(duration, () {
          if (latestEvent != null) {
            final Stream<T> mappedStream;
            mappedStream = mapper(latestEvent as T);
            mappedSubscription = mappedStream.listen(
              controller.add,
              onError: controller.addError,
              onDone: () {
                if (latestEvent != null) {
                  controller.add(latestEvent as T);
                }
                // Set mappedSubscription and latestEvent to null
                // only after the current mapping is complete
                mappedSubscription = null;
                latestEvent = null;
                //controller.close();
              },
            );
          }
        });
      },
      onError: controller.addError,
      onDone: () => mappedSubscription ?? controller.close(),
    );

    return controller.stream;
  }
}
