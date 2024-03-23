export const getMovieList = async (searchTerm: string, pageParam = 1) => {
    const apiKey = process.env.NEXT_PUBLIC_API_KEY;

    const options = {
        method: 'GET',
        headers: {
            'accept': 'application/json',
            'Authorization': `Bearer ${apiKey}`
        }
    };

    const response = await fetch(
        `https://api.themoviedb.org/3/search/movie?query=${searchTerm}&api_key=${apiKey}&page=${pageParam}`,
        options
    );

    if (!response.ok) {
        throw new Error('Network response was not ok');
    }

    const responseData = await response.json();
    return responseData;
};

export const getMovieDetails = async (id: string) => {
    const apiKey = process.env.NEXT_PUBLIC_API_KEY;
    const options = {
        method: 'GET',
        headers: {
            'accept': 'application/json',
            'Authorization': `Bearer ${apiKey}`
        }
    };

    const response = await fetch(
        `https://api.themoviedb.org/3/movie/${id}?api_key=${apiKey}`,
        options
    );

    if (!response.ok) {
        throw new Error('Network response was not ok');
    }

    const responseData = await response.json();
    return responseData;
};
