export const queryStringBuilder = (ids) => {
  let idsArray = [...ids].map(id => {
    return `currencyPairIds[]=${id}`
  }).join("&")
  return idsArray;
}

export const setTrend = (prev, cur) => {
  if(cur > prev) {
    return "growing"
  } else if(cur < prev) {
    return "declining"
  } else if(cur === prev) {
    return "stagnating"
  }
}