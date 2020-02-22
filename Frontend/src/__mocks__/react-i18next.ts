import { TransProps } from 'react-i18next'

interface UseTranslationInt extends Array<any> {
  t?: (k: any) => any
  i18n?: any
}

const t = (key: any) => key
const i18n = {
  language: 'en',
  languages: ['en'],
  t,
}
const useMock: UseTranslationInt = [t, i18n]
useMock.t = t
useMock.i18n = i18n

export const useTranslation = () => useMock
export const Trans = (props: TransProps): any =>
  `${props.i18nKey} (${props.values})`
export const getI18n = () => i18n

export default {
  useTranslation,
  Trans,
  getI18n,
}
