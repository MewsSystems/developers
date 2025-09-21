import { baseGetApi } from "@/shared/api/baseApi";
type AccoundResponse = {
  name: string;
  username: string;
  id: string;
};

export async function getAccountApi({
  session_id,
}: {
  session_id: string;
}): Promise<AccoundResponse | undefined> {
  return baseGetApi({
    version: "3",
    path: "account",
    params: {
      session_id,
    },
  });
}
