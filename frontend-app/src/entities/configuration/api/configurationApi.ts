import { baseGetApi } from "@/shared/api/baseApi";
import type { Configuration } from "@/entities/configuration/types";

export async function getConfiguration() {
    return baseGetApi<Configuration>({ version: "3", path: `configuration`, params: {} })
}
