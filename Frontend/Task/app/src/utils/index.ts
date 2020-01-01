export const queryStringBuilder = (ids: Array<string>) => {
  let idsArray = [...ids].map(id => {
    return `currencyPairIds[]=${id}`
  }).join("&")
  return idsArray;
}

export const setTrend = (prev: number, cur: number) => {
  if(cur > prev) {
    return "growing"
  } else if(cur < prev) {
    return "declining"
  } else if(cur === prev) {
    return "stagnating"
  }
}

export const loadState = (key: string) => {
  try {
    const serializedState = localStorage.getItem(key);
    if (serializedState === null || serializedState === undefined) {
      return undefined;
    }
    return JSON.parse(serializedState);
  } catch (err) {
    return undefined;
  }
};

export const saveState = (key: string, state: object) => {
  const serializedState = JSON.stringify(state);
  localStorage.setItem(key, serializedState);
};

export const namesArray = [
 "Armenia Dram / Georgia Lari",
 "Philippines Peso / Algeria Dinar",
 "Lesotho Loti / Australia Dollar",
 "Bangladesh Taka / Venezuela Bolivar",
 "Qatar Riyal / Jordan Dinar",
 "Suriname Dollar / Czech Republic Koruna",
 "Czech Republic Koruna / Lesotho Loti",
 "Algeria Dinar / Maldives (Maldive Islands) Rufiyaa",
 "Burundi Franc / Chile Peso",
 "Hong Kong Dollar / Falkland Islands (Malvinas) Pound"
]