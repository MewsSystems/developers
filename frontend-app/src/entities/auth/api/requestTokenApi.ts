import { baseGetApi } from "@/shared/api/baseApi";

type RequestTokenResponse = {
  success: boolean;
  request_token: string;
};

export async function requestTokenApi(): Promise<
  RequestTokenResponse | undefined
> {
  return baseGetApi({ version: "3", path: "authentication/token/new" });
}
