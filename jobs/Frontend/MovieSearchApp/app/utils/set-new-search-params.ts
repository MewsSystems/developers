export const setNewSearchParams = (
  searchParams: URLSearchParams,
  ...newParams: Record<string, string>[]
) => {
  const newSearchParams = new URLSearchParams(searchParams)

  newParams.forEach((param) => {
    for (const key in param) {
      newSearchParams.set(key, param[key])
    }
  })

  return newSearchParams
}
