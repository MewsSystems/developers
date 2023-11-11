import dayjs from "dayjs";

const DATE_FORMAT = {
  withLongMonth: "MMMM DD, YYYY",
  withDateAndTime: "DD.mm.YYYY. hh:mm",
};

export const formatDate = (
  date: Date | string,
  format: keyof typeof DATE_FORMAT
) => dayjs(date).format(DATE_FORMAT[format]);

export const getCurrentTimezone = () =>
  Intl.DateTimeFormat().resolvedOptions().timeZone;
