import { UndefinedInitialDataOptions } from "@tanstack/react-query";

export type QueryOptionsWithoutKeyAndFn<T> = Omit<UndefinedInitialDataOptions<T>, "queryKey" | "queryFn">;
