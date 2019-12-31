export const queryStringBuilder = (ids) => {
  let idsArray = [...ids].map(id => {
    return `currencyPairIds[]=${id}`
  }).join("&")
  return idsArray;
}