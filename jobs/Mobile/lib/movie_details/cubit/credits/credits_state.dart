part of 'credits_cubit.dart';

enum CreditsStatus { init, loading, success, failure }

class CreditsState extends Equatable {
  const CreditsState({required this.status, required this.membersList});

  final CreditsStatus status;
  final List<Member> membersList;

  @override
  List<Object> get props => [status, membersList];

  CreditsState copyWith({
    CreditsStatus? status,
    List<Member>? membersList,
  }) =>
      CreditsState(
        status: status ?? this.status,
        membersList: membersList ?? this.membersList,
      );
}
