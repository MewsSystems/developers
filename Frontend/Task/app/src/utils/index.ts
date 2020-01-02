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
    label: "Mozambique Metical / Ghana Cedi",
    value: "f70c6744-c2cb-5a28-b4c6-5aa0680dac0c"
  },
  {
    label: "Bosnia and Herzegovina Convertible Marka / Albania Lek",
    value: "43a1ae34-1cae-50fd-bb74-d13042a845c2"
  },
  {
    label: "Mongolia Tughrik / Colombia Peso",
    value: "000471ea-bd5d-525b-828a-c91ec57e13d3"
  },
  {
    label: "Moldova Leu / Fiji Dollar",
    value: "a0872019-9469-5fb4-a3a0-f816e384f0e4"
  },
  {
    label: "Cambodia Riel / Guatemala Quetzal",
    value: "ce7cba01-71de-5aa8-a666-774b9f5b9884"
  },
  {
    label: "Netherlands Antilles Guilder / Kenya Shilling",
    value: "bbfe5f56-45e9-5321-a068-763d7e2a361b"
  },
  {
    label: "Brunei Darussalam Dollar / Tuvalu Dollar",
    value: "bda952f4-369c-5d41-87dd-0b4e6c9947e5"
  },
  {
    label: "Djibouti Franc / Pakistan Rupee",
    value: "27dc1993-f7b9-5ff9-aed5-1a8f8eac3312"
  },
  {
    label: "Iceland Krona / Aruba Guilder",
    value: "befd0cfd-3361-5fb0-b0de-626ece0ae403"
  },
  {
    label: "Bahrain Dinar / Suriname Dollar",
    value: "aef95d5c-3220-53d4-8b7f-dd67f2505138"
  }
];
