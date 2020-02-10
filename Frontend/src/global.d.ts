declare type PromiseReturnType<T> = T extends PromiseLike<infer U> ? U : T
