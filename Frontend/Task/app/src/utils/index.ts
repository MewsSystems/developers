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
  {name: "Show all", value: ""},
  {name: "Armenia Dram / Georgia Lari", value: "Armenia Dram / Georgia Lari"},
  {name: "Philippines Peso / Algeria Dinar", value: "Philippines Peso / Algeria Dinar"},
  {name:  "Lesotho Loti / Australia Dollar", value:  "Lesotho Loti / Australia Dollar"},
  {name: "Bangladesh Taka / Venezuela Bolivar", value: "Bangladesh Taka / Venezuela Bolivar"},
  {name:  "Qatar Riyal / Jordan Dinar", value:  "Qatar Riyal / Jordan Dinar"},
  {name: "Suriname Dollar / Czech Republic Koruna", value: "Suriname Dollar / Czech Republic Koruna"},
  {name: "Czech Republic Koruna / Lesotho Loti", value: "Czech Republic Koruna / Lesotho Loti"},
  {name: "Algeria Dinar / Maldives (Maldive Islands) Rufiyaa", value: "Algeria Dinar / Maldives (Maldive Islands) Rufiyaa"},
  {name: "Burundi Franc / Chile Peso", value: "Burundi Franc / Chile Peso"},
  {name: "Hong Kong Dollar / Falkland Islands (Malvinas) Pound", value: "Hong Kong Dollar / Falkland Islands (Malvinas) Pound"}

]