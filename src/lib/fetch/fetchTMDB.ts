const token = process.env.MOVIE_DB_ACCESS_TOKEN;
if (!token) {
  throw new Error(
    'Missing MOVIE_DB_ACCESS_TOKEN in environment variables. Please define it in .env.local'
  );
}

export async function fetchTMDB(path: string, revalidate: number): Promise<Response> {
  return fetch(`https://api.themoviedb.org/3${path}`, {
    headers: {
      Accept: 'application/json',
      Authorization: `Bearer ${token}`,
    },
    next: { revalidate },
  });
}
