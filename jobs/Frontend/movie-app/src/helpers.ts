export const formatDate = (inputDate: string): string => {
    const options: Intl.DateTimeFormatOptions = {
        month: "short",
        day: "numeric",
        year: "numeric",
    };

    const formattedDate = new Date(inputDate).toLocaleDateString(
        "en-US",
        options
    );
    return formattedDate;
};

const BASE_URL = 'https://api.themoviedb.org/3';

export const createFetchUrl = (path: string, params: Record<string, any>) => {
    const queryParams = new URLSearchParams({
        api_key: process.env.REACT_APP_MOVIE_API_KEY ?? '03b8572954325680265531140190fd2a',
        ...params,
    });

    return `${BASE_URL}/${path}?${queryParams.toString()}`;
}