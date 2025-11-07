export interface HttpRepository {
  get: <T>(url: string, options?: Record<string, unknown>) => Promise<T>;
}
