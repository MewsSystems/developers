import { basePostApi } from "@/shared/api/baseApi";

type SessionResponse = {
  success: boolean;
  session_id: string;
};

export async function createSessionApi(
  requestToken: string
): Promise<SessionResponse> {
  return basePostApi({
    version: "3",
    payload: { request_token: requestToken },
    path: "authentication/session/new",
  });
}
