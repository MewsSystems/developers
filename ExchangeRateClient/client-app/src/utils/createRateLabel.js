// @flow

export default (currencies: Array<{ code: string, name: string }>) =>
  `${currencies[0].code} - ${currencies[1].code}`;
