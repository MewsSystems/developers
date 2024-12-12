export namespace tmdbService {
  const KEY = `${process.env.REACT_APP_TMDB_AUTH_KEY}`;
  const BASE_URL = `https://api.themoviedb.org`;

  export async function get<V>(url: string, isGetById?: boolean) {
    const options = {
      method: "GET",
      headers: {
        accept: "application/json",
        Authorization: `Bearer ${KEY}`,
      },
    };

    const completeUrl = BASE_URL + url;
    console.log(`${completeUrl}?api_key=${KEY}`);

    const response: Response = await fetch(
      `${completeUrl}${isGetById ? "?" : "&"}api_key=${KEY}`,
      options
    );
    const result: V = await response.json();
    return result;
  }
}
