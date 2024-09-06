type Month =
  "01" | "02" | "03" | "04" | "05" | "06" | "07" | "08" | "09" | "10" | "11" | "12";
type DayOfTheMonth =
  "01" | "02" | "03" | "04" | "05" | "06" | "07" | "08" | "09" | "10" |
  "11" | "12" | "13" | "14" | "15" | "16" | "17" | "18" | "19" | "20" |
  "21" | "22" | "23" | "24" | "25" | "26" | "27" | "28" | "29" | "30" | "31";

/** 1993-12-12 format dates */
export type DateString = `${string}-${Month}-${DayOfTheMonth}` // note: the year is too complex too type better for compiler
