export const getApiKey = (customErrorMessage?: string) => {
  const apiKey = process.env.TMDB_API_KEY

  if (apiKey === undefined || apiKey === "") {
    const message = customErrorMessage ?? "Action is not authorized"
    throw new Response(message, { status: 401 })
  }

  return apiKey
}
