export const queryStringBuilder = (ids: Array<string>) => {
  let idsArray = [...ids]
    .map(id => {
      return `currencyPairIds[]=${id}`;
    })
    .join("&");
  return idsArray;
};

export const loadState = (key: string) => {
  try {
    const serializedState = localStorage.getItem(key);
    if (serializedState === null) {
      return undefined;
    }
    return JSON.parse(serializedState);
  } catch (err) {
    return undefined;
  }
};

export const saveState = (key: string, state: string) => {
  const serializedState = JSON.stringify(state);
  localStorage.setItem(key, serializedState);
};

export const namesArray = [
  { label: "Show all", value: "" },
  {
    label: "Armenia Dram / Georgia Lari",
    value: "70c6744c-cba2-5f4c-8a06-0dac0c4e43a1"
  },
  {
    label: "Philippines Peso / Algeria Dinar",
    value: "41cae0fd-b74d-5304-a45c-ba000471eabd"
  },
  {
    label: "Lesotho Loti / Australia Dollar",
    value: "5b428ac9-ec57-513d-8a08-20199469fb4d"
  },
  {
    label: "Bangladesh Taka / Venezuela Bolivar",
    value: "f816e384-0e43-5ce7-a017-deaa8d666774"
  },
  {
    label: "Qatar Riyal / Jordan Dinar",
    value: "5b98842f-bfe5-5564-b321-068763d7e2a3"
  },
  {
    label: "Suriname Dollar / Czech Republic Koruna",
    value: "a2bda952-4369-5d41-9d0b-e6c9947e5b82"
  },
  {
    label: "Czech Republic Koruna / Lesotho Loti",
    value: "1993f7b9-f9be-551a-beac-312d6befd0cf"
  },
  {
    label: "Algeria Dinar / Maldives (Maldive Islands) Rufiyaa",
    value: "61fb0e0d-626e-5e0a-831a-ef95d5c32203"
  },
  {
    label: "Burundi Franc / Chile Peso",
    value: "b7fdd67f-5051-58b7-a3c6-84f5da637df5"
  },
  {
    label: "Hong Kong Dollar / Falkland Islands (Malvinas) Pound",
    value: "611398c5-6bd9-596e-8803-3ed0b093995d"
  }
];
