abstract class IFailure {
  final String message;

  IFailure(this.message);
}

class Failure extends IFailure {
  Failure(super.message);
}

class ConnectionFailure extends Failure {
  ConnectionFailure()
      : super(
            "You seem to be offline. Please check your internet connection and try again.");
}
