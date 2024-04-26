import { HttpRepository } from '@/modules/movies/domain/HttpRepository';

export class FetchHttpRepository implements HttpRepository {
  async get<T>(url: string, options?: Record<string, unknown>): Promise<T> {
    try {
      const generatedUrl = options
        ? `${url}?${this.parseOptionsToQueryString(options)}`
        : url;
      const response = await fetch(generatedUrl);

      if (!response.ok) {
        throw new Error('Response not ok');
      }

      return response.json() as T;
    } catch (error) {
      throw new Error('Failed to fetch data');
    }
  }

  private parseOptionsToQueryString = (
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    options: Record<string, any>,
  ): string => {
    return new URLSearchParams(Object.entries(options)).toString();
  };
}
