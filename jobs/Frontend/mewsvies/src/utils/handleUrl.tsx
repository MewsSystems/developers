const API_KEY = process.env.REACT_APP_API_KEY;
const BASE_URL = process.env.REACT_APP_BASE_URL;

export function handleURL(term: string, page = 1) {
    // ! TODO: remove this before production
    console.log("handleURL => pageNumber: ", page);

    if (term.trim().length === 0)
        return `${BASE_URL}/movie/popular?api_key=${API_KEY}&language=en-US&page=${page}`;

    return `${BASE_URL}/search/movie?api_key=${API_KEY}&query=${term}&language=en-US&page=${page}`;
}
