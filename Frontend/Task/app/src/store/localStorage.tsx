export const FILTER = 'FILTER';
export const CURRENCY_PAIRS = 'CURRENCY_PAIRS';

export const saveToLocalStorage = (key: string, data: any) => {
  try {
    const serializeData = JSON.stringify(data);
    localStorage.setItem(key, serializeData);
    return true;
  } catch(e){
    return false;
  }
}

export const loadFromLocalStorage = (key: string) => {
  try {
    const serializeData = localStorage.getItem(key);
    if(serializeData === null) return [];
    return JSON.parse(serializeData);
  }catch(e){
    return [];
  }
}