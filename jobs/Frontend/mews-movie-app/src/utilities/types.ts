export type ElementOf<T> = T extends Array<infer U> ? U : never;
