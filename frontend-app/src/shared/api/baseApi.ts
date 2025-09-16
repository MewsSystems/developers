export async function baseGetApi<T>({ path, params, version }: { path: string, params: Record<string, string>, version: string }): Promise<T | undefined> {
    const API_KEY = getApiKey();
    const searchParams = new URLSearchParams({ api_key: API_KEY });
    Object.entries(params).forEach(([key, value]) => {
        searchParams.append(key, value);
    })

    const url = `https://api.themoviedb.org/${version}/${path}?${searchParams.toString()}`;
    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json")
    myHeaders.append("accept", "application/json")

    try {
        const response = await fetch(url, {
            method: "GET",
            headers: myHeaders
        });
        if (!response.ok) {
            throw new Error(`Response status: ${response.status}`);
        }
        return await response.json();
    } catch (error: any) {
        console.error(error?.message);
        return undefined
    }

}


export function getBaseUrlApi({ version }: { version: string }): string {
    return `https://api.themoviedb.org/${version}`
}

export function getApiKey() {
    return import.meta.env["VITE_TMDB_API_KEY"]
}