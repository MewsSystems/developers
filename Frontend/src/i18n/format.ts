import { format as dateFormat } from 'date-fns'

export const format = (value: any, format: any) => {
  if (value instanceof Date) {
    return dateFormat(value, String(format))
  }
  return value
}
