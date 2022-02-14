import i18n from 'i18next';
import {initReactI18next} from 'react-i18next';

import translationEN from './locales/en-US.json';

const splitJson = (obj: any) => {
  Object.keys(obj).forEach(function (k) {
    const prop = k.split('.');

    const last = prop.pop();
    prop.reduce(function (o, key) {
      return (o[key] = o[key] || {});
    }, obj)[last!] = obj[k];
    delete obj[k];
  });

  return obj;
};

const enResources = {
  'en-US': {
    translation: splitJson(translationEN),
  },
};

i18n.use(initReactI18next).init({
  resources: enResources,
  fallbackLng: 'en-US',
  debug: true,

  interpolation: {
    escapeValue: false,
  },
});

export default i18n;
